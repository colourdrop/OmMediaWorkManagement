using OmMediaWorkManagement.Web.AuthModels;

namespace OmMediaWorkManagement.Web.AuthInterface
{
    public interface IAuthenticationService
    {
        Task<HttpResponseMessage> RegisterUser(UserRegistrationViewModel userForRegistration);
        Task<HttpResponseMessage> Login(LoginViewModel userForAuthentication);
        Task Logout();
    }
}
