using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    class PenState
    {
        public Pen PenColor { get; set; }
        public int State { get; set; }

        public PenState(Pen color, int state)
        {
            this.PenColor = color;
            this.State = state;
        }
    }
}
