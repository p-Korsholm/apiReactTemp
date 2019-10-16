using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace dataStore
{
    public class UserInitializer
    {
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<User> _userMgr;

        public UserInitializer(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task Seed()
        {
            var user = await _userMgr.FindByNameAsync("pKorsholm");

            // Add User
            if (user == null)
            {

                user = new User()
                {
                    UserName = "pKorsholm",
                    name = "Philip",
                    Email = "pko@it-minds.dk"
                };

                var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");

                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user");
                }

            }
        }
    }
}
