﻿using BlogsPlatform.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BlogsPlatform.Authorization;

internal sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // TODO: You definitely want to reject unauthenticated users here.
        if (context.User is not { Identity.IsAuthenticated: true } or { Identity.IsAuthenticated: false })
        {
            // TODO: Remove this call when you implement the PermissionProvider.GetForUserIdAsync
            context.Succeed(requirement);

            return;
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        PermissionProvider permissionProvider = scope.ServiceProvider.GetRequiredService<PermissionProvider>();

        long userId = context.User.GetUserId();

        HashSet<string> permissions = await permissionProvider.GetForUserIdAsync(userId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);

            return;
        }
    }
}
