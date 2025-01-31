﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using PassengerMicroService.Services;
using Services;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ProjRabbitMQLogs.Service;

namespace PassengerMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly PassengerService _passengerService;

        public PassengerController(PassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Passenger>> Get() =>
            _passengerService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPassenger")]
        [Authorize]
        public ActionResult<Passenger> Get(string id)
        {
            var passenger = _passengerService.Get(id);

            if (passenger == null)
                return NotFound();

            return passenger;
        }

        [HttpGet("{cpf}", Name = "GetPassengerCpf")]
        [Authorize]
        public ActionResult<Passenger> GetCpf(string cpf)
        {
            var passenger = _passengerService.VerifyCpfPassenger(cpf);

            if (passenger == null)
                return NotFound("unregistered passenger, try again");

            return passenger;
        }

        [HttpPost]
        [Route("Master, User")]
        public async Task<ActionResult<Passenger>> Create(Passenger newPassenger)
        {
            var address = await ServiceSeachViaCep.ServiceSeachCepInApiViaCep(newPassenger.Address.Cep);

            try
            {
                if (!ValidateCpf.VerifyValidCpf(newPassenger.Cpf))
                    return Conflict("Cpf invalid, try again");

                if (_passengerService.VerifyPassengerExist(newPassenger.Cpf))
                    return BadRequest("Passenger Exist, try again");

                if (address.Cep == null)
                {
                    return NotFound("Cep entered incorrect, try again");
                }

                if (address.Street != "")
                {
                    newPassenger.Address.City = address.City;
                    newPassenger.Address.Street = address.Street;
                    newPassenger.Address.State = address.State;
                    newPassenger.Address.District = address.District;
                    newPassenger.Address.Complement = address.Complement;
                    _passengerService.Create(newPassenger);
                }
                else
                    _passengerService.Create(newPassenger);

            }
            catch (NullReferenceException)
            {
                _passengerService.Create(newPassenger);
            }
            catch (HttpRequestException)
            {
                return StatusCode(443, "Service ViaCep Off");
            }

            var newPassengerJson = JsonConvert.SerializeObject(newPassenger);
            ServiceSeachApiExisting.LogInApiRabbit(new Log(newPassenger.LoginUser, null, newPassengerJson, "Post"));


            return CreatedAtRoute("GetPassenger", new { id = newPassenger.Id.ToString() }, newPassenger);

        }

        [HttpPut("{cpf}")]
        [Authorize(Roles = "Master, User")]
        public IActionResult Update(string cpf, Passenger upAassenger)
        {

            var seachPassenger = _passengerService.VerifyCpfPassenger(cpf);

            if (seachPassenger == null)
                return BadRequest("");

            _passengerService.Update(cpf, upAassenger);

            var UpdatePassengerJson = JsonConvert.SerializeObject(upAassenger);
            var OldPassengerJson = JsonConvert.SerializeObject(seachPassenger);
            ServiceSeachApiExisting.LogInApiRabbit(new Log(upAassenger.LoginUser, OldPassengerJson, UpdatePassengerJson, "Update"));

            return NoContent();
        }

        [HttpDelete("{cpf}")]
        [Authorize(Roles = "Master, User")]
        public IActionResult Delete(string cpf)
        {
            var seachPassenger = _passengerService.VerifyCpfPassenger(cpf);


            if (seachPassenger == null)
                return BadRequest();

            _passengerService.Remove(seachPassenger.Cpf);

            return NoContent();
        }
    }
}
