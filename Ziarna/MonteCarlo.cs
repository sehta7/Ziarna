using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    class MonteCarlo
    {
        public int States { get; set; }
        public List<Pen> PenColors { get; set; }
        public List<Grain> Grains { get; set; }
        public Grain[,] GrainsInPreviousStep { get; set; }

        public List<Grain> InitializeBoard(int boardWidth, int boardHeight, int penColors)
        {
            Random random = new Random();

            for (int i = 0; i < penColors; i++)
            {
                Pen color = new Pen(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 255)));
                PenColors.Add(color);
            }

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    Point position = new Point(i, j);
                    int colorIndex = random.Next(PenColors.Count);
                    Grain grain = new Grain(position, PenColors[colorIndex]);
                    grain.SetGrainAlive();

                    Grains.Add(grain);
                }
            }
            return Grains;
        }

        public void GrowGrains(int boardWidth, int boardHeight, double energy)
        {
            List<Grain> temporaryGrains = Grains.ToList();

            Random rand = new Random();
            while (temporaryGrains.Count > 0)
            {
                int randomIndeks = rand.Next(temporaryGrains.Count);
                int firstEnergy = CheckEnergy(temporaryGrains[randomIndeks].Position.X, temporaryGrains[randomIndeks].Position.Y, boardWidth, boardHeight);
                int xPosition = temporaryGrains[randomIndeks].Position.X;
                int yPosition = temporaryGrains[randomIndeks].Position.Y;
                int n = rand.Next(1, 8);
                temporaryGrains.Remove(temporaryGrains[randomIndeks]);

                switch (n)
                {
                    case 1:
                        if (xPosition > 0 && yPosition > 0)
                        {
                            int secondEnergy = CheckEnergy(xPosition - 1, yPosition - 1, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition - 1, yPosition - 1].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition - 1, yPosition - 1].PenColor;
                            }
                        }
                        break;
                    case 2:
                        if (xPosition > 0 && yPosition < boardHeight - 1)
                        {
                            int secondEnergy = CheckEnergy(xPosition - 1, yPosition + 1, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition - 1, yPosition + 1].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition - 1, yPosition + 1].PenColor;
                            }
                        }
                        break;
                    case 3:
                        if (xPosition > 0)
                        {
                            int secondEnergy = CheckEnergy(xPosition - 1, yPosition, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition - 1, yPosition].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition - 1, yPosition].PenColor;
                            }
                        }
                        break;
                    case 4:
                        if (xPosition < boardWidth - 1 && yPosition > 0)
                        {
                            int secondEnergy = CheckEnergy(xPosition + 1, yPosition - 1, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition + 1, yPosition - 1].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition + 1, yPosition - 1].PenColor;
                            }
                        }
                        break;
                    case 5:
                        if (yPosition > 0)
                        {
                            int secondEnergy = CheckEnergy(xPosition, yPosition - 1, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition, yPosition - 1].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition, yPosition - 1].PenColor;
                            }
                        }
                        break;
                    case 6:
                        if (xPosition < boardWidth - 1 && yPosition < boardHeight - 1)
                        {
                            int secondEnergy = CheckEnergy(xPosition + 1, yPosition + 1, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition + 1, yPosition + 1].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition + 1, yPosition + 1].PenColor;
                            }
                        }
                        break;
                    case 7:
                        if (xPosition < boardWidth - 1)
                        {
                            int secondEnergy = CheckEnergy(xPosition + 1, yPosition, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition + 1, yPosition].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition + 1, yPosition].PenColor;
                            }
                        }
                        break;
                    case 8:
                        if (yPosition < boardHeight - 1)
                        {
                            int secondEnergy = CheckEnergy(xPosition, yPosition + 1, boardWidth, boardHeight);
                            double delta = energy * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                GrainsInPreviousStep[xPosition, yPosition].Alive = GrainsInPreviousStep[xPosition, yPosition + 1].Alive;
                                GrainsInPreviousStep[xPosition, yPosition].PenColor = GrainsInPreviousStep[xPosition, yPosition + 1].PenColor;
                            }
                        }
                        break;
                }

            }
        }

        public int CheckEnergy(int x, int y, int boardWidth, int boardHeight)
        {
            int count = 0;

            if (x < boardWidth - 2 && GrainsInPreviousStep[x + 1, y].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }
            if (x < boardWidth - 2 && y < boardHeight - 1 &&
                GrainsInPreviousStep[x + 1, y + 1].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }
            if (x < boardWidth - 2 && y > 0 &&
                GrainsInPreviousStep[x + 1, y - 1].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }
            if (y < boardHeight - 1 && x < boardWidth - 1 &&
                GrainsInPreviousStep[x, y + 1].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }
            if (y > 0 && x < boardWidth - 1 &&
                GrainsInPreviousStep[x, y - 1].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }
            if (x > 0 && GrainsInPreviousStep[x - 1, y].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }
            if (x > 0 && y < boardHeight - 1 &&
                GrainsInPreviousStep[x - 1, y + 1].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }
            if (x > 0 && y > 0 && GrainsInPreviousStep[x - 1, y - 1].Alive != GrainsInPreviousStep[x, y].Alive)
            {
                count++;
            }

            return count;
        }
    }
}
