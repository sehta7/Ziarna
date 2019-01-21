using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Board(List<Grain> grains, Grain[,] grainsInPreviousStep, Grain[,] grainsInCurrentStep)
        {
            this.Grains = grains;
            this.GrainsInPreviousStep = grainsInPreviousStep;
            this.GrainsInCurrentStep = grainsInCurrentStep;
        }

        public void InitializeGrainTables(int boardWidth, int boardHeight)
        {
            InitializeGrainTable(GrainsInPreviousStep, boardWidth, boardHeight);
            InitializeGrainTable(GrainsInCurrentStep, boardWidth, boardHeight);
        }

        private void InitializeGrainTable(Grain[,] grains, int boardWidth, int boardHeight)
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

        private void AddGrainNeighbours(Grain grain, int boardWidth, int boardHeight)
        {
            List<Grain> neighbours = new List<Grain>();

            if (grain.IsOnFrame(boardWidth, boardHeight))
            {
                neighbours = grain.AddSpecifiedNeighbours();
            }
            else
            {
                neighbours = grain.AddAllNeighbours();
            }
        }

        public void GenerateGrains(int numberOfGrains, int chosenBoardWidth, int chosenBoardHeight)
        {
            Random random = new Random();
            for (int i = 0; i < numberOfGrains; i++)
            {
                int xPosition = random.Next(0, chosenBoardWidth);
                int yPosition = random.Next(0, chosenBoardHeight);

                Point grainPosition = new Point(xPosition, yPosition);
                Pen grainPenColor = new Pen(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 255)));

                Grain grain = new Grain(grainPosition, grainPenColor);
                grain.SetGrainAlive();
                Grains.Add(grain);
                PenColors.Add(grainPenColor);
            }
        }

        public void InitializeFirstStep(int chosenBoardWidth, int chosenBoardHeight)
        {
            for (int i = 0; i < chosenBoardWidth - 1; i++)
            {
                for (int j = 0; j < chosenBoardHeight - 1; j++)
                {
                    InitializeFirstStepOfGrains(i, j, GrainsInPreviousStep);
                    InitializeFirstStepOfGrains(i, j, GrainsInCurrentStep);
                }
            }
        }

        internal Bitmap DrawCircleInclusion()
        {
            return Graphic.DrawCircleInclusions(Inclusions);
        }

        private void InitializeFirstStepOfGrains(int xGrainPosition, int yGrainPosition, Grain[,] grains)
        {
            foreach (var grain in Grains)
            {
                if (xGrainPosition == grain.Position.X && yGrainPosition == grain.Position.Y)
                {
                    grains[xGrainPosition, yGrainPosition].Alive = true;
                    grains[xGrainPosition, yGrainPosition].PenColor = grain.PenColor;
                }
            }
        }

        public Bitmap DrawGrains()
        {
            return Graphic.DrawGrains(Grains);
        }

        public Bitmap DrawRecrystallizedGrains(Pen penColor)
        {
            return Graphic.DrawRecrustallizedGrains(Grains, penColor);
        }

        public void GrowGrains(int boardWidth, int boardHeight)
        {
            for (int i = 0; i < boardWidth - 1; i++)
            {
                for (int j = 0; j < boardHeight - 1; j++)
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

        internal Bitmap DrawSquareInclusion()
        {
            return Graphic.DrawSquareInclusions(Inclusions);
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

        public void GenerateInclusions(int boardWidth, int boardHeight, int numberOfInclusions)
        {
            for (int i = 0; i < numberOfInclusions; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    for (int k = 0; k < boardHeight; k++)
                    {
                        Inclusion inclusion = new Inclusion();
                        inclusion.SetRandomlyPosition(boardWidth, boardHeight);
                        Inclusions.Add(inclusion);
                    }
                }
            }
        }

        public void clearBoard(int boardWidth, int boardHeight)
        {
            Graphic.Clear(boardWidth, boardHeight);
            InitializeGrainTables(boardWidth, boardHeight);
        }

        public void SelectTheSameGrains(int boardWidth, int boardHeight)
        {
            Pen choosenColor = ChooseRandomColor();

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
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

        public void RememberSelectedGrains(int boardWidth, int boardHeight)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
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

        public void SelectBoundaries(int boardWidth, int boardHeight)
        {
            Boundaries = new List<Point>();
            notBoundaries = new List<Point>();

            SelectNotAll(false, boardWidth, boardHeight);
        }

        private void SelectNotAll(bool notAll, int boardWidth, int boardHeight)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (notAll)
                    {
                        if (GrainsInPreviousStep[i, j].PenColor.Color == Color.White)
                        {
                            notBoundaries.Add(new Point(i, j));
                            continue;
                        }
                    }

                    if ((i < boardWidth - 1 &&
                        GrainsInPreviousStep[i, j].PenColor != GrainsInPreviousStep[i + 1, j].PenColor) ||
                        (j < boardHeight - 1 &&
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

        public void SelectBoundary(int boardWidth, int boardHeight)
        {
            Boundaries = new List<Point>();
            notBoundaries = new List<Point>();

            SelectNotAll(true, boardWidth, boardHeight);
        }

        public Bitmap DrawBoundaries(int boardWidth, int boardHeight)
        {
            return Graphic.DrawBoundaries(Boundaries, notBoundaries);
        }
    }
}
