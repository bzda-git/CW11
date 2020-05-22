using CW11.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW11.Services
{
    public interface IDbService
    {
        public IEnumerable<Doctor> GetDoctors();
        public IActionResult AddDoctor(Doctor doctor);

        public IActionResult UpdateDoctor(Doctor doctor);

        public IActionResult DeleteDoctor(int id);

        public IActionResult Seed();
    }
}
