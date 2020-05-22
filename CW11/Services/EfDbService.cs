using CW11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW11.Services
{
    public class EfDbService : Controller, IDbService
    {
        private readonly CodeFirstContext _cfContext;

        public EfDbService(CodeFirstContext cfContext)
        {
            _cfContext = cfContext;
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            var doctors = _cfContext.Doctor.ToList();

            return doctors;

        }

        public IActionResult AddDoctor(Doctor d)
        {     
            var doctor = new Doctor();
            doctor.FirstName = d.FirstName;
            doctor.LastName = d.LastName;
            doctor.Email = d.Email;
            doctor.Prescriptions = new List<Prescription>();

            _cfContext.Doctor.Add(doctor);
            _cfContext.SaveChanges();
            return Ok("Added: " + d.ToString());
        }

        public IActionResult UpdateDoctor(Doctor d)
        {

            _cfContext.Doctor.Attach(d);
            _cfContext.Entry(d).State = EntityState.Modified;
            _cfContext.SaveChanges();
            return Ok("Updated" + d);
        }

        public IActionResult DeleteDoctor(int id)
        {   
            var doctor = _cfContext.Doctor.FirstOrDefault(e => e.IdDoctor == id);
            if (doctor == null)
            {
                return NotFound("Not found doctor");
            }
            _cfContext.Doctor.Remove(doctor);
            _cfContext.SaveChanges();

            return Ok("Deleted");
        }

        public IActionResult Seed()
        {
            try
            {
                var doctor1 = new Doctor();
                doctor1.FirstName = "Oleksandr";
                doctor1.LastName = "Malaniuk";
                doctor1.Email = "email123.gm.com";
                doctor1.Prescriptions = new List<Prescription>();
                _cfContext.Doctor.Add(doctor1);

                 Console.WriteLine(doctor1.ToString());
                var patient1 = new Patient();
                patient1.FirstName = "Oleg";
                patient1.LastName = "Little";
                patient1.BirthDate = DateTime.Now.AddYears(-30);
                 patient1.Prescriptions = new List<Prescription>();

                _cfContext.Patient.Add(patient1);

                var medicament1 = new Medicament();
                medicament1.Description = "Przeciw kataru";
                medicament1.Name = "KatarKill";
                medicament1.Type = "Sprej";
                medicament1.PrescriptionsMedicament = new List<PrescriptionMedicament>();

                _cfContext.Medicament.Add(medicament1);



                var prescription1 = new Prescription();
                prescription1.Date = DateTime.Now;
                prescription1.DueDate = DateTime.Now.AddDays(10);
                prescription1.IdPatient = patient1.IdPatient;
                prescription1.IdDoctor = doctor1.IdDoctor;

                prescription1.Doctor = doctor1;
                prescription1.Patient = patient1;
                
                prescription1.PrescriptionsMedicament = new List<PrescriptionMedicament>();

                _cfContext.Prescription.Add(prescription1);
                _cfContext.SaveChanges();
                doctor1.Prescriptions.Add(prescription1);
                patient1.Prescriptions.Add(prescription1);

                _cfContext.Update(doctor1);
                _cfContext.Update(patient1);
                _cfContext.SaveChanges();

                var prescriptionMedicament1 = new PrescriptionMedicament();
                prescriptionMedicament1.IdMedicament = _cfContext.Medicament.Where(e => e.Name == medicament1.Name
                && e.Description == medicament1.Description && e.Type == medicament1.Type).First().IdMedicament;

                prescriptionMedicament1.IdPrescription = _cfContext.Prescription.Where(e => e.IdDoctor == prescription1.IdDoctor
                && e.IdPatient == prescription1.IdPatient).First().IdPrescription;

                prescriptionMedicament1.Dose = 1; 
                prescriptionMedicament1.Details = "Codzienie 1 raz wieczorem";
                prescriptionMedicament1.Medicament = medicament1;
                
                prescriptionMedicament1.Prescription = prescription1;
                

                _cfContext.PrescriptionMedicaments.Add(prescriptionMedicament1);
                _cfContext.SaveChanges();
                prescription1.PrescriptionsMedicament.Add(prescriptionMedicament1);
                medicament1.PrescriptionsMedicament.Add(prescriptionMedicament1);

                _cfContext.Update(prescription1);
                _cfContext.Update(medicament1);
                _cfContext.SaveChanges();
            }
            catch (Exception e)
            {
                return NotFound("Something went wrong" + e);
            }
            return Ok("Ok");
}

    }
}
