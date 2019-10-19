using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity.Models
{
    public class Place
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int LocationID { get; set; }
        [Required]
        public int TopicID { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual Location Location { get; set; }

    }
}
