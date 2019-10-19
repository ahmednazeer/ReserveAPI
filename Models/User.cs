using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity.Models
{
    public class User:IdentityUser 
    {
        public User()
        {
            
            Reservations = new HashSet<Reservation>();
        }
        [Required]
        public DateTime Date_of_Birth { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
