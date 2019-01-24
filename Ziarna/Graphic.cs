using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    public class Graphic
    {
        public Bitmap Bitmap { get; set; }
        public Graphics Graphics { get; set; }

        public Graphic(int width, int height)
        {
            Bitmap = new Bitmap(width, height);
            this.Graphics = Graphics.FromImage(Bitmap);
        }

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

        public Bitmap Clear(int boardWidth, int boardHeight)
        {
            Bitmap = new Bitmap(boardWidth, boardHeight);
            return Bitmap;
        }

        public Bitmap DrawBoundaries(List<Point> boundaries, List<Point> notBoundaries)
        {
            for (int i = 0; i < boundaries.Count; i++)
            {
                Brush blackColor = new SolidBrush(Color.Black);
                Graphics.FillRectangle(blackColor, boundaries[i].X, boundaries[i].Y, 1, 1);
            }

            for (int i = 0; i < notBoundaries.Count; i++)
            {
                Brush whiteColor = new SolidBrush(Color.White);
                Graphics.FillRectangle(whiteColor, notBoundaries[i].X, notBoundaries[i].Y, 1, 1);
            }

            return Bitmap;
        }

        public Bitmap DrawRecrustallizedGrains(List<Grain> grains, Pen penColor)
        {
            Graphics = Graphics.FromImage(Bitmap);
            foreach (var grain in grains)
            {
                Graphics.DrawRectangle(penColor, grain.Position.X, grain.Position.Y, 1, 1);
            }

            return Bitmap;
        }
    }
}
