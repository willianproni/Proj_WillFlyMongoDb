﻿using System.Collections.Generic;
using AirportDataDapper.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DataModel;

namespace AirportDataDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DapperController : ControllerBase
    {
        private readonly AirportDateServices _airportDateServices;

        public DapperController(AirportDateServices airportDateServices)
        {
            _airportDateServices = airportDateServices;
        }

        [HttpGet]
        public ActionResult<List<AirportData>> Get() =>
            _airportDateServices.GetAll();

        [HttpGet("{id}")]
        public ActionResult<AirportData> Get(string id)
        {
            var airport = _airportDateServices.Get(id);

            if (airport == null)
                return NotFound("Airport Not Exist");

            return airport;
        }

        [HttpPost]
        public ActionResult<AirportData> Create(AirportData newAirport)
        {
            _airportDateServices.Add(newAirport);
            return newAirport;
        }

        [HttpDelete("{id}")]
        public ActionResult<AirportData> Delete(string id)
        {
            var airport = _airportDateServices.Get(id);

            if (airport == null)
                return NotFound("Airport Not Exist");

            _airportDateServices.Remove(id);

            return NoContent();
        }
    }
}
