using System;
using System.Data;

namespace Fanex.Athena.Service.Casino.Interfaces
{
    public interface IReportApiWrapper
    {
        DataTable[] WinLossDetail(int userBetId);

        DataTable WinLossHandHistory(long refNo, string betType, string casinoNo, bool isShowPic);

        DataTable RealTimeWinLossHandHistory(DateTime dateStart, DateTime dateEnd, int ibcCustID, string casinoNo, bool isShowPic);
    }
}