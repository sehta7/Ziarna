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
        public Pen pen { get; set; }
        public int st { get; set; }

        public PenState(Pen color, int state)
        {
            this.pen = color;
            this.st = state;
        }
    }
}
