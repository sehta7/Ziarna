using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziarna
{
    class Recrystallization
    {

        public static List<Grain> FindRecrystallizedGrains(Board board, int boardWidth, int boardHeight, int x, int y)
        {
            List<Grain> recrystallizedGrains = new List<Grain>();

            if (x > 0 && y > 0 && board.GrainsInPreviousStep[x - 1, y - 1].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x - 1, y - 1]);
            }

            if (x > 0 && y < boardHeight - 1 &&
                board.GrainsInPreviousStep[x - 1, y + 1].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x - 1, y + 1]);
            }

            if (x > 0 && board.GrainsInPreviousStep[x - 1, y].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x - 1, y]);
            }

            if (x < boardWidth - 1 && y > 0 &&
                board.GrainsInPreviousStep[x + 1, y - 1].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x + 1, y - 1]);
            }

            if (y > 0 && board.GrainsInPreviousStep[x, y - 1].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x, y - 1]);
            }

            if (x < boardWidth - 1 && y < boardHeight - 1 &&
                board.GrainsInPreviousStep[x + 1, y + 1].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x + 1, y + 1]);
            }

            if (x < boardWidth - 1 && board.GrainsInPreviousStep[x + 1, y].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x + 1, y]);
            }

            if (y < boardHeight - 1 && board.GrainsInPreviousStep[x, y + 1].Recrystallized)
            {
                recrystallizedGrains.Add(board.GrainsInPreviousStep[x, y + 1]);
            }

            return recrystallizedGrains;
        }
    }
}
