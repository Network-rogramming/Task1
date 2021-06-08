using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Group> Groups { get; set; }
    }
}
