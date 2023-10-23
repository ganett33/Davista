using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Utility.PermissionRequirement
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        // change code for
        /*
            I am also not sure what the correct way to do it would be, but as you point out the way it was written in the example it denies everything by default (that is not marked as [AllowAnonymous]).
            My problem was not with Login (as I had it marked with [AllowAnonymous]), but my problem was that in order to see the error pages (404 not found etc.) via UseStatusCodePages you had to be Logged-in.
            Your solution seams to work and I hope it is the right way to do it, and hopefully someone can comment and confirm your solution is the correct one.
         */
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
        
        /* For the search engines: IAuthorizationPolicyProvider AllowAnonymous not working when using this in Razor Pages. Since you cannot tag a handler method with AllowAnonymous you can’t use the Authorize attribute on the class and the AllowAnonymous attribute on the method.
           Changing this:
           public Task GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
           to this:
           public Task GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
           Fixes the issue and allows classes to be tagged with[AllowAnonymous]
        */

    }
}
