using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiWithAuth.Entities
{
    public class Student
    {
        public int id { get; set; }
        public string student_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string course { get; set; }
    }
}
