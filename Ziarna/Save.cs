using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ziarna
{
    class Save
    {
        private static String Path = "C:\\Users\\Grains.txt";

        public static void Import(int boardWidth, int boardHeight, Grain[,] grainsBoard)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@Path))
            {
                for (int i = 0; i < boardWidth - 1; i++)
                {
                    for (int j = 0; j < boardHeight - 1; j++)
                    {
                        file.WriteLine("{0} {1} {2} {3}", i, j, grainsBoard[i, j].Alive, ColorTranslator.ToHtml(grainsBoard[i, j].GetPenColor().Color));
                    }
                }

            }
        }

        public static List<Grain> Export(OpenFileDialog openFileDialog)
        {
            int fileSize;
            String fileName = null;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                try
                {
                    String fileContent = File.ReadAllText(fileName);
                    fileSize = fileContent.Length;
                }
                catch (IOException e)
                {
                    Console.WriteLine("Read file error: " + e.Message);
                }
            }
            return ReadGrainsFromFile(fileName);
        }

        private static List<Grain> ReadGrainsFromFile(string fileName)
        {
            String[] allGrains = System.IO.File.ReadAllLines(fileName);
            return ExtractData(allGrains);
        }

        private static List<Grain> ExtractData(String[] allGrains)
        {
            List<Grain> grains = new List<Grain>();

            foreach (var line in allGrains)
            {
                String[] grainInLine = line.Split(' ');
                int x = Int32.Parse(grainInLine[0]);
                int y = Int32.Parse(grainInLine[1]);
                bool alive = Convert.ToBoolean(grainInLine[2]);
                Pen penColor = new Pen(ColorTranslator.FromHtml((grainInLine[3])));

                Grain grain = new Grain(new Point(x, y), penColor);
                if (alive == true)
                {
                    grain.SetGrainAlive();
                }
                grains.Add(grain);
            }
            return grains;
        }
    }
}
