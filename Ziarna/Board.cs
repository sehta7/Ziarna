using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    class Board
    {
        public List<Grain> Grains { get; set; }
        public Grain[,] GrainsInPreviousStep { get; set; }
        public Grain[,] GrainsInCurrentStep { get; set; }
        public Grain[,] SelectedGrains { get; set; }
        public List<Inclusion> Inclusions { get; set; }
        public List<Pen> PenColors { get; set; }
        public Graphic Graphic { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Board(List<Grain> grains, Grain[,] grainsInPreviousStep, Grain[,] grainsInCurrentStep)
        {
            this.Grains = grains;
            this.GrainsInPreviousStep = new Grain[Width, Height];
            this.GrainsInCurrentStep = grainsInCurrentStep;
        }

        public void InitializeGrainTables(int boardWidth, int boardHeight)
        {
            InitializeGrainTable(GrainsInPreviousStep, boardWidth, boardHeight);
            InitializeGrainTable(GrainsInCurrentStep, boardWidth, boardHeight);
        }

        private void InitializeGrainTable(Grain[,] grains, int boardWidth, int boardHeight)
        {
            for (int xPosition = 0; xPosition < boardWidth - 1; xPosition++)
            {
                for (int yPosition = 0; yPosition < boardHeight - 1; yPosition++)
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
    }
}
