// -----------------------------------------------------------------------
// <copyright file="GroupCommMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// GroupCommissionMetaModel class.
    /// </summary>
    public class GroupCommMetaModel
    {
        private const int MaxGroupNameLength = 100;

        public GroupCommMetaModel()
        {
            GroupColor = "#ffffff";
        }

        [Required]
        public int LicenseeId { get; set; }

        [Required]
        public int GroupCommId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewResources), ErrorMessageResourceName = "GroupNameNotEmptyMsg")]
        [StringLength(MaxGroupNameLength, ErrorMessageResourceType = typeof(ViewResources), ErrorMessageResourceName = "GroupNameNotExceed100CharactersMsg")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$",
         ErrorMessageResourceType = typeof(ViewResources), ErrorMessageResourceName = "GroupNameOnlyContainCharactersAndNumberMsg")]
        public string GroupName { get; set; }

        public string OriginalGroupName { get; set; }

        public int NumberOfCustomer { get; set; }

        [Required]
        public decimal SBComm { get; set; }

        public int SBComm1 { get; set; }

        public int SBComm2 { get; set; }

        public int SBComm3 { get; set; }

        [Required]
        public decimal ParlayComm { get; set; }

        public int ParlayComm1 { get; set; }

        public int ParlayComm2 { get; set; }

        public int ParlayComm3 { get; set; }

        [Required]
        public decimal NGComm { get; set; }

        public int NGComm1 { get; set; }

        public int NGComm2 { get; set; }

        public int NGComm3 { get; set; }

        [Required]
        public decimal CSComm { get; set; }

        public int CSComm1 { get; set; }

        public int CSComm2 { get; set; }

        public int CSComm3 { get; set; }

        [Required]
        public string GroupColor { get; set; }

        public bool IsDefaultGroup { get; set; }

        public int DefaultGroupId { get; set; }

        public bool IsResetDefault { get; set; }

        public bool IsUpdateNG { get; set; }

        public bool IsUpdateCS { get; set; }
    }
}