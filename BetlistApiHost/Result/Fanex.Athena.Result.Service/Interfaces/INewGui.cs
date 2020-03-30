using System;

namespace Fanex.Athena.Result.Service.Interfaces
{
    public interface INewGui
    {
        string CreateNewId();
    }

    public class NewGui : INewGui
    {
        public string CreateNewId()
            => Guid.NewGuid().ToString();
    }
}