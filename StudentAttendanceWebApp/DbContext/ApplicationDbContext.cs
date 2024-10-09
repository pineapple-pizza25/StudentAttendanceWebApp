using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceWebApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //Administrator
        public DbSet<Administrator> Administrators { get; set; }
        
        //Attendance
        public DbSet<Attendance> Attendances { get; set; }

        // Campus
        public DbSet<Campus> Campuses { get; set; }

        //Classroom
        public DbSet<Classroom> Classrooms { get; set; }

        //Course
        public DbSet<Course> Courses { get; set; }

        //Lecturer
        public DbSet<Lecturer> Lecturers { get; set; }

        //Lesson
        public DbSet<Lesson> Lessons { get; set; }

        //Student
        public DbSet<Student> Students { get; set; }

        //StudentCourse
        public DbSet<StudentCourse> StudentCourses { get; set; }

        // StudentSubject
        public DbSet<StudentSubject> StudentSubjects { get; set; }

        //Subject
        public DbSet<Subject> Subjects { get; set; }





        // note: unsure if needed to create dbcontext for ErrorViewModel as well as controller


    }
}
