using CW11.Models;
using CW11.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW11.Controllers
{
    [ApiController]
    [Route("api/Doctor")]
    public class DoctorController : ControllerBase
    {
        IDbService _service;

        public  DoctorController(IDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Doctor> GetDoctors()
        {

            return _service.GetDoctors();
        }

        [HttpPost("add")]
        public IActionResult AddDoctor(Doctor doctor)
        {

            return Ok(_service.AddDoctor(doctor));
        }

        [HttpPut("update")]
        public IActionResult UpdateDoctor(Doctor doctor)
        {
            
            return Ok(_service.UpdateDoctor(doctor));
        }

        [HttpDelete("delete/{Id}")]
        public IActionResult DeleteDoctor(int id)
        {
            _service.DeleteDoctor(id);
            return Ok();
        }

        [HttpPost("seed")]
        public IActionResult Seed()
        {
            
            return _service.Seed(); 
        }
    }
}
