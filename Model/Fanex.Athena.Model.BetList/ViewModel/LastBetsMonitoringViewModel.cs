namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class LastBetsMonitoringViewModel : BetListBaseViewModel
    {
        public IEnumerable<SelectListItem> SportsList { get; set; }
    }
}