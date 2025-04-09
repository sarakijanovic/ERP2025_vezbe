using ERP2024.Models;

namespace ERP2024.Helpers
{
    public interface IAuthHelper
    {
        public bool AuthenticateCreds(AuthCreds creds, bool isEmployee);
        public string GenerateJwt(AuthCreds creds, string role);
    }
}
