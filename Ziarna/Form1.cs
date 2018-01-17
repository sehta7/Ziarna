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
        List<Grain> grains;
        Grain[,] previousStep;
        Grain[,] currentStep;
        Grain[,] originalPicture;
        Bitmap bitmap;
        List<Pen> colors = new List<Pen>();
        Pen state;
        Grain[,] selectedGrains;
        List<Pen> pens;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            previousStep = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];
            currentStep = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];
            originalPicture = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];
            selectedGrains = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];
        }

        private void button1_Click(object sender, EventArgs e)
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
            if (state == null)
            {
                previousStep = InitiazlizeGrainTable(previousStep);
                currentStep = InitiazlizeGrainTable(currentStep);
            }
            else
            {
                previousStep = InitiazlizeNotEmpty(previousStep);
                currentStep = InitiazlizeNotEmpty(currentStep);
                selectedGrains = InitiazlizeNotEmpty(previousStep);
            }
            originalPicture = InitiazlizeGrainTable(originalPicture);
            GenerateGrains();
            InitialFirstStep();
            if (bitmap == null)
            {
                bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            }
            DrawGrains();

        }

        private Grain[,] InitiazlizeGrainTable(Grain[,] tab)
        {
            for (int i = 0; i < pictureBox1.Width; i++)
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    tab[i, j] = new Grain()
                    {
                        State = 0,
                        Pen = new Pen(color: Color.White)
                    };
                }
            return tab;
        }

        private Grain[,] InitiazlizeNotEmpty(Grain[,] tab)
        {
            for (int i = 0; i < pictureBox1.Width; i++)
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    if (tab[i, j].Pen.Color.A != state.Color.A &&
                        tab[i, j].Pen.Color.B != state.Color.B &&
                        tab[i, j].Pen.Color.G != state.Color.G &&
                        tab[i, j].Pen.Color.R != state.Color.R)
                    {
                        tab[i, j] = new Grain()
                        {
                            State = 0,
                            Pen = new Pen(color: Color.White)
                        };
                    }
                    
                }
            return tab;
        }

        private void DrawGrains()
        {

            Graphics g = Graphics.FromImage(bitmap);
            foreach (var grain in grains)
            {
                g.DrawRectangle(grain.Pen, grain.PositionX, grain.PositionY, 1, 1);
            }

            pictureBox1.Image = bitmap;

        }

        private void InitialFirstStep()
        {
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    foreach (var grain in grains)
                    {
                        if (i == grain.PositionX && j == grain.PositionY)
                        {
                            if (grain.Pen == state)
                            {
                                previousStep[i, j].State = 1;
                                previousStep[i, j].Pen = state;
                                currentStep[i, j].State = 1;
                                currentStep[i, j].Pen = state;
                            }
                            else
                            {
                                previousStep[i, j].State = 1;
                                previousStep[i, j].Pen = grain.Pen;
                                currentStep[i, j].State = 1;
                                currentStep[i, j].Pen = grain.Pen;
                            }

                        }

                    }
                }

            }
        }

        private void InitialStep()
        {
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    foreach (var grain in grains)
                    {
                        if (i == grain.PositionX && j == grain.PositionY)
                        {
                            if (grain.Pen == state)
                            {
                                previousStep[i, j].State = 1;
                                previousStep[i, j].Pen = state;
                            }
                            else
                            {
                                previousStep[i, j].State = 1;
                                previousStep[i, j].Pen = grain.Pen;
                            }

                        }

                    }
                }

            }
        }

        private void GenerateGrains()
        {
            Random random = new Random();
            for (int i = 0; i < Int32.Parse(textBox3.Text); i++)
            {
                int x = random.Next(0, pictureBox1.Size.Width);
                int y = random.Next(0, pictureBox1.Size.Height);
                var grain = new Grain()
                {
        
                    Pen = new Pen(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 255))),
                    PositionX = x,
                    PositionY = y,
                    State = 1
                };
                grains.Add(grain);
                colors.Add(grain.Pen);
            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var temp = 0;
            while (temp < 200)
            {
                GrowGrains();
                temp++;
            }
            

        }


        private void GrowGrains()
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

                    if (previousStep[i, j].State == 0)
                    {

                        if (i < pictureBox1.Size.Width - 1 && previousStep[i + 1, j].State == 1)
                        {
                            n++;
                            m++;
                            if (state != null &&
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i + 1, j].Pen);
                        }
                        if (i < pictureBox1.Size.Width - 1 && j < pictureBox1.Size.Height - 1 && previousStep[i + 1, j + 1].State == 1)
                        {
                            n++;
                            o++;
                            if (state != null && 
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i + 1, j + 1].Pen);
                        }
                        if (i < pictureBox1.Size.Width - 1 && j > 0 && previousStep[i + 1, j - 1].State == 1)
                        {
                            n++;
                            o++;
                            if (state != null && 
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i + 1, j - 1].Pen);
                        }
                        if (j < pictureBox1.Size.Height - 1 && previousStep[i, j + 1].State == 1)
                        {
                            n++;
                            m++;
                            if (state != null && 
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i, j + 1].Pen);
                        }
                        if (j > 0 && previousStep[i, j - 1].State == 1)
                        {
                            n++;
                            m++;
                            if (state != null && 
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i, j - 1].Pen);
                        }
                        if (i > 0 && previousStep[i - 1, j].State == 1)
                        {
                            n++;
                            m++;
                            if (state != null && 
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i - 1, j].Pen);
                        }
                        if (i > 0 && j < pictureBox1.Size.Height - 1 && previousStep[i - 1, j + 1].State == 1)
                        {
                            n++;
                            o++;
                            if (state != null && 
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i - 1, j + 1].Pen);
                        }
                        if (i > 0 && j > 0 && previousStep[i - 1, j - 1].State == 1)
                        {
                            n++;
                            o++;
                            if (state != null && 
                                currentStep[i, j].Pen.Color.A == state.Color.A &&
                                currentStep[i, j].Pen.Color.B == state.Color.B &&
                                currentStep[i, j].Pen.Color.G == state.Color.G &&
                                currentStep[i, j].Pen.Color.R == state.Color.R)
                            {
                                continue;
                            }
                            colorsTable.Add(previousStep[i - 1, j - 1].Pen);
                        }

                        int x = 1;
                        int index = 0, max = 0;

                        if (n > 1)
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
                        else if (n == 1)
                        {
                            index = 0;
                            max = 1;
                        }                       

                        if (n > 5 && n < 8)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];

                        }
                        else if (m > 3 && m < 5)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];

                           
                        }
                        else if (o > 3 && o < 5)
                        {
                            currentStep[i, j].State = 1;
                                if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];

                         
                        }
                        else if (max > 0 && probability < Int32.Parse(textBox4.Text) && n > 0)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];

                           
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
                    previousStep[i, j].Pen = currentStep[i, j].Pen;
                }
            }


            Graphics gr = Graphics.FromImage(bitmap);
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    if (previousStep[i, j].State == 1)
                    {
                        gr.DrawRectangle(currentStep[i, j].Pen, i, j, 1, 1);
                    }

                }
            }

            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"C:\Users\Olka\Desktop\Ziarna.txt"))
            {
                for (int i = 0; i < pictureBox1.Size.Width; i++)
                {
                    for (int j = 0; j < pictureBox1.Size.Height - 1; j++)
                    {

                        file.WriteLine("{0} {1} {2} {3}", i, j, currentStep[i, j].State, ColorTranslator.ToHtml(currentStep[i, j].Pen.Color));
                    }
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            int size = -1;
            string file = null;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) 
            {
                file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
            }

            grains = new List<Grain>();
            string[] lines = System.IO.File.ReadAllLines(file);

            string test = lines[0].Replace(" ", "");
            string test2 = test[0].ToString();
            string test3 = test[1].ToString();
            string test4 = test[2].ToString();
            string test5 = lines[0].Remove(0, 6);

            foreach (var line in lines)
            {

                var data = line.Split(' ');
                Grain grain = new Grain()
                {
                    PositionX = Int32.Parse(data[0]),
                    PositionY = Int32.Parse(data[1]),
                    State = Int32.Parse(data[2]),
                    Pen = new Pen(ColorTranslator.FromHtml((data[3])))
                };
                grains.Add(grain);

            }
            if (bitmap == null)
            {
                bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            }
            DrawGrains();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            List<Point> inclusions = new List<Point>();
            Graphics g = Graphics.FromImage(bitmap);
            Point point = new Point();
            Grain[,] inclusion = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];

            for (int i = 0; i < Int32.Parse(textBox5.Text); i++)
            {
                for (int j = 0; j < pictureBox1.Size.Width - 1; j++)
                {
                    for (int k = 0; k < pictureBox1.Size.Height - 1; k++)
                    {
                        if (previousStep[j, k].Pen != previousStep[j+1, k+1].Pen)
                        {
                            int pointX = j;
                            int pointY = k;
                            point = new Point(pointX, pointY);
                            inclusions.Add(point);
                        }
                    }
                }
            }

            
            for (int i = 0; i < Int32.Parse(textBox5.Text); i++)
            {
                int cor = rand.Next(inclusions.Count());
                Brush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, inclusions[cor].X, inclusions[cor].Y, Int32.Parse(textBox6.Text), Int32.Parse(textBox6.Text));
                
            }



            for (int k = 0; k < Int32.Parse(textBox5.Text); k++)
            {
                for (int i = 0; i < pictureBox1.Width; i++)
                    for (int j = 0; j < pictureBox1.Height; j++)
                    {
                        inclusion[i, j] = new Grain()
                        {
                            State = 1,
                            Pen = new Pen(color: Color.Black),
                            PositionX = inclusions[k].X,
                            PositionY = inclusions[k].Y
                        };
                    }
            }
            previousStep = InitiazlizeGrainTable(inclusion);
            pictureBox1.Image = bitmap;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            previousStep = InitiazlizeGrainTable(previousStep);
            currentStep = InitiazlizeGrainTable(currentStep);

            pictureBox1.Image = bitmap;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            List<Point> inclusions = new List<Point>();
            Graphics g = Graphics.FromImage(bitmap);
            Point point = new Point();
            Grain[,] inclusion = new Grain[pictureBox1.Size.Width, pictureBox1.Size.Height];

            for (int i = 0; i < Int32.Parse(textBox5.Text); i++)
            {
                for (int j = 0; j < pictureBox1.Size.Width - 1; j++)
                {
                    for (int k = 0; k < pictureBox1.Size.Height - 1; k++)
                    {
                        if (previousStep[j, k].Pen != previousStep[j + 1, k + 1].Pen)
                        {
                            int pointX = j; 
                            int pointY = k; 
                            point = new Point(pointX, pointY);
                            inclusions.Add(point);
                        }
                    }
                }
            }


            for (int i = 0; i < Int32.Parse(textBox5.Text); i++)
            {
                int cor = rand.Next(inclusions.Count());
                Brush brush = new SolidBrush(Color.Black);
                g.FillRectangle(brush, inclusions[cor].X, inclusions[cor].Y, Int32.Parse(textBox6.Text), Int32.Parse(textBox6.Text));

            }



            for (int k = 0; k < Int32.Parse(textBox5.Text); k++)
            {
                for (int i = 0; i < pictureBox1.Width; i++)
                    for (int j = 0; j < pictureBox1.Height; j++)
                    {
                        inclusion[i, j] = new Grain()
                        {
                            State = 1,
                            Pen = new Pen(color: Color.Black),
                            PositionX = inclusions[k].X,
                            PositionY = inclusions[k].Y
                        };
                    }
            }
            previousStep = InitiazlizeGrainTable(inclusion);
            pictureBox1.Image = bitmap;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int x = rand.Next(colors.Count);
            state = new Pen(colors[x].Color);

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    if (state.Color.A == currentStep[i, j].Pen.Color.A &&
                        state.Color.B == currentStep[i, j].Pen.Color.B &&
                        state.Color.G == currentStep[i, j].Pen.Color.G &&
                        state.Color.R == currentStep[i, j].Pen.Color.R)
                    {
                        previousStep[i, j].Pen = state;
                        previousStep[i, j].State = 1;
                        previousStep[i, j].PositionX = i;
                        previousStep[i, j].PositionY = j;
                        currentStep[i, j].Pen = state;
                        currentStep[i, j].State = 1;
                        currentStep[i, j].PositionX = i;
                        currentStep[i, j].PositionY = j;
                    }
                    else
                    {
                        previousStep[i, j].Pen = new Pen(Color.White);
                        previousStep[i, j].State = 0;
                        previousStep[i, j].PositionX = i;
                        previousStep[i, j].PositionY = j;
                        currentStep[i, j].Pen = new Pen(Color.White);
                        currentStep[i, j].State = 0;
                        currentStep[i, j].PositionX = i;
                        currentStep[i, j].PositionY = j;
                    }
                }
            }

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    selectedGrains[i, j] = currentStep[i, j];


                }
            }

            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    g.DrawRectangle(currentStep[i, j].Pen, currentStep[i, j].PositionX, currentStep[i, j].PositionY, 1, 1);
                }
                
            }            

            pictureBox1.Image = bitmap;
        }

        private void button9_Click(object sender, EventArgs e)
        {

            List<Point> boundaries = new List<Point>();
            List<Point> white = new List<Point>();
            Graphics g = Graphics.FromImage(bitmap);
            Point point = new Point();

                for (int j = 0; j < pictureBox1.Size.Width; j++)
                {
                    for (int k = 0; k < pictureBox1.Size.Height; k++)
                    {
                        
                        if ((j < pictureBox1.Size.Width - 1 && previousStep[j, k].Pen != previousStep[j + 1, k].Pen) || (k < pictureBox1.Size.Height - 1 && previousStep[j, k].Pen != previousStep[j, k + 1].Pen))
                        {
                            int pointX = j;
                            int pointY = k;
                            point = new Point(pointX, pointY);
                            boundaries.Add(point);
                        }
                        else
                        {
                            int pointX = j;
                            int pointY = k;
                            point = new Point(pointX, pointY);
                            white.Add(point);
                        }
                    }
                }


            for (int i = 0; i < boundaries.Count; i++)
            {
                Brush brush = new SolidBrush(Color.Black);
                g.FillRectangle(brush, boundaries[i].X, boundaries[i].Y, 1, 1);
            }

            for (int i = 0; i < white.Count; i++)
            {
                Brush brushWhite = new SolidBrush(Color.White);
                g.FillRectangle(brushWhite, white[i].X, white[i].Y, 1, 1);
            }

            pictureBox1.Image = bitmap;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            List<Point> boundaries = new List<Point>();
            List<Point> white = new List<Point>();
            Graphics g = Graphics.FromImage(bitmap);
            Point point = new Point();

            for (int j = 1; j < pictureBox1.Size.Width; j++)
            {
                for (int k = 0; k < pictureBox1.Size.Height; k++)
                {
                    if (previousStep[j, k].Pen.Color == Color.White)
                    {
                        point = new Point(j, k);
                        white.Add(point);
                        continue;
                    }

                    if ((j < pictureBox1.Size.Width - 1 && previousStep[j, k].Pen != previousStep[j + 1, k].Pen) || (k < pictureBox1.Size.Height - 1 && previousStep[j, k].Pen != previousStep[j, k + 1].Pen))
                    {
                        int pointX = j + 1;
                        int pointY = k;
                        point = new Point(pointX, pointY);
                        boundaries.Add(point);
                    }
                    else
                    {
                        int pointX = j;
                        int pointY = k;
                        point = new Point(pointX, pointY);
                        white.Add(point);
                    }
                }
            }

            for (int i = 0; i < white.Count; i++)
            {
                Brush brushWhite = new SolidBrush(Color.White);
                g.FillRectangle(brushWhite, white[i].X, white[i].Y, 1, 1);
            }

            for (int i = 0; i < boundaries.Count; i++)
            {
                Brush brush = new SolidBrush(Color.Black);
                g.FillRectangle(brush, boundaries[i].X, boundaries[i].Y, 1, 1);
            }

            pictureBox1.Image = bitmap;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int s = 0;
            List<PenState> penList = new List<PenState>();
            Random random = new Random();
            previousStep = InitiazlizeGrainTable(previousStep);

            if (int.Parse(textBox1.Text) > 250 || int.Parse(textBox2.Text) > 250)
            {
                pictureBox1.Size = new Size(250, 250);
            }
            else
            {
                pictureBox1.Size = new Size(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            }
            grains = new List<Grain>();

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
                    grain.PositionX = i;
                    grain.PositionY = j;
                    int x = random.Next(pens.Count);
                    grain.Pen = pens[x];
                    grain.State = x;
                    grains.Add(grain);
                    //colors.Add(grain.Pen);
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
                            previousStep[i, j].Pen = grain.Pen;
                        }

                    }
                }

            }

                DrawGrains();
            
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        public int CheckEnergy(int i, int j)
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

        private void button12_Click(object sender, EventArgs e)
        {
            var temp = 0;
            while (temp < 20)
            {
                MonteGrow();
                temp++;
            }
        }

        private void MonteGrow()
        {
            List<Grain> temp = new List<Grain>();
            foreach (Grain g in grains)
            {
                temp.Add(g);
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
                                previousStep[i, j].Pen = previousStep[i - 1, j - 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i - 1, j + 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i - 1, j].Pen;
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
                                previousStep[i, j].Pen = previousStep[i + 1, j - 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i, j - 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i + 1, j + 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i + 1, j].Pen;
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
                                previousStep[i, j].Pen = previousStep[i, j + 1].Pen;
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
                        gr.DrawRectangle(previousStep[i, j].Pen, i, j, 1, 1);

                }
            }

            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int x = rand.Next(colors.Count);
            state = new Pen(colors[x].Color);

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    if (state.Color.A == previousStep[i, j].Pen.Color.A &&
                        state.Color.B == previousStep[i, j].Pen.Color.B &&
                        state.Color.G == previousStep[i, j].Pen.Color.G &&
                        state.Color.R == previousStep[i, j].Pen.Color.R)
                    {
                        previousStep[i, j].Pen = state;
                        previousStep[i, j].State = 1;
                        previousStep[i, j].PositionX = i;
                        previousStep[i, j].PositionY = j;
                        grains.Add(previousStep[i, j]);
                    }
                    else
                    {
                        previousStep[i, j].Pen = new Pen(Color.White);
                        previousStep[i, j].State = 0;
                        previousStep[i, j].PositionX = i;
                        previousStep[i, j].PositionY = j;
                    }
                }
            }

            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    selectedGrains[i, j] = previousStep[i, j];
                    
                }
            }

            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    g.DrawRectangle(previousStep[i, j].Pen, previousStep[i, j].PositionX, previousStep[i, j].PositionY, 1, 1);
                }

            }

            pictureBox1.Image = bitmap;



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

                    
                    if (previousStep[i, j].State == 0 && previousStep[i,j].Pen != state)
                    {

                        if (i < pictureBox1.Size.Width - 1 && previousStep[i + 1, j].State == 1)
                        {
                            n++;
                            m++;
                            colorsTable.Add(previousStep[i + 1, j].Pen);
                        }
                        if (i < pictureBox1.Size.Width - 1 && j < pictureBox1.Size.Height - 1 && previousStep[i + 1, j + 1].State == 1)
                        {
                            n++;
                            o++;
                         
                            colorsTable.Add(previousStep[i + 1, j + 1].Pen);
                        }
                        if (i < pictureBox1.Size.Width - 1 && j > 0 && previousStep[i + 1, j - 1].State == 1)
                        {
                            n++;
                            o++;
                            
                            colorsTable.Add(previousStep[i + 1, j - 1].Pen);
                        }
                        if (j < pictureBox1.Size.Height - 1 && previousStep[i, j + 1].State == 1)
                        {
                            n++;
                            m++;
                            
                            colorsTable.Add(previousStep[i, j + 1].Pen);
                        }
                        if (j > 0 && previousStep[i, j - 1].State == 1)
                        {
                            n++;
                            m++;
                            
                            colorsTable.Add(previousStep[i, j - 1].Pen);
                        }
                        if (i > 0 && previousStep[i - 1, j].State == 1)
                        {
                            n++;
                            m++;
                            
                            colorsTable.Add(previousStep[i - 1, j].Pen);
                        }
                        if (i > 0 && j < pictureBox1.Size.Height - 1 && previousStep[i - 1, j + 1].State == 1)
                        {
                            n++;
                            o++;
                            
                            colorsTable.Add(previousStep[i - 1, j + 1].Pen);
                        }
                        if (i > 0 && j > 0 && previousStep[i - 1, j - 1].State == 1)
                        {
                            n++;
                            o++;
                            
                            colorsTable.Add(previousStep[i - 1, j - 1].Pen);
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
                            if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];

                        }
                        else if (m > 3 && m < 5)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];


                        }
                        else if (o > 3 && o < 5)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];


                        }
                        else if (max > 0 && probability < Int32.Parse(textBox4.Text) && n > 0)
                        {
                            currentStep[i, j].State = 1;
                            if (previousStep[i, j].Pen == state)
                            {
                                continue;

                            }
                            currentStep[i, j].Pen = colorsTable[index];


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
                    previousStep[i, j].Pen = currentStep[i, j].Pen;
                }
            }


            Graphics gr = Graphics.FromImage(bitmap);
            for (int i = 0; i < pictureBox1.Size.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Size.Height; j++)
                {
                    if (previousStep[i, j].State == 1)
                    {
                        gr.DrawRectangle(currentStep[i, j].Pen, i, j, 1, 1);
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
                    if (currentStep[i,j].State != 1)
                    {
                        grain.PositionX = i;
                        grain.PositionY = j;
                        int x = random.Next(pens.Count);
                        grain.Pen = pens[x];
                        grain.State = x + 1;
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
                            previousStep[i, j].Pen = grain.Pen;
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
                if (grain.State != 1)
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
                                previousStep[i, j].Pen = previousStep[i - 1, j - 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i - 1, j + 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i - 1, j].Pen;
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
                                previousStep[i, j].Pen = previousStep[i + 1, j - 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i, j - 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i + 1, j + 1].Pen;
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
                                previousStep[i, j].Pen = previousStep[i + 1, j].Pen;
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
                                previousStep[i, j].Pen = previousStep[i, j + 1].Pen;
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
                    gr.DrawRectangle(previousStep[i, j].Pen, i, j, 1, 1);

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
                                gr.State = -1;
                                gr.H = 0;
                                gr.Pen = new Pen(Color.FromArgb(rand.Next(0, 255), 0, 0));
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
                        g.DrawRectangle(previousStep[k, l].Pen, k, l, 10, 10);
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

                    if ((j < pictureBox1.Size.Width - 1 && previousStep[j, k].Pen != previousStep[j + 1, k].Pen) || (k < pictureBox1.Size.Height - 1 && previousStep[j, k].Pen != previousStep[j, k + 1].Pen))
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
                    g.DrawRectangle(previousStep[i, j].Pen, i, j, 1, 1);
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
                            previousStep[i, j].Pen = rec[x].Pen;
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
                            g.DrawRectangle(previousStep[k, l].Pen, k, l, 1, 1);
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
                    gr.DrawRectangle(previousStep[i, j].Pen, i, j, 1, 1);
                }
            }

        }
    }
}

