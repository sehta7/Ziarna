using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    class Graphic
    {
        public Bitmap Bitmap { get; set; }
        public Graphics Graphics { get; set; }

        public Bitmap DrawGrains(List<Grain> grains)
        {
            Graphics = Graphics.FromImage(Bitmap);
            foreach (var grain in grains)
            {
                Graphics.DrawRectangle(grain.PenColor, grain.Position.X, grain.Position.Y, 1, 1);
            }

            return Bitmap;
        }

        internal Bitmap DrawCircleInclusions(List<Inclusion> inclusions)
        {
            Graphics = Graphics.FromImage(Bitmap);
            foreach (var inclusion in inclusions)
            {
                Graphics.DrawEllipse(inclusion.PenColor, inclusion.Position.X, inclusion.Position.Y, inclusion.Size, inclusion.Size);
            }

            return Bitmap;
        }

        internal Bitmap DrawSquareInclusions(List<Inclusion> inclusions)
        {
            Graphics = Graphics.FromImage(Bitmap);
            foreach (var inclusion in inclusions)
            {
                Graphics.DrawRectangle(inclusion.PenColor, inclusion.Position.X, inclusion.Position.Y, inclusion.Size, inclusion.Size);
            }

            return Bitmap;
        }

        public Bitmap clear(int boardWidth, int boardHeight)
        {
            Bitmap = new Bitmap(boardWidth, boardHeight);
            return Bitmap;
        }
    }
}
