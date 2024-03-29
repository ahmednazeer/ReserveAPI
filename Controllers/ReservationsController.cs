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
    public class ReservationsController : ControllerBase
    {
        DBContext context;
        public ReservationsController(DBContext context)
        {
            this.context = context;
        }
        [HttpGet("list")]
        public IActionResult ListReservations(string userId)
        {
            var reservations = context.Reservations.Where(res => res.UserID == userId).ToList();
            if (reservations.Count > 0)
            {
                return Ok(new { status = "Success", data=reservations });
            }

            return Ok(new { status = "Success", data="Empty Reservation List" });

            
        }

        [HttpPost("add")]
        public IActionResult AddReservation(Reservation model)
        {
            if (ModelState.IsValid)
            {
                context.Reservations.Add(model);
                context.SaveChanges();
                return Created("", new { status = "Fail", data = model });

            }
            else
            {
                return BadRequest(new { status = "Fail" });
            }

        }

        [HttpDelete("remove/{reservationId}")]
        public IActionResult RemoveReservation(int reservationId)
        {
            var reservation = context.Reservations.SingleOrDefault(res => res.ID == reservationId);

            if (reservation != null)
            {
                context.Reservations.Remove(reservation);
                context.SaveChanges();

                return Ok(new { status = "Success" });
            }
            return BadRequest(new { status = "Fail" });

            /*if (ModelState.IsValid)
            {
                context.Places.Add(model);
                context.SaveChanges();
                return Created("", model);
            }
            else
            {
                return BadRequest("Location Name Must be Unique");
            }*/

        }

    }
}