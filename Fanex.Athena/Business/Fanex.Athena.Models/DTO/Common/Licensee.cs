// -----------------------------------------------------------------------
// <copyright file="Licensee.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Common
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
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsAllowedCustomerComm { get; set; }

        #region ISerializable Members

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IsActive", IsActive);
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("Description", Description);
            info.AddValue("IsAllowedCustomerComm", IsAllowedCustomerComm);
        }

        #endregion ISerializable Members
    }
}