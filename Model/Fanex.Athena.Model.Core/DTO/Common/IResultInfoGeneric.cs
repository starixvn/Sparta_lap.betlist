namespace Fanex.Athena.Model.Core.DTO.Common
{
    public interface IResultInfo<T> : IResultInfo
    {
        new T ExtendedData { get; set; }
    }
}