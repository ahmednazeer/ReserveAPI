﻿using System;
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
    public class LocationsController : ControllerBase
    {
        private DBContext context;

        public LocationsController(DBContext context)
        {
            this.context = context;
        }
        [HttpGet("list")]
        public IActionResult ListAll()
        {

            var topics = context.Locations.ToList();
            if (topics.Count == 0)
            {
                Ok("Empty Topic List");
            }
            return Ok(topics);

        }
        [HttpPost("add")]
        public IActionResult AddTopic(Location model)
        {
            if (ModelState.IsValid)
            {
                context.Locations.Add(model);
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