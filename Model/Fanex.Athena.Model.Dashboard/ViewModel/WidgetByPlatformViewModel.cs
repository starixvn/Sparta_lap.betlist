namespace Fanex.Athena.Model.Dashboard.ViewModel
{
    using System.Collections.Generic;

    public class WidgetByPlatformViewModel
    {
        public IList<KeyValuePair<string, decimal>> Data { get; set; }

        public decimal Total { get; set; }
    }
}