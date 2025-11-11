using GleamVaultApi.DB;
using System.Security.Principal;

namespace GleamVaultApi.Extension
{
    public class UserIdentity : IIdentity
    {
        private readonly User _user;

        public UserIdentity(User user)
        {
            _user = user;
        }

        public string Name => _user.Username;
        public string AuthenticationType => "ApiKey";
        public bool IsAuthenticated => _user.IsActive;

       
        public User User => _user;
    }
}
