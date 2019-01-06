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
        public bool Alive { get; set; }
        public Point Position { get; set; }
        public Pen PenColor { get; set; }          
        public int H { get; set; }
        public List<Grain> Neighbours { get; set; }

        public Grain(Point position, Pen penColor)
        {
            this.Alive = false;
            this.Position = position;
            this.PenColor = penColor;
        }

        public Grain CheckNeigbours()
        {
            foreach (var neighbour in Neighbours)
            {
                if (neighbour.IsAlive())
                {
                    return neighbour;
                }
            }

            return new Grain(new Point(0, 0), new Pen(Color.White));
        }

        public void AddNeighbours(List<Grain> neighbours)
        {
            this.Neighbours = neighbours;
        }

        public List<Grain> AddAllNeighbours()
        {
            List<Grain> neighbours = new List<Grain>();
            neighbours.Add(new Grain(new Point(Position.X - 1, Position.Y - 1), new Pen(Color.White)));
            neighbours.Add(new Grain(new Point(Position.X, Position.Y - 1), new Pen(Color.White)));
            neighbours.Add(new Grain(new Point(Position.X + 1, Position.Y - 1), new Pen(Color.White)));
            neighbours.Add(new Grain(new Point(Position.X + 1, Position.Y), new Pen(Color.White)));
            neighbours.Add(new Grain(new Point(Position.X + 1, Position.Y + 1), new Pen(Color.White)));
            neighbours.Add(new Grain(new Point(Position.X, Position.Y + 1), new Pen(Color.White)));
            neighbours.Add(new Grain(new Point(Position.X - 1, Position.Y + 1), new Pen(Color.White)));
            neighbours.Add(new Grain(new Point(Position.X - 1, Position.Y), new Pen(Color.White)));

            return neighbours;
        }

        internal List<Grain> AddSpecifiedNeighbours()
        {
            throw new NotImplementedException();
        }

        public bool IsOnFrame(int boardWidth, int boardHeight)
        {
            bool onFrame = false;

            if (Position.X == 0 || Position.X == boardWidth - 1 ||
                Position.Y == 0 || Position.Y == boardHeight - 1)
            {
                onFrame = true;
            }

            return onFrame;
        }

        public bool IsAlive()
        {
            return Alive;
        }

        public void Reviev(Grain aliveNeighbour)
        {
            this.Alive = true;
            this.PenColor = aliveNeighbour.PenColor;
        }

        public Pen GetPenColor()
        {
            return PenColor;
        }

        public void SetGrainAlive()
        {
            this.Alive = true;
        }
    }

}
