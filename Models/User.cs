using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity.Models
{
    public class User:IdentityUser 
    {
        public int Age { get; set; }

        
    }
}
