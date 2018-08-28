// -----------------------------------------------------------------------
namespace Fanex.Athena.Model.Report.ViewModel
{
    public class SpecialLicenseeWinLossGridViewModel : LicenseeWinLossGridViewModel
    {
        public long Ticks
        {
            get
            {
                return Date.Ticks;
            }
        }
    }
}