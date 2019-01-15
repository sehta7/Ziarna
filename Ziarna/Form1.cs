using System;
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
            List<Grain> grains = new List<Grain>();
            Grain[,] previousStep = new Grain[width, height];
            Grain[,] currentStep = new Grain[width, height];
            board = new Board(grains, previousStep, currentStep);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            SetSize();
            board.InitializeGrainTables(width, height);
            GenerateGrains();
            board.InitializeFirstStep(width, height);
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
            int chosenBoardWidth = int.Parse(textBox1.Text);
            int chosenBoardHeight = int.Parse(textBox2.Text);

            int chosenGrainsNumber = Int32.Parse(textBox3.Text);

            board.GenerateGrains(chosenGrainsNumber, chosenBoardWidth, chosenBoardHeight);
        }

        private void DrawGrains()
        {
            pictureBox1.Image = board.DrawGrains();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (board.IsNotFull())
            {
                board.GrowGrains(width, height);
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
            board.GenerateInclusions(Width, Height, numberOfInclusions);
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
            board.GenerateInclusions(Width, Height, numberOfInclusions);
            DrawSquareInclusion(board.Inclusions);
        }

        private void DrawSquareInclusion(List<Inclusion> inclusions)
        {
            pictureBox1.Image = board.DrawSquareInclusion();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            board.clearBoard(pictureBox1.Size.Width, pictureBox1.Size.Height);
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
            board.SelectTheSameGrains(pictureBox1.Size.Width, pictureBox1.Size.Height);
            board.RememberSelectedGrains(pictureBox1.Size.Width, pictureBox1.Size.Height);

            pictureBox1.Image = board.DrawGrains();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            board.SelectBoundaries(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = board.DrawBoundaries(pictureBox1.Size.Width, pictureBox1.Size.Height);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            board.SelectBoundaries(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = board.DrawBoundaries(pictureBox1.Size.Width, pictureBox1.Size.Height);
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
            board.SelectTheSameGrains(pictureBox1.Size.Width, pictureBox1.Size.Height);
            board.RememberSelectedGrains(pictureBox1.Size.Width, pictureBox1.Size.Height);

            pictureBox1.Image = board.DrawGrains();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (int.Parse(textBox1.Text) > 250 || int.Parse(textBox2.Text) > 150)
            {
                pictureBox1.Size = new Size(250, 150);
            }
            else
            {
                pictureBox1.Size = new Size(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            }
            grains = new List<Grain>();

            previousStep = InitiazlizeNotEmpty(selectedGrains);
            currentStep = InitiazlizeGrainTable(currentStep);
            //foreach (Grain g in selectedGrains)
            //{
            //    grains.Add(g);
            //}
            

            GenerateGrains();
            InitialStep();
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    currentStep[i, j] = previousStep[i, j];
                }
            }
            if (bitmap == null)
            {
                bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            }
            DrawGrains();
            //for (int i = 0; i < pictureBox1.Size.Width; i++)
            //{
            //    for (int j = 0; j < pictureBox1.Size.Height; j++)
            //    {
            //        if(previousStep[i,j].Pen != selectedGrains[i, j].Pen)
            //        {
            //            previousStep[i,j].Pen = 
            //        }
            //    }
            //}

            var temp = 0;
            while (temp < 100)
            {

                GrowNewGrains();
                temp++;
            }

        }

        private void GrowNewGrains()
        {
            Random rand = new Random();

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    int n = 0;
                    int m = 0;
                    int o = 0;
                    List<Pen> colorsTable = new List<Pen>();
                    List<int> colorsCount = new List<int>();

                    int probability = rand.Next(100);

                    
                    if (previousStep[i, j].State == 0 && previousStep[i,j].PenColor != state)
                    {

                        if (i < pictureBox1.Size.Width - 1 && previousStep[i + 1, j].State == 1)
                        {
                            n++;
                            m++;
                            colorsTable.Add(previousStep[i + 1, j].PenColor);
                        }
                        if (i < pictureBox1.Size.Width - 1 && j < pictureBox1.Size.Height - 1 && previousStep[i + 1, j + 1].State == 1)
                        {
                            n++;
                            o++;
                         
                            colorsTable.Add(previousStep[i + 1, j + 1].PenColor);
                        }
                        if (i < pictureBox1.Size.Width - 1 && j > 0 && previousStep[i + 1, j - 1].State == 1)
                        {
                            n++;
                            o++;
                            
                            colorsTable.Add(previousStep[i + 1, j - 1].PenColor);
                        }
                        if (j < pictureBox1.Size.Height - 1 && previousStep[i, j + 1].State == 1)
                        {
                            n++;
                            m++;
                            
                            colorsTable.Add(previousStep[i, j + 1].PenColor);
                        }
                        if (j > 0 && previousStep[i, j - 1].State == 1)
                        {
                            n++;
                            m++;
                            
                            colorsTable.Add(previousStep[i, j - 1].PenColor);
                        }
                        if (i > 0 && previousStep[i - 1, j].State == 1)
                        {
                            n++;
                            m++;
                            
                            colorsTable.Add(previousStep[i - 1, j].PenColor);
                        }
                        if (i > 0 && j < pictureBox1.Size.Height - 1 && previousStep[i - 1, j + 1].State == 1)
                        {
                            n++;
                            o++;
                            
                            colorsTable.Add(previousStep[i - 1, j + 1].PenColor);
                        }
                        if (i > 0 && j > 0 && previousStep[i - 1, j - 1].State == 1)
                        {
                            n++;
                            o++;
                            
                            colorsTable.Add(previousStep[i - 1, j - 1].PenColor);
                        }

                        for (int s = 0; s < colorsTable.Count; s++)
                        {
                            if(colorsTable[s] == state)
                            {
                                colorsTable.Remove(colorsTable[s]);
                                s = -1;
                            }
                           
                        }

                        int x = 1;
                        int index = 0, max = 0;

                        if (n > 1 && colorsTable.Count > 1)
                        {
                            for (int k = 0; k < colorsTable.Count(); k++)
                            {
                                for (int l = 0; l < colorsTable.Count(); l++)
                                {
                                    if (k == l)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (colorsTable[k].Color.A == colorsTable[l].Color.A &&
                                            colorsTable[k].Color.B == colorsTable[l].Color.B &&
                                            colorsTable[k].Color.G == colorsTable[l].Color.G &&
                                            colorsTable[k].Color.R == colorsTable[l].Color.R)
                                        {
                                            x++;
                                        }

                                    }
                                }
                                colorsCount.Add(x);
                            }

                            if (colorsCount.Count() != 0)
                            {
                                max = colorsCount.Max();
                                index = colorsCount.ToList().IndexOf(max);
                            }
                        }
                        else if (n == 1 && colorsTable.Count > 1)
                        {
                            index = 0;
                            max = 1;
                        }
                        else if (colorsTable.Count == 0)
                        {
                            continue;
                        }

                        if (n > 5 && n < 8)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].PenColor == state)
                            {
                                continue;

                            }
                            currentStep[i, j].PenColor = colorsTable[index];

                        }
                        else if (m > 3 && m < 5)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].PenColor == state)
                            {
                                continue;

                            }
                            currentStep[i, j].PenColor = colorsTable[index];


                        }
                        else if (o > 3 && o < 5)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].PenColor == state)
                            {
                                continue;

                            }
                            currentStep[i, j].PenColor = colorsTable[index];


                        }
                        else if (max > 0 && probability < Int32.Parse(textBox4.Text) && n > 0)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].PenColor == state)
                            {
                                continue;

                            }
                            currentStep[i, j].PenColor = colorsTable[index];


                        }
                        x = 1;
                        index = 0;
                        max = 0;
                    }
                }
            }

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    previousStep[i, j].State = currentStep[i, j].State;
                    previousStep[i, j].PenColor = currentStep[i, j].PenColor;
                }
            }


            Graphics gr = Graphics.FromImage(bitmap);
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    if (previousStep[i, j].State == 1)
                    {
                        gr.DrawRectangle(currentStep[i, j].PenColor, i, j, 1, 1);
                    }

                }
            }

            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (int.Parse(textBox1.Text) > 250 || int.Parse(textBox2.Text) > 150)
            {
                pictureBox1.Size = new Size(250, 150);
            }
            else
            {
                pictureBox1.Size = new Size(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            }
            grains = new List<Grain>();

            previousStep = InitiazlizeNotEmpty(selectedGrains);
            currentStep = InitiazlizeGrainTable(currentStep);

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    currentStep[i, j] = previousStep[i, j];
                }
            }

            Monte(currentStep);
            InitialStep();

            if (bitmap == null)
            {
                bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            }

            var temp = 0;
            while (temp < 5)
            {
                DrawMonte();
                temp++;
            }
        }

        public void Monte(Grain[,] currentStep)
        {
            Random random = new Random();
            pens = new List<Pen>();
            for (int i = 0; i < Int32.Parse(textBox7.Text); i++)
            {
                Pen color = new Pen(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 255)));
                pens.Add(color);
                colors.Add(color);
            }

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    Grain grain = new Grain();
                    if (currentStep[i,j].Alive != 1)
                    {
                        grain.PositionX = i;
                        grain.PositionY = j;
                        int x = random.Next(pens.Count);
                        grain.PenColor = pens[x];
                        grain.Alive = x + 1;
                        grains.Add(grain);
                        //colors.Add(grain.Pen);
                    }

                }
            }

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    foreach (var grain in grains)
                    {
                        if (i == grain.PositionX && j == grain.PositionY)
                        {
                            previousStep[i, j].State = grain.State;
                            previousStep[i, j].PenColor = grain.PenColor;
                        }

                    }
                }

            }

            DrawGrains();
        }

        public void DrawMonte()
        {
            List<Grain> temp = new List<Grain>();
            List<Grain> temp2 = new List<Grain>();
            foreach (Grain g in grains)
            {
                temp2.Add(g);
            }
            foreach (Grain grain in temp2)
            {
                if (grain.Alive != 1)
                {
                    temp.Add(grain);
                }
            }
            Random rand = new Random();

            while (temp.Count > 0)
            {
                int a = rand.Next(temp.Count);
                int firstEnergy = CheckEnergy(temp[a].PositionX, temp[a].PositionY);
                int i = temp[a].PositionX;
                int j = temp[a].PositionY;
                int n = rand.Next(1, 8);
                temp.Remove(temp[a]);

                switch (n)
                {
                    case 1:
                        if (i > 0 && j > 0)
                        {
                            int secondEnergy = CheckEnergy(i - 1, j - 1);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i - 1, j - 1].State;
                                previousStep[i, j].PenColor = previousStep[i - 1, j - 1].PenColor;
                            }
                        }
                        break;
                    case 2:
                        if (i > 0 && j < pictureBox1.Size.Height - 1)
                        {
                            int secondEnergy = CheckEnergy(i - 1, j + 1);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i - 1, j + 1].State;
                                previousStep[i, j].PenColor = previousStep[i - 1, j + 1].PenColor;
                            }
                        }
                        break;
                    case 3:
                        if (i > 0)
                        {
                            int secondEnergy = CheckEnergy(i - 1, j);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i - 1, j].State;
                                previousStep[i, j].PenColor = previousStep[i - 1, j].PenColor;
                            }
                        }
                        break;
                    case 4:
                        if (i < pictureBox1.Size.Width - 1 && j > 0)
                        {
                            int secondEnergy = CheckEnergy(i + 1, j - 1);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i + 1, j - 1].State;
                                previousStep[i, j].PenColor = previousStep[i + 1, j - 1].PenColor;
                            }
                        }
                        break;
                    case 5:
                        if (j > 0)
                        {
                            int secondEnergy = CheckEnergy(i, j - 1);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i, j - 1].State;
                                previousStep[i, j].PenColor = previousStep[i, j - 1].PenColor;
                            }
                        }
                        break;
                    case 6:
                        if (i < pictureBox1.Size.Width - 1 && j < pictureBox1.Size.Height - 1)
                        {
                            int secondEnergy = CheckEnergy(i + 1, j + 1);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i + 1, j + 1].State;
                                previousStep[i, j].PenColor = previousStep[i + 1, j + 1].PenColor;
                            }
                        }
                        break;
                    case 7:
                        if (i < pictureBox1.Size.Width - 1)
                        {
                            int secondEnergy = CheckEnergy(i + 1, j);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i + 1, j].State;
                                previousStep[i, j].PenColor = previousStep[i + 1, j].PenColor;
                            }
                        }
                        break;
                    case 8:
                        if (j < pictureBox1.Size.Height - 1)
                        {
                            int secondEnergy = CheckEnergy(i, j + 1);
                            double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                            if (delta <= 0)
                            {
                                previousStep[i, j].State = previousStep[i, j + 1].State;
                                previousStep[i, j].PenColor = previousStep[i, j + 1].PenColor;
                            }
                        }
                        break;
                }

            }

            Graphics gr = Graphics.FromImage(bitmap);
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    gr.DrawRectangle(previousStep[i, j].PenColor, i, j, 1, 1);

                }
            }

            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
        }

        private void ENERGY_Click(object sender, EventArgs e)
        {

            Random rand = new Random();
            List<Grain> energies = new List<Grain>();
            Graphics g = Graphics.FromImage(bitmap);
            Grain[,] energy = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];
            Grain gr;

            for (int j = 0; j < pictureBox1.Size.Width - 1; j++)
            {
                for (int k = 0; k < pictureBox1.Size.Height - 1; k++)
                {
                    if (previousStep[j, k].H == 8)
                    {
                        gr = previousStep[j, k];
                        energies.Add(gr);
                        //previousStep[j, k] = gr;
                    }
                }
            }


            if (checkBox1.Checked)
            {

                for (int i = 0; i < Int32.Parse(textBox9.Text)/ Int32.Parse(textBox12.Text); i++)
                {
                    for (int j = 0; j < Int32.Parse(textBox12.Text); j++)
                    {
                        int cor = rand.Next(energies.Count());
                        Brush brush = new SolidBrush(Color.Red);
                        g.FillEllipse(brush, energies[cor].PositionX, energies[cor].PositionY, 2, 2);

                        //for (int k = 0; k < Int32.Parse(textBox12.Text); k++)
                        //{
                        //    for (int m = 0; m < pictureBox1.Width; m++)
                        //        for (int n = 0; n < pictureBox1.Height; n++)
                        //        {
                        //            energy[m, n] = new Grain()
                        //            {
                        //                State = 1,
                        //                Pen = new Pen(color: Color.Black),
                        //                PositionX = energies[k].PositionX,
                        //                PositionY = energies[k].PositionY
                        //            };
                        //        }
                        //}
                        pictureBox1.Image = bitmap;
                        pictureBox1.Refresh();
                    }
                    //previousStep = InitiazlizeGrainTable(energy);
                    
                }  

            }

            if (checkBox2.Checked)
            {
                
            }

            if (checkBox3.Checked)
            {
                for (int i = 0; i < Int32.Parse(textBox11.Text); i++)
                {
                    int x = rand.Next(0, energies.Count - 1);

                    for (int j = 0; j < pictureBox1.Size.Width - 1; j++)
                    {
                        for (int k = 0; k < pictureBox1.Size.Height - 1; k++)
                        {


                            if (j == energies[x].PositionX && k == energies[x].PositionY)
                            {
                                gr = new Grain();
                                gr.PositionX = j;
                                gr.PositionY = k;
                                gr.Alive = -1;
                                gr.H = 0;
                                gr.PenColor = new Pen(Color.FromArgb(rand.Next(0, 255), 0, 0));
                                //energies.Add(gr);
                                previousStep[j, k] = gr;
                            }
                        }
                    }

                    energies.Remove(energies[x]);

                }

                for (int k = 0; k < pictureBox1.Size.Width; k++)
                {
                    for (int l = 0; l < pictureBox1.Size.Height; l++)
                    {
                        //Color col = previousStep[k, l].Pen.Color;
                        g.DrawRectangle(previousStep[k, l].PenColor, k, l, 10, 10);
                        //g.FillRectangle(new SolidBrush(col), k, l, 10, 10);

                    }
                }

                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();

                //for (int i = 0; i < Int32.Parse(textBox11.Text); i++)
                //{

                //    for (int j = 0; j < pictureBox1.Size.Width - 1; j++)
                //    {
                //        for (int k = 0; k < pictureBox1.Size.Height - 1; k++)
                //        {
                //            int x = rand.Next(0, pictureBox1.Size.Width - 1);
                //            int y = rand.Next(0, pictureBox1.Size.Height - 1);

                //            if (j == x && k == y)
                //            {
                //                gr = new Grain();
                //                gr.PositionX = j;
                //                gr.PositionY = k;
                //                gr.State = -1;
                //                gr.H = 0;
                //                gr.Pen = new Pen(Color.FromArgb(rand.Next(0, 255), 0, 0));
                //                //energies.Add(gr);
                //                previousStep[j, k] = gr;
                //            }
                //        }
                //    }

                //}

                //for (int k = 0; k < pictureBox1.Size.Width; k++)
                //{
                //    for (int l = 0; l < pictureBox1.Size.Height; l++)
                //    {
                //        //Color col = previousStep[k, l].Pen.Color;
                //        g.DrawRectangle(previousStep[k, l].Pen, k, l, 10, 10);
                //        //g.FillRectangle(new SolidBrush(col), k, l, 10, 10);

                //    }
                //}

                //pictureBox1.Image = bitmap;
                //pictureBox1.Refresh();

            }

        }

        public int Energy(int i, int j)
        {
            int n = 0;

            if (i < pictureBox1.Size.Width - 2 && previousStep[i + 1, j].State != previousStep[i, j].State)
            {
                n++;
            }
            if (i < pictureBox1.Size.Width - 2 && j < pictureBox1.Size.Height - 1 && previousStep[i + 1, j + 1].State != previousStep[i, j].State)
            {
                n++;
            }
            if (i < pictureBox1.Size.Width - 2 && j > 0 && previousStep[i + 1, j - 1].State != previousStep[i, j].State)
            {
                n++;
            }
            if (j < pictureBox1.Size.Height - 1 && i < pictureBox1.Size.Width - 1 && previousStep[i, j + 1].State != previousStep[i, j].State)
            {
                n++;
            }
            if (j > 0 && i < pictureBox1.Size.Width - 1 && previousStep[i, j - 1].State != previousStep[i, j].State)
            {
                n++;
            }
            if (i > 0 && previousStep[i - 1, j].State != previousStep[i, j].State)
            {
                n++;
            }
            if (i > 0 && j < pictureBox1.Size.Height - 1 && previousStep[i - 1, j + 1].State != previousStep[i, j].State)
            {
                n++;
            }
            if (i > 0 && j > 0 && previousStep[i - 1, j - 1].State != previousStep[i, j].State)
            {
                n++;
            }

            return n;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < pictureBox1.Size.Width; j++)
            {
                for (int k = 0; k < pictureBox1.Size.Height; k++)
                {

                    if ((j < pictureBox1.Size.Width - 1 && previousStep[j, k].PenColor != previousStep[j + 1, k].PenColor) || (k < pictureBox1.Size.Height - 1 && previousStep[j, k].PenColor != previousStep[j, k + 1].PenColor))
                    {
                        previousStep[j, k].H = 8;
                        previousStep[j, k].PositionX = j;
                        previousStep[j, k].PositionY = k;
                    }
                    else
                    {
                        previousStep[j, k].H = 2;
                        previousStep[j, k].PositionX = j;
                        previousStep[j, k].PositionY = k;
                    }
                }
            }

            
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bitmap);

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    g.DrawRectangle(previousStep[i, j].PenColor, i, j, 1, 1);
                }
            }

            Random rand = new Random();
           
            Grain[,] energy = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];

                for (int z = 0; z < Int32.Parse(textBox14.Text); z++)
                {

                
                List<Grain> temp = new List<Grain>();
                foreach (Grain gra in previousStep)
                {
                    //if (gra.PositionX != 0 && gra.PositionY != 0)
                    //{
                    temp.Add(gra);
                    //}
                }

                Random random = new Random();
                while (temp.Count > 0)
                {
                    int a = random.Next(temp.Count - 1);
                    int i = temp[a].PositionX;
                    int j = temp[a].PositionY;
                    List<Grain> rec = new List<Grain>();


                    if (i > 0 && j > 0 && previousStep[i - 1, j - 1].State == -1)
                    {
                        rec.Add(previousStep[i - 1, j - 1]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i - 1, j - 1);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i - 1, j - 1].Pen;
                        //}
                    }

                    if (i > 0 && j < pictureBox1.Size.Height - 1 && previousStep[i - 1, j + 1].State == -1)
                    {
                        rec.Add(previousStep[i - 1, j + 1]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i - 1, j + 1);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i - 1, j + 1].Pen;
                        //}
                    }

                    if (i > 0 && previousStep[i - 1, j].State == -1)
                    {
                        rec.Add(previousStep[i - 1, j]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i - 1, j);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i - 1, j].Pen;
                        //}
                    }

                    if (i < pictureBox1.Size.Width - 1 && j > 0 && previousStep[i + 1, j - 1].State == -1)
                    {
                        rec.Add(previousStep[i + 1, j - 1]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i + 1, j - 1);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i + 1, j - 1].Pen;
                        //}
                    }

                    if (j > 0 && previousStep[i, j - 1].State == -1)
                    {
                        rec.Add(previousStep[i, j - 1]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i, j - 1);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i, j - 1].Pen;
                        //}
                    }

                    if (i < pictureBox1.Size.Width - 1 && j < pictureBox1.Size.Height - 1 && previousStep[i + 1, j + 1].State == -1)
                    {
                        rec.Add(previousStep[i + 1, j + 1]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i + 1, j + 1);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i + 1, j + 1].Pen;
                        //}
                    }

                    if (i < pictureBox1.Size.Width - 1 && previousStep[i + 1, j].State == -1)
                    {
                        rec.Add(previousStep[i + 1, j]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i + 1, j);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i + 1, j].Pen;
                        //}
                    }

                    if (j < pictureBox1.Size.Height - 1 && previousStep[i, j + 1].State == -1)
                    {
                        rec.Add(previousStep[i, j + 1]);
                        //int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        //int secondEnergy = CheckEnergy(i, j + 1);
                        //double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        //if (delta <= 0)
                        //{
                        //    previousStep[i, j].State = -1;
                        //    previousStep[i, j].Pen = previousStep[i, j + 1].Pen;
                        //}
                    }

                    if (rec.Count > 0)
                    {
                        int x = rand.Next(0, rec.Count);
                        int firstEnergy = Energy(temp[a].PositionX, temp[a].PositionY) + temp[a].H;
                        int secondEnergy = CheckEnergy(rec[x].PositionX, rec[x].PositionY);
                        double delta = double.Parse(textBox8.Text) * secondEnergy - firstEnergy;
                        if (delta <= 0)
                        {
                            previousStep[i, j].State = -1;
                            previousStep[i, j].PenColor = rec[x].PenColor;
                            previousStep[i, j].H = 0;
                        }
                    }
                    

                    temp.Remove(temp[a]);

                }

                for (int k = 0; k < pictureBox1.Size.Width; k++)
                {
                    for (int l = 0; l < pictureBox1.Size.Height; l++)
                    {
                        if (previousStep[k, l].State == -1)
                        {
                            g.DrawRectangle(previousStep[k, l].PenColor, k, l, 1, 1);
                        }
                        

                    }
                }

                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();


            }
            
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(bitmap);
            Grain[,] temp = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    temp[i, j] = previousStep[i, j];
                }
            }

            if (checkBox4.Checked)
            {
                for (int i = 0; i < pictureBox1.Size.Width; i++)
                {
                    for (int j = 0; j < pictureBox1.Size.Height; j++)
                    {
                        if (previousStep[i, j].State != -1)
                        {
                            if (previousStep[i, j].H == 8)
                            {
                                gr.DrawRectangle(new Pen(Color.Green), i, j, 1, 1);
                            }
                            else if (previousStep[i, j].H == 2)
                            {
                                gr.DrawRectangle(new Pen(Color.Blue), i, j, 1, 1);
                            }
                        }
                        else
                        {
                            gr.DrawRectangle(new Pen(Color.Red), i, j, 1, 1);
                        }

                    }
                }

                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
            }

            if (checkBox5.Checked)
            {
                for (int i = 0; i < pictureBox1.Size.Width; i++)
                {
                    for (int j = 0; j < pictureBox1.Size.Height; j++)
                    {
                        if (previousStep[i, j].State != -1)
                        {
                            if (previousStep[i, j].H == 8)
                            {
                                gr.DrawRectangle(new Pen(Color.Green), i, j, 1, 1);
                            }
                            else if (previousStep[i, j].H == 2)
                            {
                                gr.DrawRectangle(new Pen(Color.Blue), i, j, 1, 1);
                            }
                        }
                        else
                        {
                            gr.DrawRectangle(new Pen(Color.Red), i, j, 1, 1);
                        }

                    }
                }

                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
            }

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    gr.DrawRectangle(previousStep[i, j].PenColor, i, j, 1, 1);
                }
            }

        }
    }
}

