namespace Fanex.Athena.Model.Core.DTO.OneAuth
{
    using System.Runtime.Serialization;

    [DataContract]
    public class OneAuthCurrency
    {
        [DataMember]
        public int[] AgentId { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }
    }
}