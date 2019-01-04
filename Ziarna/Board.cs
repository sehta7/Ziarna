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
        public List<Pen> PenColors { get; set; }
        public Graphic Graphic { get; set; }

        public Board(List<Grain> grains, Grain[,] grainsInPreviousStep, Grain[,] grainsInCurrentStep)
        {
            this.Grains = grains;
            this.GrainsInPreviousStep = grainsInPreviousStep;
            this.GrainsInCurrentStep = grainsInCurrentStep;
        }

        public void InitializeGrainTables(int width, int height)
        {
            InitializeGrainTable(GrainsInPreviousStep, width, height);
            InitializeGrainTable(GrainsInCurrentStep, width, height);
        }

        private void InitializeGrainTable(Grain[,] grains, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grains[i, j] = new Grain()
                    {
                        State = 0,
                        PenColor = new Pen(color: Color.White)
                    };
                }
            }
        }

        public void GenerateGrains(int numberOfGrains, int chosenBoardWidth, int chosenBoardHeight)
        {
            Random random = new Random();
            for (int i = 0; i < numberOfGrains; i++)
            {
                int x = random.Next(0, chosenBoardWidth);
                int y = random.Next(0, chosenBoardHeight);
                var grain = new Grain()
                {

                    PenColor = new Pen(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 255))),
                    PositionX = x,
                    PositionY = y,
                    State = 1
                };
                Grains.Add(grain);
                PenColors.Add(grain.PenColor);
            }
        }

        public void InitializeFirstStep(int chosenBoardWidth, int chosenBoardHeight)
        {
            for (int i = 0; i < chosenBoardWidth; i++)
            {
                for (int j = 0; j < chosenBoardHeight; j++)
                {
                    InitializeFirstStepOfGrains(i, j, GrainsInPreviousStep);
                    InitializeFirstStepOfGrains(i, j, GrainsInCurrentStep);
                }
            }
        }

        private void InitializeFirstStepOfGrains(int i, int j, Grain[,] grains)
        {
            foreach (var grain in Grains)
            {
                if (i == grain.PositionX && j == grain.PositionY)
                {
                    grains[i, j].State = 1;
                    grains[i, j].PenColor = grain.PenColor;
                }
            }
        }

        public Bitmap DrawGrains()
        {
            return Graphic.DrawGrains(Grains);
        }
    }
}
