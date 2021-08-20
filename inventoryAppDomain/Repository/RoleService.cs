using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class RoleService : IRoleService
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        
        
        
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            }
            private set 
            { 
                _roleManager = value; 
            }
        }
        
        public UserManager<ApplicationUser> UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set 
            { 
                _userManager = value; 
            }
        }
        
        public IdentityRole Create(string roleName)
        {
            var role = new IdentityRole(roleName);
            var result = RoleManager.Create(role);
            
            return result.Succeeded ? role : throw new Exception(result.Errors.ToString());
        }

        public List<string> GetAllRoles() => RoleManager.Roles.Where(role => role.Name != "Admin").Select(x => x.Name).ToList();


        public IdentityRole GetAppUserRole(string roleId)
        {
           var role = RoleManager.FindById(roleId);
            return role;
        }


        public async Task RemoveUserRole(string roleId)
        {
            var userRole = RoleManager.FindById(roleId);
           await _roleManager.DeleteAsync(userRole);
        }

        public IdentityRole FindByRoleName(string roleName)
        {
          var role =  RoleManager.FindByName(roleName);
            return role;
        }
        public async Task<string> GetRoleByUser(string userId)
        {
            var result = await UserManager.GetRolesAsync(userId);
            return result?.First();
        }

        public async Task RemoveUserFromRole(string userId)
        {
            await UserManager.RemoveFromRolesAsync(userId);
        }

        public async Task ChangeUserRole(string userId, string updatedRoleName)
        {
            var role = await RoleManager.FindByNameAsync(updatedRoleName);

            if (role == null)
            {
                throw new Exception("Role Doesn't Exist");
            }
            
            if (await UserManager.IsInRoleAsync(userId, updatedRoleName))
            {
                throw new Exception("User Already in Role");
            }

            var previousRole = await UserManager.GetRolesAsync(userId);
            
            var result = await UserManager.RemoveFromRoleAsync(userId, previousRole.First());
            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(userId, role.Name);
            }
        }
    }
}