// -----------------------------------------------------------------------
// <copyright file="NumberGameBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Data;

    public class NumberGameBetSettingDataAccess : BaseDataAccess, INumberGameBetSettingDataAccess
    {
        public IList<BetSetting> GetBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelEditNG";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return RenderNumberGameBetSetting(reader);
                    }
                }
            }

            return new List<BetSetting>();
        }

        public IResultInfo UpdateBetSetting(NumberGameBetSetting param)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet161";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, param.CustId);
                db.AddInParameter("userid", DbType.Int32, param.UserId);
                db.AddInParameter("minbetv161", DbType.Decimal, param.MinBet);
                db.AddInParameter("maxbetv161", DbType.Decimal, param.MaxBet);
                db.AddInParameter("maxpermatchv161", DbType.Decimal, param.MaxPerMatch);
                db.AddInParameter("maxperballv161", DbType.Decimal, param.MaxPerBall);
                db.AddInParameter("language", DbType.String, param.Language);
                db.AddInParameter("ip", DbType.String, param.Ip);
                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        private static IList<BetSetting> RenderNumberGameBetSetting(IDataReader reader)
        {
            if (reader.FieldCount == 0)
            {
                return null;
            }

            var numberGameList = new List<BetSetting>();

            BetSettingHelper.RenderSport(reader, "161", numberGameList, true); // Soccer

            return numberGameList;
        }
    }
}