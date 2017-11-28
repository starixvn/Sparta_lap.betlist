using System;
using System.Data;
using CasinoLibrary.SBAgent;

namespace Fanex.Athena.Service.Casino.Wrappers
{
    public class ReportApiWrapper : IReportApiWrapper
    {
        private readonly Report _reportApi;

        public ReportApiWrapper()
        {
            _reportApi = new Report();
        }

        public DataTable[] WinLossDetail(int userBetId)
        {
            return _reportApi.WinLossDetail(userBetId);
        }

        public DataTable WinLossHandHistory(long refNo, string betType, string casinoNo, bool isShowPic)
        {
            return _reportApi.WinLossHandHistory(refNo, betType, casinoNo, isShowPic);
        }

        public DataTable RealTimeWinLossHandHistory(DateTime dateStart, DateTime dateEnd, int ibcCustID, string casinoNo, bool isShowPic)
        {
            return _reportApi.RealTimeWinLossHandHistory(dateStart, dateEnd, ibcCustID, casinoNo, isShowPic);
        }
    }
}