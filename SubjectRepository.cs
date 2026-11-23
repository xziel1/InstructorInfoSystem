using System.Collections.Generic;
using System.Linq;
using InstructorInfoSystem.Models;

namespace InstructorInfoSystem.Data
{
    // Simple in-memory data store for this demo.
    // In a production system this would be replaced by Entity Framework Core
    // and an actual database table.
    public static class SubjectRepository
    {
        private static List<Subject> subjects = new List<Subject>
        {
            new Subject { SubjectId = 1, SubjectCode = "IT312", SubjectName = "Systems Integration and Architecture", Units = 3, InstructorName = "Prof. Dela Cruz" },
            new Subject { SubjectId = 2, SubjectCode = "IT313", SubjectName = "Capstone Project 1", Units = 3, InstructorName = "Prof. Santos" }
        };

        private static int nextId = 3;

        // READ (all)
        public static List<Subject> GetAll()
        {
            return subjects;
        }

        // READ (single) - Updated return type to Subject? to fix CS8603
        public static Subject? GetById(int id)
        {
            return subjects.FirstOrDefault(s => s.SubjectId == id);
        }

        // CREATE
        public static void Add(Subject subject)
        {
            subject.SubjectId = nextId++;
            subjects.Add(subject);
        }

        // UPDATE
        public static void Update(Subject subject)
        {
            var existing = GetById(subject.SubjectId);
            if (existing != null)
            {
                existing.SubjectCode = subject.SubjectCode;
                existing.SubjectName = subject.SubjectName;
                existing.Units = subject.Units;
                existing.InstructorName = subject.InstructorName;
            }
        }

        // DELETE
        public static void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                subjects.Remove(existing);
            }
        }
    }
}