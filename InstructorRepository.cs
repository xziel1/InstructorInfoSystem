using System;
using System.Collections.Generic;
using System.Linq;
using InstructorInfoSystem.Models;

namespace InstructorInfoSystem.Data
{
    // Simple in-memory data store for instructor records (no database).
    public static class InstructorRepository
    {
        private static List<Instructor> Instructors = new List<Instructor>
        {
            new Instructor
            {
                InstructorId = "INS-001",
                FullName = "Maria Dela Cruz",
                Age = 35,
                Address = "City of San Fernando, Pampanga",
                Designation = "Assistant Professor",
                Gender = "Female",
                ContactNumber = "09171234567",
                Majorship = "Information Technology",
                BachelorDegree = "BS Information Technology",
                Email = "maria.delacruz@pcb.edu.ph"
            },
            new Instructor
            {
                InstructorId = "INS-002",
                FullName = "Jose Santos",
                Age = 40,
                Address = "Angeles City, Pampanga",
                Designation = "Associate Professor",
                Gender = "Male",
                ContactNumber = "09181234567",
                Majorship = "Computer Science",
                BachelorDegree = "BS Computer Science",
                Email = "jose.santos@pcb.edu.ph"
            }
        };

        // READ (all)
        public static List<Instructor> GetAll()
        {
            return Instructors;
        }

        // READ (single)
        public static Instructor GetById(string id)
        {
            return Instructors.FirstOrDefault(i =>
                string.Equals(i.InstructorId, id, StringComparison.OrdinalIgnoreCase));
        }

        // CREATE
        public static void Add(Instructor instructor)
        {
            Instructors.Add(instructor);
        }

        // UPDATE
        public static void Update(Instructor instructor)
        {
            var existing = GetById(instructor.InstructorId);
            if (existing != null)
            {
                existing.FullName = instructor.FullName;
                existing.Age = instructor.Age;
                existing.Address = instructor.Address;
                existing.Designation = instructor.Designation;
                existing.Gender = instructor.Gender;
                existing.ContactNumber = instructor.ContactNumber;
                existing.Majorship = instructor.Majorship;
                existing.BachelorDegree = instructor.BachelorDegree;
                existing.Email = instructor.Email;
            }
        }

        // DELETE
        public static void Delete(string id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                Instructors.Remove(existing);
            }
        }
    }
}