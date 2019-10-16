using System;
using Microsoft.AspNetCore.Identity;

namespace dataStore
{
    public class User : IdentityUser
    {
        public string name { get; set; }
    }
}
