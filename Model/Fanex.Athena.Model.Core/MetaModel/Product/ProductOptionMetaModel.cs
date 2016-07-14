// -----------------------------------------------------------------------
// <copyright file="ProductOptionMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.MetaModel.Product
{
    /// <summary>
    /// ProductOptionMetaModel class.
    /// </summary>
    public class ProductOptionMetaModel
    {
        public bool ShowSB { get; set; }

        public bool ShowRB { get; set; }

        public bool ShowNG { get; set; }

        public bool ShowBA { get; set; }

        public bool ShowLC { get; set; }

        public int LabelSpan { get; set; }

        public int ListSpan { get; set; }
    }
}