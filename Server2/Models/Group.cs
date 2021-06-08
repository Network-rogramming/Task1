using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public virtual IEnumerable<Student> Students { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}
