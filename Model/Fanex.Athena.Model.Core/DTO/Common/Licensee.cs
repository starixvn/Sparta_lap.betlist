// -----------------------------------------------------------------------
// <copyright file="Licensee.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Licensee class.
    /// </summary>
    [Serializable]
    public class Licensee : ISerializable
    {
        public Licensee()
        {
        }

        protected Licensee(SerializationInfo info, StreamingContext context)
        {
            IsActive = info.GetBoolean("IsActive");
            Id = info.GetInt32("Id");
            Name = info.GetString("Name");
            Description = info.GetString("Description");
            IsAllowedCustomerComm = info.GetBoolean("IsAllowedCustomerComm");
            IsEnableBA = info.GetBoolean("IsEnableBA");
            ResellerId = info.GetInt32("ResellerId");
            ResellerName = info.GetString("ResellerName");
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsAllowedCustomerComm { get; set; }

        public bool IsEnableBA { get; set; }

        public int ResellerId { get; set; }

        public string ResellerName { get; set; }

        #region ISerializable Members

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IsActive", IsActive);
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("Description", Description);
            info.AddValue("IsAllowedCustomerComm", IsAllowedCustomerComm);
            info.AddValue("IsEnableBA", IsEnableBA);
            info.AddValue("ResellerId", ResellerId);
            info.AddValue("ResellerName", ResellerName);
        }

        #endregion ISerializable Members
    }
}