using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lista11
{
    public class BasketAccesHandler : AuthorizationHandler<BasketAccesRequrement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BasketAccesRequrement requirement)
        {
            var user = context.User;
            if (user.IsInRole("Admin"))
            {
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class BasketAccesRequrement : IAuthorizationRequirement { 
        public BasketAccesRequrement()
        {
        }
    }
}
