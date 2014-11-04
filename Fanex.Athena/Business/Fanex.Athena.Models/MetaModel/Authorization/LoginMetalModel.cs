// -----------------------------------------------------------------------
// <copyright file="LoginMetalModel.cs" company="Nexel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Authorization
{
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

        public string LogInToken { get; set; }

        public string LoginSalt { get; set; }
    }
}