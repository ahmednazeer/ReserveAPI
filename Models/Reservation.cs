using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity.Models
{
    public class Reservation
    {
        public int ID { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        //public DateTime Time { get; set; }
        public string Description { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public int PlaceID { get; set; }
        //[Required]
        //public int LocationID { get; set; }

        //public virtual Location Location { get; set; }
        public virtual Place Place { get; set; }
        public virtual User User { get; set; }
    }
}
