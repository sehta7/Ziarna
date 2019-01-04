using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ziarna
{
    public class Grain
    {
        public int State { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Pen PenColor { get; set; }          
        public int H { get; set; }
    }
}
