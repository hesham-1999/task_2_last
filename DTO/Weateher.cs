using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherForm.DTO
{
    public class Weateher
    {
        public string? sunSetTime { get; set; }
        public string? sunRaiseTime { get; set; }
        public Location Location { get; set; }
        public Current   current { get; set; }

    }
}
