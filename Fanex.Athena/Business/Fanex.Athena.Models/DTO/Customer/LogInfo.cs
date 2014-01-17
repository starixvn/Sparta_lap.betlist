// -----------------------------------------------------------------------
// <copyright file="LogInfo.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer
{
    using System;

    /// <summary>
    /// LogInfo class.
    /// </summary>
    public class LogInfo
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public DateTime? LastLogged { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LastTransferDate { get; set; }

        public DateTime? LastUpdateBA { get; set; }

        public string BAUpdatedBy { get; set; }

        public string SpecialLevelUpdateBy { get; set; }

        public DateTime? SpecialLevelUpdateDate { get; set; }
    }
}