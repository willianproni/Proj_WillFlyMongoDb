﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FunctionMicroService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace FunctionMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        private readonly FunctionService _functionService;

        public FunctionController(FunctionService functionService)
        {
            _functionService = functionService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Function>> Get() =>
            _functionService.Get();

        [HttpGet("{id}", Name = "GetFuction")]
        [Authorize]
        public ActionResult<Function> Get(string id)
        {
            var returnSeachFunction = _functionService.Get(id);

            if (returnSeachFunction == null)
                return BadRequest("Function not Exist");

            return returnSeachFunction;
        }

        [HttpPost]
        [Authorize(Roles = "Master")]
        public async Task<ActionResult<Function>> Create(Function newFunction)
        {
            Access access;

            if (!string.IsNullOrEmpty(newFunction.Id))
            {
                var verifyExistFunction = _functionService.Get(newFunction.Id);

                if (verifyExistFunction != null)
                    return BadRequest("Function Id Exist, try New Id");

                try
                {
                    access = await ServiceSeachApiExisting.SeachAccessIdInApi(newFunction.Access.Id);

                }
                catch (HttpRequestException)
                {
                    return StatusCode(503, "Service Access unavailable, start Api Access");
                }

                newFunction.Access = access;

                _functionService.Create(newFunction);

                return CreatedAtRoute("GetFuction", new { id = newFunction.Id }, newFunction);
            }

            return BadRequest("Id cannot be null, try again");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Master")]
        public IActionResult Update(string id, Function upFunction)
        {
            var verifyExistFunction = _functionService.Get(id);

            if (verifyExistFunction == null)
                return BadRequest("Funciton not exist");

            _functionService.Update(id, upFunction);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Master")]
        public IActionResult Delete(string id)
        {
            var verifyExistFunction = _functionService.Get(id);

            if (verifyExistFunction == null)
                return BadRequest("Funciton not exist");

            _functionService.Delete(id);

            return NoContent();
        }

    }
}
