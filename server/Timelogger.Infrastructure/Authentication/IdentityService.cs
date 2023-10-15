using Timelogger.Core.Interfaces;

namespace Timelogger.Infrastructure.Authentication
{ 
    public class IdentityService : IIdentityService
    {
        public IdentityService()
        {
            
        }

        public int CurrentUserId { get; set; }

    }
}