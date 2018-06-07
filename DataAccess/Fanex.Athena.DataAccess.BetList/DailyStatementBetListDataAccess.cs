namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Interfaces;
    using Session.Interfaces;
    using Util;

    public class DailyStatementBetListDataAccess : BaseBetListDataAccess, IDailyStatementBetListDataAccess
    {
        private static readonly string[] TransTypeHasBetList = { "get", "com", "post", "bet", "gp" };

        private static readonly Dictionary<string, int> TransTypeIdMap = new Dictionary<string, int>
            {
                { "betting statement", 1 }, { "void/cancelled bet", 2 },
                { "outstanding bet", 3 }, { "grand prize", 4 }
            };

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public DailyStatementBetListDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage,
            IBalanceUpDataAccess balanceUpDataAccess)
            : base(appSetting, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public IEnumerable<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool allowCustComm)
        {
            const string UspKey = "Lrf_Statement_Mem";

            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.MoveBetsDay = balanceUp.MoveBetsDay;
            try
            {
                using (var db = this.GetObjectDb(UspKey))
                {
                    db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
                    BetListParameterBuilder.CreateBetListDateRangeParameters(db, metaModel);
                    BetListParameterBuilder.CreateBetlistBUParameters(db, metaModel);
                    BetListParameterBuilder.CreateBetListPagingParameters(db, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadData(metaModel, allowCustComm, reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspKey, metaModel);
            }
        }

        private static IEnumerable<DailyStatement> ReadData(BetListMetaModel metaModel, bool allowCustComm, IDataReader reader)
        {
            const string FormatRemark = "<span class='bl-underdog'>{0} <br/>- {1}</span>";
            var index = 1;
            var commissionColumn = "commission" + metaModel.BaseCurrency;
            var totalAmtColumn = "totalamt" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            decimal balance = 0;
            var currentCulture = CultureInfo.CurrentCulture;

            var result = new List<DailyStatement>();

            while (reader.Read())
            {
                var winlostDate = reader.ToDateTime("winlostdate");
                var transType = reader.ToString("transtype").ToLower(currentCulture);
                var transDesc = reader.ToString("transdesc");
                var transTypeId = TransTypeIdMap.ContainsKey(transDesc.ToLower(currentCulture)) ? TransTypeIdMap[transDesc.ToLower(currentCulture)] : 0;
                var credit = transDesc.ToLower(currentCulture) == "outstanding bet" || transType == "bal" ? 0 : reader.ToDecimal(totalAmtColumn);
                var comm = reader.ToDecimal(commissionColumn);

                // Add commission.
                if (!transType.Equals("gp"))
                {
                    balance = transType == "bal" ? reader.ToDecimal(totalAmtColumn) : balance + credit + (allowCustComm ? comm : 0);
                }

                result.Add(new DailyStatement
                {
                    TransType = transTypeId.ToString(),
                    CustId = metaModel.CustId,
                    ExternalId = metaModel.ExternalId ?? "0",
                    SystemId = metaModel.SystemId ?? "0",
                    FullStatementDate = $"{winlostDate:dddd} <br/> {winlostDate}",
                    TransTypeHasBetList = TransTypeHasBetList.Contains(transType),
                    Index = index++,
                    Date = winlostDate.ToShortDateString(),
                    Remark = string.Format(FormatRemark, transType.ToUpper(currentCulture), transDesc),
                    Turnover = NumberReport(reader.ToDecimal(turnoverColumn)),
                    CreditDebit = NumberReport(credit),
                    Commission = NumberReport(comm),
                    Balance = NumberReport(balance)
                });
            }

            return result;
        }

        protected static string NumberReport(object value)
        {
            var number = Convert.ToDecimal(value);
            return number >= 0 ? "<span class='bl-underdog'>" + $"{number:#,##0.00}" + "</span>"
                       : "<span class='bl-favorite'>" + $"{number:#,##0.00}" + "</span>";
        }
    }
}