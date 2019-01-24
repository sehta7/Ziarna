using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ziarna
{
    public class Board
    {
        public List<Grain> Grains { get; set; }
        public Grain[,] GrainsInPreviousStep { get; set; }
        public Grain[,] GrainsInCurrentStep { get; set; }
        public Grain[,] SelectedGrains { get; set; }
        public List<Inclusion> Inclusions { get; set; }
        public List<Pen> PenColors { get; set; }
        public List<Point> Boundaries { get; set; }
        public List<Point> notBoundaries { get; set; }
        public Graphic Graphic { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Board(PictureBox pictureBox)
        {
            this.Width = pictureBox.Size.Width;
            this.Height = pictureBox.Size.Height;
            this.Grains = new List<Grain>();
            this.GrainsInPreviousStep = new Grain[Width, Height];
            this.GrainsInCurrentStep = new Grain[Width, Height];
            this.Graphic = new Graphic(Width, Height);
        }

        public void InitializeGrainArrays()
        {
            Initialization.InitializeGrainArray(GrainsInPreviousStep, Width, Height);
            Initialization.InitializeGrainArray(GrainsInCurrentStep, Width, Height);
        }

        public void GenerateGrains(int numberOfGrains)
        {
            Grains = new List<Grain>();
            PenColors = new List<Pen>();
            Random random = new Random();
            for (int i = 0; i < numberOfGrains; i++)
            {
                int xPosition = random.Next(0, Width);
                int yPosition = random.Next(0, Height);

                Point grainPosition = new Point(xPosition, yPosition);
                Pen grainPenColor = new Pen(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 255)));

                Grain grain = new Grain(grainPosition, grainPenColor);
                grain.SetGrainAlive();
                Grains.Add(grain);
                PenColors.Add(grainPenColor);

                SaveGrainInArrays(xPosition, yPosition, grain);
            }
        }

        private void SaveGrainInArrays(int xPosition, int yPosition, Grain grain)
        {
            GrainsInPreviousStep[xPosition, yPosition] = grain;
            GrainsInCurrentStep[xPosition, yPosition] = grain;
        }

        public Bitmap DrawGrains()
        {
            return Graphic.DrawGrains(Grains);
        }

        public Bitmap DrawSquareInclusion()
        {
            return Graphic.DrawSquareInclusions(Inclusions);
        }

        public Bitmap DrawCircleInclusion()
        {
            return Graphic.DrawCircleInclusions(Inclusions);
        }

        public Bitmap DrawRecrystallizedGrains(Pen penColor)
        {
            return Graphic.DrawRecrustallizedGrains(Grains, penColor);
        }

        public void GrowGrains()
        {
            for (int i = 0; i < Width - 1; i++)
            {
                for (int j = 0; j < Height - 1; j++)
                {
                    Grain currentGrain = GrainsInPreviousStep[i, j];
                    Grain grainNeighbour = currentGrain.CheckNeigbours();
                    if (grainNeighbour.IsAlive())
                    {
                        GrainsInCurrentStep[i, j].Reviev(currentGrain);
                        Grains.Add(currentGrain);
                    }
                }
            }
        }

        public bool IsNotFull()
        {
            bool isNotFull = true;

            if (Grains.Count == Width * Height)
            {
                isNotFull = false;
            }

            return isNotFull;
        }

        public void GenerateInclusions(int numberOfInclusions)
        {
            for (int i = 0; i < numberOfInclusions; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    for (int k = 0; k < Height; k++)
                    {
                        Inclusion inclusion = new Inclusion();
                        inclusion.SetRandomlyPosition(Width, Height);
                        Inclusions.Add(inclusion);
                    }
                }
            }
        }

        public void clearBoard()
        {
            Graphic.Clear(Width, Height);
            InitializeGrainArrays();
        }

        public void SelectTheSameGrains()
        {
            Pen choosenColor = ChooseRandomColor();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Pen currentColor = GrainsInCurrentStep[i, j].PenColor;
                    if (GrainsInCurrentStep[i, j].HasSameColor(choosenColor, currentColor))
                    {
                        RememberGrain(choosenColor, i, j);
                    }
                    else
                    {
                        ClearGrain(i, j);
                    }
                }
            }

        }

        public void RememberSelectedGrains()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    SelectedGrains[i, j] = GrainsInCurrentStep[i, j];
                }
            }
        }

        private void ClearGrain(int x, int y)
        {
            GrainsInCurrentStep[x, y].SetGrainAlive();
            GrainsInCurrentStep[x, y].PenColor = new Pen(Color.White);
            GrainsInPreviousStep[x, y].SetGrainAlive();
            GrainsInPreviousStep[x, y].PenColor = new Pen(Color.White);
        }

        private void RememberGrain(Pen choosenColor, int x, int y)
        {
            GrainsInCurrentStep[x, y].SetGrainAlive();
            GrainsInCurrentStep[x, y].PenColor = choosenColor;
            GrainsInPreviousStep[x, y].SetGrainAlive();
            GrainsInPreviousStep[x, y].PenColor = choosenColor;
        }

        private Pen ChooseRandomColor()
        {
            Random rand = new Random();
            int x = rand.Next(PenColors.Count);
            return new Pen(PenColors[x].Color);
        }

        public void SelectBoundaries()
        {
            Boundaries = new List<Point>();
            notBoundaries = new List<Point>();

            SelectNotAll(false);
        }

        private void SelectNotAll(bool notAll)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (notAll)
                    {
                        if (GrainsInPreviousStep[i, j].PenColor.Color == Color.White)
                        {
                            notBoundaries.Add(new Point(i, j));
                            continue;
                        }
                    }

                    if ((i < Width - 1 &&
                        GrainsInPreviousStep[i, j].PenColor != GrainsInPreviousStep[i + 1, j].PenColor) ||
                        (j < Height - 1 &&
                        GrainsInPreviousStep[i, j].PenColor != GrainsInPreviousStep[i, j + 1].PenColor))
                    {
                        Boundaries.Add(new Point(i, j));
                    }
                    else
                    {
                        notBoundaries.Add(new Point(i, j));
                    }
                }
            }
        }

        public void SelectBoundary()
        {
            Boundaries = new List<Point>();
            notBoundaries = new List<Point>();

            SelectNotAll(true);
        }

        public Bitmap DrawBoundaries()
        {
            return Graphic.DrawBoundaries(Boundaries, notBoundaries);
        }
    }
}
