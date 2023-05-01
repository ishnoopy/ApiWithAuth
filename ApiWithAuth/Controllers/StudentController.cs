using ApiWithAuth.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithAuth.Controllers
{
    public class StudentController : Controller
    {

        private readonly JwtDemoContext _context;

        public StudentController(JwtDemoContext context)
        {
            _context = context;
        }

        [HttpGet("studentslist"), Authorize(Roles = "admin")]
        public ActionResult<List<Student>> GetStudents()
        {
            var studentsInDb = _context.students.ToList();
            return studentsInDb;
        }
    }
}
