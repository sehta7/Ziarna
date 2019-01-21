using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ziarna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ziarna.Tests
{
    [TestClass()]
    public class BoardTests
    {
        
        [TestMethod()]
        public void InitializeGrainTablesTest()
        {
            //Arrange
            int size = 50;
            List<Grain> grains = new List<Grain>();
            Grain[,] grainsInPreviousStep = new Grain[size, size];
            Grain[,] grainsInCurrentStep = new Grain[size, size];
            Board board = new Board(grains, grainsInPreviousStep, grainsInCurrentStep);

            //Act
            board.InitializeGrainTables(size, size);

            //Assert
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Assert.IsNotNull(board.GrainsInPreviousStep[i, j]);
                    Assert.IsFalse(board.GrainsInPreviousStep[i, j].Alive);
                    Assert.IsFalse(board.GrainsInPreviousStep[i, j].Recrystallized);
                    Assert.AreEqual(i, board.GrainsInPreviousStep[i, j].Position.X);
                    Assert.AreEqual(j, board.GrainsInPreviousStep[i, j].Position.Y);
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Assert.IsNotNull(board.GrainsInCurrentStep[i, j]);
                    Assert.IsFalse(board.GrainsInCurrentStep[i, j].Alive);
                    Assert.IsFalse(board.GrainsInCurrentStep[i, j].Recrystallized);
                    Assert.AreEqual(i, board.GrainsInCurrentStep[i, j].Position.X);
                    Assert.AreEqual(j, board.GrainsInCurrentStep[i, j].Position.Y);
                }
            }
        }

        [TestMethod()]
        public void GenerateGrainsTest()
        {
            //Arrange
            int size = 50;
            int grainsNumber = 10;
            List<Grain> grains = new List<Grain>();
            Grain[,] grainsInPreviousStep = new Grain[size, size];
            Grain[,] grainsInCurrentStep = new Grain[size, size];
            Board board = new Board(grains, grainsInPreviousStep, grainsInCurrentStep);

            //Act
            board.GenerateGrains(grainsNumber, size, size);

            //Assert
            Assert.AreEqual(grainsNumber, board.Grains.Count);
            for (int i = 0; i < grainsNumber; i++)
            {
                Assert.IsTrue(board.Grains[i].Alive);
            }
        }
    }
}