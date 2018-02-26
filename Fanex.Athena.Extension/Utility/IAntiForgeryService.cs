namespace Fanex.Athena.Extension.Utility
{
    public interface IAntiForgeryService
    {
        void Validate();

        void Validate(string cookieToken, string formToken);
    }
}