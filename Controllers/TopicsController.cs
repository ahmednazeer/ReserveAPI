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
    public class TopicsController : ControllerBase
    {
        private DBContext context;
        public TopicsController(DBContext context)
        {
            this.context = context;
        }
        [HttpGet("list")]
        public IActionResult ListAll()
        {

            var topics = context.Topics.ToList();
            if (topics.Count == 0)
            {
                Ok("Empty Topic List");
            }
            return Ok(topics);
            
        }
        [HttpPost("add")]
        public IActionResult AddTopic(Topic model)
        {
            if (ModelState.IsValid)
            {
                context.Topics.Add(model);
                context.SaveChanges();
                return Created("", model);
            }
            else
            {
                return BadRequest("Topic Name Must be Unique");
            }

        }

    }
}