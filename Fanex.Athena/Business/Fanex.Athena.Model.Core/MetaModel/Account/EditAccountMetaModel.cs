// -----------------------------------------------------------------------
// <copyright file="EditAccountMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.MetaModel.Account
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// EditAccountMetaModel class.
    /// </summary>
    public class EditAccountMetaModel
    {
        public EditAccountMetaModel()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public int UserId { get; set; }

        [Display(Name = "Username", ResourceType = typeof(ViewResources))]
        public string UserName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(ViewResources))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(ViewResources))]
        public string LastName { get; set; }

        [Display(Name = "Status", ResourceType = typeof(ViewResources))]
        public string Status { get; set; }
    }
}