using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kosystem.Services
{
    /// <summary>
    /// Inspired by <see href="https://gist.github.com/SteveSandersonMS/175a08dcdccb384a52ba760122cd2eda"/>
    /// and <see href="https://stackoverflow.com/a/58390813"/>
    /// </summary>
    public class MyAuthenticationStateProvider : AuthenticationStateProvider, IAuthSetter
    {
        private bool isLoggedIn;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (isLoggedIn)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "Some fake user"),
                }, "Fake authentication type");

                var user = new ClaimsPrincipal(identity);
                return Task.FromResult(new AuthenticationState(user));
            }
            else
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            }
        }

        public bool IsLoggedIn()
        {
            return isLoggedIn;
        }

        public void Login()
        {
            isLoggedIn = true;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Logout()
        {
            isLoggedIn = false;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
