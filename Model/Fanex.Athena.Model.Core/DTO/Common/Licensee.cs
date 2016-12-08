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
            info.AddValue(nameof(IsActive), IsActive);
            info.AddValue(nameof(Id), Id);
            info.AddValue(nameof(Name), Name);
            info.AddValue(nameof(Description), Description);
            info.AddValue(nameof(IsAllowedCustomerComm), IsAllowedCustomerComm);
            info.AddValue(nameof(IsEnableBA), IsEnableBA);
            info.AddValue(nameof(ResellerId), ResellerId);
            info.AddValue(nameof(ResellerName), ResellerName);
        }

        #endregion ISerializable Members
    }
}