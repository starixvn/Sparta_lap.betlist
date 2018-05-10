namespace Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers
{
    using System.Collections.Generic;
    using NSubstitute;
    using Session.Interfaces;
    using Session.Model;

    public class ReportBaseDataAccessHelperTest<T> : BaseDataAccessHelperTest<T> where T : BaseDataAccess
    {
        protected const int CurrencyId = 123;

        protected const string CurrencyName = "aaaaaaaaa";

        protected ISessionStorage CreateSessionStorage()
        {
            var sessionStorage = Substitute.For<ISessionStorage>();
            sessionStorage.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { CurrencyId, CurrencyName }
                }
            });

            return sessionStorage;
        }
    }
}