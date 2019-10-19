using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySQLIdentity.Models;

namespace MySQLIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private DBContext context;
        public PlacesController(DBContext context)
        {
            this.context = context;
        }
        [HttpGet("list")]
        public IActionResult ListPlaces(int locationID,int topicID)
        {

            var places= context.Places.Where(p => p.LocationID == locationID && p.TopicID == topicID).ToList();

            if (places.Count == 0)
            {
                Ok("Empty Places List");
            }
            return Ok(places);
        }

        [HttpPost("add")]
        public IActionResult AddPlaces(Place model)
        {
            if (ModelState.IsValid)
            {
                context.Places.Add(model);
                context.SaveChanges();
                return Created("", model);
            }
            else
            {
                return BadRequest("Location Name Must be Unique");
            }

        }

    }
}