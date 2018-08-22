// -----------------------------------------------------------------------
// <copyright file="NoteViewModel.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel.Note
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public class NoteViewModel
    {
        public DateTime MoveBetsDate { get; set; }

        public IList<Product> ProductList { get; set; }
    }
}