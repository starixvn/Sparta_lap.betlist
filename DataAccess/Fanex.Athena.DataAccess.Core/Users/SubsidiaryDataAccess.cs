namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Users.Interfaces;
    using Model.Report.DTO;

    public class SubsidiaryDataAccess : BaseDataAccess, ISubsidiaryDataAccess
    {
        public IEnumerable<Subsidiary> GetSubsidiaryList(string licenseeName)
        {
            var dataList = new List<Subsidiary>();

            const string SpName = "Lrf_SubsidiarySel";

            using (var objectDb = GetObjectDb(SpName))
            {
                objectDb.SetParameterValue("LicenseeName", licenseeName);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new Subsidiary
                        {
                            AgentId = Convert.ToInt32(reader["AgentID"]),
                            OperatorId = reader["OperatorID"].ToString(),
                            CurrencyId = Convert.ToInt32(reader["Currency"])
                        });
                    }
                }
            }

            return dataList;
        }
    }
}