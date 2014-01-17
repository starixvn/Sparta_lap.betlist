// -----------------------------------------------------------------------
// <copyright file="ProductOptionMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Product
{
    /// <summary>
    /// ProductOptionMetaModel class.
    /// </summary>
    public class ProductOptionMetaModel
    {
        public bool ShowSb { get; set; }

        public bool ShowRb { get; set; }

        public bool ShowNg { get; set; }

        public bool ShowBa { get; set; }

        public bool ShowLc { get; set; }

        public int LabelSpan { get; set; }

        public int ListSpan { get; set; }
    }
}