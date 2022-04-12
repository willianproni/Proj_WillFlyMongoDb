﻿using System.Collections.Generic;
using AircraftMicroService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using UserMicroServices.Services;
using Services;
using System.Threading.Tasks;
using System.Net.Http;

namespace AircraftMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly AircraftService _aircraftService;

        public AircraftController(AircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        [HttpGet]
        public ActionResult<List<Aircraft>> Get() =>
            _aircraftService.Get();

        /*        [HttpGet("{id:length(24)}", Name = "GetAircraft")]
                public ActionResult<Aircraft> Get(string id)
                {
                    var aircraft = _aircraftService.Get(id);

                    if (aircraft == null)
                    {
                        return NotFound();
                    }

                    return aircraft;
                }*/

        [HttpGet("{nameAircraft}", Name = "GetAircraft")]
        public ActionResult<Aircraft> GetArcraftName(string nameAircraft)
        {
            var SeachArcraft = _aircraftService.GetNameAircraft(nameAircraft);

            if (SeachArcraft == null)
                return NotFound();

            return SeachArcraft;
        }

        [HttpPost]
        public async Task<ActionResult<Aircraft>> Create(Aircraft newAircraft)
        {
            User seahcUser;
            try
            {
                seahcUser = await ServiceSeachApiExisting.SeachUserInApiByLoginUser(newAircraft.LoginUser);

                if (seahcUser.Function.Id != "1")
                    return BadRequest("Access blocked, need manager permission");
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "Service User unavailable, start Api");
            }

            try
            {
                if (_aircraftService.VerifyAircraftExist(newAircraft.Name))
                    return Conflict("Aicraft already Registered, Try again");

                await _aircraftService.Create(newAircraft);

                return CreatedAtRoute("GetAircraft", new { id = newAircraft.Id.ToString() }, newAircraft);
            }
            catch (Exception e)
            {
                return BadRequest("Exception " + e.Message);
            }
        }

        [HttpPut("{nameAircraft}")]
        public IActionResult Update(string nameAircraft, Aircraft upAircraft)
        {
            var SeachArcraft = _aircraftService.GetNameAircraft(nameAircraft);

            if (SeachArcraft == null)
                return BadRequest("Aircraft does not exist in the database, check the data and try again");

            _aircraftService.Update(nameAircraft, upAircraft);

            return NoContent();
        }

        [HttpDelete("{nameAircraft}")]
        public IActionResult Delete(string nameAircraft)
        {
            var SeachArcraft = _aircraftService.GetNameAircraft(nameAircraft);

            if (SeachArcraft == null)
                return BadRequest("Aircraft does not exist in the database, check the data and try again");

            _aircraftService.Remove(SeachArcraft.Name);

            return NoContent();
        }
    }
}
