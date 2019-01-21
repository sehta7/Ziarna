using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ziarna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna.Tests
{
    [TestClass()]
    public class BoardTests
    {
        [TestMethod()]
        public void InitializeGrainTablesTest()
        {
            //Arrange
            List<Grain> grains = new List<Grain>();
            Grain[,] grainsInPreviousStep = new Grain[2, 2];
            Grain[,] grainsInCurrentStep = new Grain[2, 2];
            Board board = new Board(grains, grainsInPreviousStep, grainsInCurrentStep);

            //Act
            board.InitializeGrainTables(2, 2);

            //Assert
            foreach (var grain in board.GrainsInCurrentStep)
            {
                Assert.IsNotNull(grain);
            }

            foreach (var grain in board.GrainsInPreviousStep)
            {
                Assert.IsNotNull(grain);
            }
        }
    }
}