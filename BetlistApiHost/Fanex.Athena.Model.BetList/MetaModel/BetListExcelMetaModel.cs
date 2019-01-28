namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class BetListExcelMetaModel : BetListMetaModel
    {
        public string Title { get; set; }

        public ShowingOption ShowingOption { get; set; }

        public int[] HiddenColumns { get; set; }

        public int StartIndex { get; set; } = 1;
    }
}