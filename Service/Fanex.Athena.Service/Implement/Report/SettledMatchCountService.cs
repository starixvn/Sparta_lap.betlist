// -----------------------------------------------------------------------
// <copyright file="SettledMatchCountService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SettledMatchCountService : ISettledMatchCountService
    {
        #region Field

        private readonly SettledMatchCountDataAccess _settledMatchCountDataAccess;

        #endregion Field

        #region Constructor

        public SettledMatchCountService()
        {
            this._settledMatchCountDataAccess = new SettledMatchCountDataAccess();
        }

        #endregion Constructor

        public IList<SettledMatchCountViewModel> GetDataList(bool isShowAll, int licenseeId, ProductAccess producAccess)
        {
            var result = _settledMatchCountDataAccess.GetDataList(isShowAll, licenseeId);

            return this.GetDataListAfterCheckProductAccess(result, producAccess);
        }

        #region Utility

        #region Get data list after check product access

        /// <summary>
        /// Check acceptable product based on sport type.
        /// </summary>
        /// <param name="sportType">The sport type.</param>
        /// <param name="productAccess">The products object.</param>
        /// <returns>True or false.</returns>
        private static bool CheckProductAccess(int sportType, ProductAccess productAccess)
        {
            if (sportType == (int)SportType.HorseRacing)
            {
                return productAccess.IsAllowRB;
            }

            if (sportType == (int)SportType.NumberGame)
            {
                return productAccess.IsAllowNG;
            }

            if (sportType == (int)SportType.Greyhound)
            {
                return productAccess.IsAllowRB;
            }

            if (sportType == (int)SportType.Harness)
            {
                return productAccess.IsAllowRB;
            }

            if (sportType == (int)SportType.LiveCasino)
            {
                return productAccess.IsAllowLC;
            }

            if (sportType == (int)SportType.VirtualSport)
            {
                return productAccess.IsAllowVS;
            }

            return true;
        }

        #endregion Get data list after check product access

        #region Convert DTO data to View data for display

        /// <summary>
        /// Mapping data.
        /// </summary>
        /// <param name="dtoData">The data list.</param>
        /// <returns>The view model.</returns>
        private static List<SettledMatchCountViewModel> ConvertDtoDataToViewData(IList<SettledMatchCount> dtoData)
        {
            var resultList = dtoData.Where(x => x.IsToday == 0)
                                    .Select(i => new SettledMatchCountViewModel
                                    {
                                        SportName = CacheRef.GetSportName(i.SportType),
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
                                         SportName = CacheRef.GetSportName(i.SportType),
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

        #endregion Convert DTO data to View data for display

        private List<SettledMatchCountViewModel> GetDataListAfterCheckProductAccess(
         IEnumerable<SettledMatchCount> settledMatchCountList, ProductAccess productAccess)
        {
            var listAfterCheckProduct = settledMatchCountList.Where(item => CheckProductAccess(item.SportType, productAccess)).ToList();

            return ConvertDtoDataToViewData(listAfterCheckProduct);
        }

        #endregion Utility
    }
}