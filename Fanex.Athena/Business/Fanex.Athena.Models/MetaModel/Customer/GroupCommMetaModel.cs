// -----------------------------------------------------------------------
// <copyright file="GroupCommMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Customer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// GroupCommissionMetaModel class.
    /// </summary>
    public class GroupCommMetaModel
    {
        public GroupCommMetaModel()
        {
            GroupColor = "#ffffff";
        }

        [Required]
        public int LicenseeId { get; set; }

        [Required]
        public int GroupCommId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewResources), ErrorMessageResourceName = "GroupNameNotEmptyMsg")]
        [StringLength(100, ErrorMessageResourceType = typeof(ViewResources), ErrorMessageResourceName = "GroupNameNotExceed100CharactersMsg")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$",
         ErrorMessageResourceType = typeof(ViewResources), ErrorMessageResourceName = "GroupNameOnlyContainCharactersAndNumberMsg")]
        public string GroupName { get; set; }

        public int NumberOfCustomer { get; set; }

        [Required]
        public decimal SBComm { get; set; }

        public int SBComm1 { get; set; }

        public int SBComm2 { get; set; }

        public int SBComm3 { get; set; }

        [Required]
        public decimal RCComm { get; set; }

        public int RCComm1 { get; set; }

        public int RCComm2 { get; set; }

        public int RCComm3 { get; set; }

        [Required]
        public decimal NGComm { get; set; }

        public int NGComm1 { get; set; }

        public int NGComm2 { get; set; }

        public int NGComm3 { get; set; }

        [Required]
        public decimal LCComm { get; set; }

        public int LCComm1 { get; set; }

        public int LCComm2 { get; set; }

        public int LCComm3 { get; set; }

        [Required]
        public decimal CSComm { get; set; }

        public int CSComm1 { get; set; }

        public int CSComm2 { get; set; }

        public int CSComm3 { get; set; }

        [Required]
        public string GroupColor { get; set; }

        public bool IsUpdateRacing { get; set; }

        public bool IsDefaultGroup { get; set; }

        public int DefaultGroupId { get; set; }

        public bool IsResetDefault { get; set; }

        public bool IsUpdateNG { get; set; }

        public bool IsUpdateLC { get; set; }

        public bool IsUpdateCS { get; set; }

        /// <summary>
        /// Equals method.
        /// </summary>
        /// <param name="obj">Comparing object.</param>
        /// <returns>Two object equal or not.</returns>
        public override bool Equals(object obj)
        {
            var temp = obj as GroupCommMetaModel;
            if (temp != null)
            {
                return temp.GroupColor.Equals(GroupColor, StringComparison.OrdinalIgnoreCase)
                    && temp.GroupCommId == GroupCommId && temp.GroupName.Equals(GroupName)
                    && temp.SBComm == SBComm && temp.RCComm == RCComm && temp.CSComm == CSComm && temp.LicenseeId == LicenseeId;
            }
            // ReSharper disable BaseObjectEqualsIsObjectEquals
            return base.Equals(obj);
            // ReSharper restore BaseObjectEqualsIsObjectEquals
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}