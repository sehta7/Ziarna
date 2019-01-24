using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    class Initialization
    {
        public static void InitializeGrainArray(Grain[,] grains, int boardWidth, int boardHeight)
        {
            for (int xPosition = 0; xPosition < boardWidth; xPosition++)
            {
                for (int yPosition = 0; yPosition < boardHeight; yPosition++)
                {
                    Point grainPosition = new Point(xPosition, yPosition);
                    Pen grainPenColor = new Pen(Color.White);

                    grains[xPosition, yPosition] = new Grain(grainPosition, grainPenColor);
                }
            }
        }
    }
}
