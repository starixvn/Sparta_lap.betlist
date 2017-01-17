namespace Fanex.Athena.Model.Dashboard.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Model.Dashboard.DTO;

    public class TurnoverViewModel
    {
        public IList<Platform> Data { get; set; }

        public decimal Total => Data.Sum(p => p.Value);
    }
}
