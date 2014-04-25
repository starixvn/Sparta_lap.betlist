// -----------------------------------------------------------------------
// <copyright file="LoginMetalModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoginMetalModel
    {
        public string UserName { get; set; }
        public string PassWord
        {
            get;
            set;
        }
        public string Captcha { get; set; }
    }
}
