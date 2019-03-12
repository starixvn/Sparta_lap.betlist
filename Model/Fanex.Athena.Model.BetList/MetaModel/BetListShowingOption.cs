namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class BetListShowingOption
    {
        public BetListShowingOption()
        {
            GrandTotalVisible = true;
            NGVisible = false;
            BAVisible = false;
            CSVisible = false;
            KNVisible = false;
            ABVisible = false;
        }

        public bool GrandTotalVisible { get; set; }

        public bool BAVisible { get; set; }

        public bool CSVisible { get; set; }

        public bool CustomerCommVisible { get; set; }

        public bool NGVisible { get; set; }

        public bool SBVisible { get; set; }

        public bool VSVisible { get; set; }

        public bool KNVisible { get; set; }

        public bool BTVisible { get; set; }

        public bool ABVisible { get; set; }
    }
}