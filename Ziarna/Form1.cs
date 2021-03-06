﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ziarna
{
    public partial class Form1 : Form
    {
        private static int width;
        private static int height;

        private Board board;

        public Form1()
        {
            InitializeComponent();
            width = pictureBox1.Size.Width;
            height = pictureBox1.Size.Height;
            board = new Board(pictureBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            SetSize();
            board.InitializeGrainArrays();
            GenerateGrains();
            DrawGrains();
        }

        private void SetSize()
        {
            int chosenBoardWidth = int.Parse(textBox1.Text);
            int chosenBoardHeight = int.Parse(textBox2.Text);

            if (chosenBoardWidth > 250 || chosenBoardHeight > 150)
            {
                pictureBox1.Size = new Size(250, 150);
            }
            else
            {
                pictureBox1.Size = new Size(chosenBoardWidth, chosenBoardHeight);
            }
        }

        private void GenerateGrains()
        {
            int chosenGrainsNumber = Int32.Parse(textBox3.Text);

            board.GenerateGrains(chosenGrainsNumber);
        }

        private void DrawGrains()
        {
            pictureBox1.Image = board.DrawGrains();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (board.IsNotFull())
            {
                board.GrowGrains();
                DrawGrains();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Save.Import(Width, Height, board.GrainsInCurrentStep);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            board.Grains = Save.Export(openFileDialog1);
            DrawGrains();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int numberOfInclusions = Int32.Parse(textBox5.Text);
            int sizeOfInclusions = Int32.Parse(textBox6.Text);
            board.GenerateInclusions(numberOfInclusions);
            DrawCircleInclusion(board.Inclusions);
        }

        private void DrawCircleInclusion(List<Inclusion> inclusions)
        {
            pictureBox1.Image = board.DrawCircleInclusion();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int numberOfInclusions = Int32.Parse(textBox5.Text);
            int sizeOfInclusions = Int32.Parse(textBox6.Text);
            board.GenerateInclusions(numberOfInclusions);
            DrawSquareInclusion(board.Inclusions);
        }

        private void DrawSquareInclusion(List<Inclusion> inclusions)
        {
            pictureBox1.Image = board.DrawSquareInclusion();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            board.clearBoard();
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            board.SelectTheSameGrains();
            board.RememberSelectedGrains();

            pictureBox1.Image = board.DrawGrains();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            board.SelectBoundaries();
            pictureBox1.Image = board.DrawBoundaries();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            board.SelectBoundaries();
            pictureBox1.Image = board.DrawBoundaries();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MonteCarlo monteCarlo = new MonteCarlo();
            board.Grains = monteCarlo.InitializeBoard(pictureBox1.Size.Width, pictureBox1.Size.Height, Int32.Parse(textBox7.Text));
            DrawGrains();            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MonteCarlo monteCarlo = new MonteCarlo();

            while (board.IsNotFull())
            {
                monteCarlo.GrowGrains(width, height, Int32.Parse(textBox8.Text));
                DrawGrains();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            board.SelectTheSameGrains();
            board.RememberSelectedGrains();

            pictureBox1.Image = board.DrawGrains();
        }

        private void ENERGY_Click(object sender, EventArgs e)
        {
            List<Grain> energies = FindGrainsWithHighEnergy();

            if (atBegining.Checked)
            {
                ChangeGrainEnergy(energies);
                pictureBox1.Image = board.DrawGrains();
            }

        }

        private void ChangeGrainEnergy(List<Grain> energies)
        {
            Random rand = new Random();

            for (int i = 0; i < Int32.Parse(textBox11.Text); i++)
            {
                int randIndex = rand.Next(0, energies.Count - 1);

                for (int j = 0; j < pictureBox1.Size.Width - 1; j++)
                {
                    for (int k = 0; k < pictureBox1.Size.Height - 1; k++)
                    {
                        if (j == energies[randIndex].Position.X && k == energies[randIndex].Position.Y)
                        {
                            Grain tempGrain = new Grain(new Point(j, k), new Pen(Color.FromArgb(rand.Next(0, 255), 0, 0)));
                            tempGrain.H = 0;

                            board.GrainsInPreviousStep[j, k] = tempGrain;
                        }
                    }
                }
                energies.Remove(energies[randIndex]);
            }
        }

        private List<Grain> FindGrainsWithHighEnergy()
        {
            List<Grain> energies = new List<Grain>();
            Grain tempGrain;

            for (int j = 0; j < pictureBox1.Size.Width - 1; j++)
            {
                for (int k = 0; k < pictureBox1.Size.Height - 1; k++)
                {
                    if (board.GrainsInPreviousStep[j, k].H == 8)
                    {
                        tempGrain = board.GrainsInPreviousStep[j, k];
                        energies.Add(tempGrain);
                    }
                }
            }

            return energies;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            DistributeEnergies();
        }

        private void DistributeEnergies()
        {
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {

                    if ((i < pictureBox1.Size.Width - 1 &&
                        board.GrainsInPreviousStep[i, j].PenColor != board.GrainsInPreviousStep[i + 1, j].PenColor) ||
                        (j < pictureBox1.Size.Height - 1 &&
                        board.GrainsInPreviousStep[i, j].PenColor != board.GrainsInPreviousStep[i, j + 1].PenColor))
                    {
                        board.GrainsInPreviousStep[i, j].SetHighEnergy();
                        board.GrainsInPreviousStep[i, j].SetRecrystallized();
                    }
                    else
                    {
                        board.GrainsInPreviousStep[i, j].SetLowEnergy();
                        board.GrainsInPreviousStep[i, j].SetRecrystallized();
                    }
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            MonteCarlo monteCarlo = new MonteCarlo();
            List<Grain> tempGrains = new List<Grain>();
            foreach (Grain grain in board.GrainsInPreviousStep)
            {
                tempGrains.Add(grain);
            }

            for (int i = 0; i < Int32.Parse(textBox14.Text); i++)
            {
                int randIndex = random.Next(board.Grains.Count - 1);
                int x = board.Grains[randIndex].Position.X;
                int y = board.Grains[randIndex].Position.Y;

                List<Grain> recrystallizedGrains = Recrystallization.FindRecrystallizedGrains(board, pictureBox1.Size.Width, pictureBox1.Size.Height, x, y);

                while (tempGrains.Count > 0)
                {
                    if (recrystallizedGrains.Count > 0)
                    {
                        int recrystalizedIndex = random.Next(0, recrystallizedGrains.Count);
                        int firstEnergy = monteCarlo.CheckEnergy(tempGrains[randIndex].Position.X, tempGrains[randIndex].Position.Y, pictureBox1.Size.Width, pictureBox1.Size.Height) + tempGrains[randIndex].H;
                        int secondEnergy = monteCarlo.CheckEnergy(recrystallizedGrains[recrystalizedIndex].Position.X, recrystallizedGrains[recrystalizedIndex].Position.Y, pictureBox1.Size.Width, pictureBox1.Size.Height);
                        double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        if (delta <= 0)
                        {
                            board.GrainsInPreviousStep[x, y].SetRecrystallized();
                            board.GrainsInPreviousStep[x, y].PenColor = recrystallizedGrains[x].PenColor;
                            board.GrainsInPreviousStep[x, y].H = 0;
                        }
                    }
                    
                    tempGrains.Remove(tempGrains[randIndex]);
                }

                pictureBox1.Image = board.DrawGrains();
            }
            
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Grain[,] tempGrains = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    tempGrains[i, j] = board.GrainsInPreviousStep[i, j];
                }
            }

            if (after.Checked)
            {
                MatchEnergies();
                pictureBox1.Image = board.DrawGrains();
            }

            if (before.Checked)
            {

            }
        }

        private void MatchEnergies()
        {
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    if (board.GrainsInPreviousStep[i, j].Recrystallized)
                    {
                        if (board.GrainsInPreviousStep[i, j].H == 8)
                        {
                            board.DrawRecrystallizedGrains(new Pen(Color.Green));
                        }
                        else if (board.GrainsInPreviousStep[i, j].H == 2)
                        {
                            board.DrawRecrystallizedGrains(new Pen(Color.Blue));
                        }
                    }
                    else
                    {
                        board.DrawRecrystallizedGrains(new Pen(Color.Red));
                    }

                }
            }
        }
    }
}

