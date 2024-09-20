﻿using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public AuthorizationService(ApplicationDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identity)
    {
        var cacheKey = $"auth:roles-{identity}";

        var cachedRoles = await _cacheService.GetAsync<UserRolesResponse>(cacheKey);

        if (cachedRoles is not null)
        {
            return cachedRoles;
        }

        var roles = await _dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identity)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();

        await _cacheService.SetAsync(cacheKey, roles);

        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identity)
    {
        var cacheKey = $"auth:permissions-{identity}";

        var cachedPermissions = await _cacheService.GetAsync<HashSet<string>>(cacheKey);

        if (cachedPermissions is not null)
        {
            return cachedPermissions;
        }

        var permissions = await _dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identity)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        var permissionsSet = permissions.Select(permission => permission.Name).ToHashSet();

        await _cacheService.SetAsync(cacheKey, permissionsSet);

        return permissionsSet;
    }
}
