// -----------------------------------------------------------------------
// <copyright file="ResetPasswordMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Account
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Athena.Models.ViewModel;

    /// <summary>
    /// ResetPasswordMetaModel class.
    /// </summary>
    public class ResetPasswordMetaModel
    {
        public int UserId { get; set; }

        [Display(Name = "Username", ResourceType = typeof(ViewResources))]
        public string UserName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(ViewResources))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(ViewResources))]
        public string LastName { get; set; }

        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        // ReSharper disable LocalizableElement
        [Required(ErrorMessage = "(*)")]
        // ReSharper restore LocalizableElement
        public string Password { get; set; }

        [Display(Name = "Confirm", ResourceType = typeof(ViewResources))]
        // ReSharper disable LocalizableElement
        [Required(ErrorMessage = "(*)")]
        // ReSharper restore LocalizableElement
        public string ConfirmPassword { get; set; }
    }
}