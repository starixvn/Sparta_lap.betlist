namespace Fanex.Athena.Extension.Utility.Interfaces
{
    public interface IFormsAuthenticationService
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);
    }
}