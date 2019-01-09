using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    class Inclusion
    {
        public Point Position { get; set; }
        public Pen PenColor { get; set; }
        public int Size { get; set; }

        public Inclusion()
        {
            this.PenColor = new Pen(Color.Black);
        }

        public Inclusion(Point position, int size)
        {
            this.Position = position;
            this.Size = size;
            this.PenColor = new Pen(Color.Black);
        }

        public void SetRandomlyPosition(int boardWidth, int boardHeight)
        {
            Random random = new Random();
            int x = random.Next(0, boardWidth);
            int y = random.Next(0, boardHeight);
            Point tempPosition = new Point(x, y);
            this.Position = tempPosition;
        }
    }
}
