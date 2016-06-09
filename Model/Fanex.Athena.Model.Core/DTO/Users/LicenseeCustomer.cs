// -----------------------------------------------------------------------
// <copyright file="LicenseeCustomer.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Users
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// LicenseeCustomer class.
    /// </summary>
    [Serializable]
    public class LicenseeCustomer : ISerializable
    {
        public LicenseeCustomer()
        {
        }

        protected LicenseeCustomer(SerializationInfo info, StreamingContext context)
        {
            CustId = info.GetInt32("CustId");
            RoleId = info.GetInt32("RoleId");
            Currency = info.GetInt32("Currency");
            Id = info.GetInt32("Id");
            Username = info.GetString("Username");
            Description = info.GetString("Description");
            CurrencyName = info.GetString("CurrencyName");
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public int CustId { get; set; }

        public int RoleId { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public string Description { get; set; }

        #region ISerializable Members

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CustId", CustId);
            info.AddValue("RoleId", RoleId);
            info.AddValue("Currency", Currency);
            info.AddValue("Id", Id);
            info.AddValue("Username", Username);
            info.AddValue("Description", Description);
            info.AddValue("CurrencyName", CurrencyName);
        }

        #endregion ISerializable Members
    }
}