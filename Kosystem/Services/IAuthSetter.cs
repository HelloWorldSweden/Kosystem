namespace Kosystem.Services
{
    public interface IAuthSetter
    {
        void Login();

        void Logout();

        bool IsLoggedIn();
    }
}
