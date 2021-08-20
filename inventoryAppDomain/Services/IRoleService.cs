using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace inventoryAppDomain.Services
{
    public interface IRoleService
    {
        IdentityRole Create(string roleName);
        List<String> GetAllRoles();

        IdentityRole FindByRoleName(string roleName);
        IdentityRole GetAppUserRole(string roleId);

        Task<string> GetRoleByUser(string userId);

        Task RemoveUserFromRole(string userId);
        Task RemoveUserRole(string roleId);

        Task ChangeUserRole(string userId, string updatedRoleName);
    }
}