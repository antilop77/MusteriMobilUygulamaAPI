using MmuAPI.Models;

namespace MmuAPI
{
    public interface IJwtTokenManager
    {
        string Authenticate(cUserCredential pUserCredential);
    }
}
