// -----------------------------------------------------------------------
// <copyright file="SettledMatchCountService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Constant.DataDomain;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class SettledMatchCountService : ISettledMatchCountService
    {
        private readonly ISettledMatchCountDataAccess _settledMatchCountDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public SettledMatchCountService()
        {
            this._settledMatchCountDataAccess = new SettledMatchCountDataAccess();
            _cacheRefAdapter = new CacheRefAdapter();
        }

        public SettledMatchCountService(ISettledMatchCountDataAccess settledMatchCountDataAccess, ICacheRefAdapter cacheRefAdapter)
        {
            this._settledMatchCountDataAccess = settledMatchCountDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public IList<SettledMatchCountViewModel> GetDataList(bool isShowAll, int licenseeId, ProductAccess producAccess)
        {
            var result = _settledMatchCountDataAccess.GetDataList(isShowAll, licenseeId);

            return FilterValidSports(result, producAccess);
        }

        private static bool IsValidSportType(int sportType, ProductAccess productAccess)
        {
            switch (sportType)
            {
                case (int)SportType.HorseRacing:
                case (int)SportType.Greyhound:
                case (int)SportType.Harness:
                    return productAccess.IsAllowRB;

                case (int)SportType.NumberGame:
                    return productAccess.IsAllowNG;

                case (int)SportType.VirtualSport:
                    return productAccess.IsAllowVS;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Mapping data.
        /// </summary>
        /// <param name="dtoData">The data list.</param>
        /// <returns>The view model.</returns>
        private IList<SettledMatchCountViewModel> ConvertDtoDataToViewData(IList<SettledMatchCount> dtoData)
        {
            var resultList = dtoData.Where(x => x.IsToday == 0)
                                    .Select(i => new SettledMatchCountViewModel
                                    {
                                        SportName = _cacheRefAdapter.GetSportName(i.SportType),
                                        YesterdayEntered = i.Entered,
                                        YesterdayNonEntered = i.NonEntered,
                                        TodayEntered =
                                                dtoData.Where(g => g.IsToday == 1 && g.SportType == i.SportType)
                                                       .Select(j => j.Entered)
                                                       .FirstOrDefault(),
                                        TodayNonEntered =
                                                dtoData.Where(g => g.IsToday == 1 && g.SportType == i.SportType)
                                                       .Select(j => j.NonEntered)
                                                       .FirstOrDefault()
                                    }).ToList();

            var resultList2 = dtoData.Where(x => x.IsToday == 1)
                                     .Select(i => new SettledMatchCountViewModel
                                     {
                                         SportName = _cacheRefAdapter.GetSportName(i.SportType),
                                         TodayEntered = i.Entered,
                                         TodayNonEntered = i.NonEntered,
                                         YesterdayEntered =
                                                 dtoData.Where(g => g.IsToday == 0 && g.SportType == i.SportType)
                                                        .Select(j => j.Entered)
                                                        .FirstOrDefault(),
                                         YesterdayNonEntered =
                                                 dtoData.Where(g => g.IsToday == 0 && g.SportType == i.SportType)
                                                        .Select(j => j.NonEntered)
                                                        .FirstOrDefault()
                                     }).ToList();

            foreach (var item in resultList2)
            {
                if (resultList.All(x => x.SportName != item.SportName))
                {
                    resultList.Add(item);
                }
            }

            return resultList;
        }

        private IList<SettledMatchCountViewModel> FilterValidSports(
         IEnumerable<SettledMatchCount> settledMatchCountList, ProductAccess productAccess)
        {
            var listAfterCheckProduct = settledMatchCountList.Where(item => IsValidSportType(item.SportType, productAccess)).ToList();

            return ConvertDtoDataToViewData(listAfterCheckProduct);
        }
    }
}