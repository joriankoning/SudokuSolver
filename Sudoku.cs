namespace SudokuSolver
{
    /// <summary>
    /// Deze class representeert het sudoku veld en bevat controles en methoden om getallen uit het veld op te vragen 
    /// </summary>
    internal class Sudoku
    {
        public int[][] Field { get; set; }

        public int Fieldsize { get => 9; }
        public int Blocksize { get => 3; }
        public int[] NumbersInSudoku { get; }
        private bool ShowAllSteps { get => true; }

        public Sudoku()
        {
            Field = new int[Fieldsize][].Select(row => new int[Fieldsize]).ToArray();

            NumbersInSudoku = new int[Fieldsize];
            for (int i = 1; i <= Fieldsize; i++)
                NumbersInSudoku[i - 1] = i;
        }

        internal void FillRow(int y, int[] numbers)
        {
            for (int x = 0; x < Fieldsize; x++)
            {
                SetNumber(x, y, numbers[x]);
            }
        }

        internal void SetNumber(int x, int y, int num)
        {
            Field[y][x] = num;
        }

        internal bool IsNumberOk(int x, int y, int number)
        {
            if (Field[y][x] != 0)
                return false;

            int[] row = GetRow(y);
            int[] column = GetColumn(x);
            int[] block = GetBlockAsRow(x, y);

            if (row.Contains(number))
                return false;
            if (column.Contains(number))
                return false;
            if (block.Contains(number))
                return false;

            return true;
        }

        internal void PrintField(bool lastOne)
        {
            if (!lastOne && !ShowAllSteps)
                return;
            Console.WriteLine("---------------------------------------------------\n");
            for (int y = 0; y < Fieldsize; y++)
            {
                PrintRow(GetRow(y));
                if ((y + 1) % Blocksize == 0) Console.WriteLine();
            }
        }

        private void PrintRow(int[] numbers)
        {
            int i = 0;
            foreach (int num in numbers)
            {
                if (++i % Blocksize == 1) Console.Write(" ");
                if (num == 0) Console.Write(".");
                else Console.Write(num);
            }
            Console.Write("\n");
        }

        internal int[] GetRow(int y)
        {
            return Field[y];
        }

        internal int[] GetColumn(int x)
        {
            int[] result = new int[Fieldsize];
            for (int y = 0; y < Fieldsize; y++)
            {
                result[y] = Field[y][x];
            }
            return result;
        }

        internal int[][] GetBlock(int x, int y)
        {
            x = (x / Blocksize) * Blocksize;
            y = (y / Blocksize) * Blocksize;

            int[][] result = new int[Blocksize][];

            for (int blockY = 0; blockY < Blocksize; blockY++)
            {
                result[blockY] = new int[Blocksize];
                for (int blockX = 0; blockX < Blocksize; blockX++)
                {
                    result[blockY][blockX] = Field[y + blockY][x + blockX];
                }
            }
            return result;
        }

        internal int[] GetBlockAsRow(int x, int y)
        {
            int[][] numbersInBlock = GetBlock(x, y);
            int[] result = new int[Fieldsize];
            int counter = 0;
            foreach (int[] blockRow in numbersInBlock)
            {
                foreach (int number in blockRow)
                {
                    result[counter++] = number;
                }
            }
            return result;
        }

        internal bool HasDuplicates()
        {
            for (int i = 0; i < Fieldsize; i++)
            {
                if (FindDuplicate(GetColumn(i))) return true;
                if (FindDuplicate(GetRow(i))) return true;
                if (i % Blocksize == 0)
                {
                    for (int j = 0; j < Blocksize; j++)
                    {
                        if (FindDuplicate(GetBlockAsRow(i, j * 3))) return true;
                    }
                }
            }
            return false;
        }

        internal bool IsStillSolvable(int x, int y, int number)
        {
            x = (x / Blocksize) * Blocksize;
            y = (y / Blocksize) * Blocksize;

            for (int i = 0; i < Fieldsize; i += 3)
            {
                if (i == x) continue;
                if (!IsPossibleInBlock(i, y, number))
                {
                    return false;
                }
            }
            for (int i = 0; i < Fieldsize; i += 3)
            {
                if (i == y) continue;
                if (!IsPossibleInBlock(x, i, number))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsPossibleInRow(int skip, int y, int number)
        {
            for (int x = 0; x < Fieldsize; x++)
            {
                if (x == skip)
                {
                    continue;
                }
                if (IsNumberOk(x, y, number))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPossibleInColumn(int x, int skip, int number)
        {
            for (int y = 0; y < Fieldsize; y++)
            {
                if (y == skip)
                {
                    continue;
                }
                if (IsNumberOk(x, y, number))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPossibleInBlock(int x, int y, int number)
        {
            x = (x / Blocksize) * Blocksize;
            y = (y / Blocksize) * Blocksize;

            int[] block = GetBlockAsRow(x, y);
            if (block.Contains(number))
            {
                return true;
            }

            for (int blockX = 0; blockX < Blocksize; blockX++)
            {
                for (int blockY = 0; blockY < Blocksize; blockY++)
                {
                    if (IsNumberOk(x + blockX, y + blockY, number))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FindDuplicate(int[] arryToCheck)
        {
            var duplicates = arryToCheck.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Where(y => y.Key > 0)
              .Select(z => z.Key)
              .ToList();
            return duplicates.Count > 0;
        }

        internal bool isDone()
        {
            foreach (int[] row in Field)
            {
                foreach (int num in row)
                {
                    if (num == 0) return false;
                }
            }
            return true;
        }
    }
}