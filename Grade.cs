using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectSGMS
{
    public class Grade
    {
        public int Id { get; set; }
        public int studentId { get; set; }

        public int subjectId { get; set; }

        public string Term { get; set; }

        public double Mark { get; set; }
    }
}
