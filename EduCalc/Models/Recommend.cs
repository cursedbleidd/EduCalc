using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCalc.Models
{
    public class Recommend
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Inc { get; set; }
        public double Coef { get; set; }
        public double Value { get; set; }

        public static implicit operator double(Recommend recommend) => recommend.Value;
    }
}
