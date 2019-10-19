using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity.ViewModels
{
    public class UserModel
    {
        public string ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public DateTime Date_of_Birth { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }


    }
}
