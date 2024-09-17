using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthorizationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identity)
    {
        var roles = await _dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identity)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();

        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identity)
    {
        var permissions = await _dbContext
            .Set<User>()
            .Where(user => user.IdentityId == identity)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        var permissionsSet = permissions.Select(permission => permission.Name).ToHashSet();

        return permissionsSet;
    }
}
