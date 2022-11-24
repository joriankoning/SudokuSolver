namespace SudokuSolver
{
    internal class Solver
    {
        Sudoku Sudoku { get; set; }

        public Solver(Sudoku sudoku)
        {
            Sudoku = sudoku;
        }

        public int SolveSudoku()
        {
            int tries = LogicalSolve();
            return tries;
        }

        private int LogicalSolve()
        {
            bool emptySpots = CheckEmptySpots();
            bool rows = CheckRows();
            bool columns = CheckColumns();
            bool blocks = CheckBlocks();

            if (emptySpots || rows || columns || blocks)
            {
                Sudoku.PrintField(false);
                return LogicalSolve() + 1;
            }
            return 0;
        }

        private bool CheckEmptySpots()
        {
            bool change = false;

            for (int y = 0; y < Sudoku.Fieldsize; y++)
            {
                for (int x = 0; x < Sudoku.Fieldsize; x++)
                {
                    if (Sudoku.Field[y][x] == 0)
                    {
                        int hits = 0;
                        int numberThatIsOk = 0;
                        foreach (int number in Sudoku.NumbersInSudoku)
                        {
                            if (Sudoku.IsNumberOk(x, y, number))
                            {
                                hits++;
                                numberThatIsOk = number;
                            }
                        }
                        if (hits == 1)
                        {
                            Sudoku.SetNumber(x, y, numberThatIsOk);
                            change = true;
                        }
                    }
                }
            }

            return change;
        }

        private bool CheckRows()
        {
            bool change = false;
            foreach (int number in Sudoku.NumbersInSudoku)
            {
                for (int y = 0; y < Sudoku.Fieldsize; y++)
                {
                    int counter = 0;
                    int numThatIsOk = 0;
                    int xThatIsOk = 0;
                    int yThatIsOk = 0;
                    for (int x = 0; x < Sudoku.Fieldsize; x++)
                    {
                        if (Sudoku.IsNumberOk(x, y, number))
                        {
                            counter++;
                            numThatIsOk = number;
                            xThatIsOk = x;
                            yThatIsOk = y;
                        }
                    }
                    if (counter == 1)
                    {
                        Sudoku.SetNumber(xThatIsOk, yThatIsOk, numThatIsOk);
                        change = true;
                    }
                }
            }
            return change;
        }

        private bool CheckColumns()
        {
            bool change = false;
            foreach (int number in Sudoku.NumbersInSudoku)
            {
                for (int x = 0; x < Sudoku.Fieldsize; x++)
                {
                    int counter = 0;
                    int numThatIsOk = 0;
                    int xThatIsOk = 0;
                    int yThatIsOk = 0;
                    for (int y = 0; y < Sudoku.Fieldsize; y++)
                    {
                        if (Sudoku.IsNumberOk(x, y, number))
                        {
                            counter++;
                            numThatIsOk = number;
                            xThatIsOk = x;
                            yThatIsOk = y;
                        }
                    }
                    if (counter == 1)
                    {
                        Sudoku.SetNumber(xThatIsOk, yThatIsOk, numThatIsOk);
                        change = true;
                    }
                }
            }
            return change;
        }

        private bool CheckBlocks()
        {
            bool change = false;
            for (int x = 0; x < Sudoku.Fieldsize; x += Sudoku.Blocksize)
            {
                for (int y = 0; y < Sudoku.Fieldsize; y += Sudoku.Blocksize)
                {
                    if (CheckOneBlock(x, y))
                    {
                        change = true;
                    }
                }
            }
            return change;
        }

        private bool CheckOneBlock(int left, int top)
        {
            bool change = false;
            foreach (int number in Sudoku.NumbersInSudoku)
            {
                int counter = 0;
                int numThatIsOk = 0;
                int xThatIsOk = 0;
                int yThatIsOk = 0;
                for (int x = 0; x < Sudoku.Blocksize; x++)
                {
                    for (int y = 0; y < Sudoku.Blocksize; y++)
                    {
                        if (Sudoku.IsNumberOk(x + left, y + top, number))
                        {
                            counter++;
                            numThatIsOk = number;
                            xThatIsOk = x + left;
                            yThatIsOk = y + top;
                        }
                    }
                }
                if (counter == 1)
                {
                    Sudoku.SetNumber(xThatIsOk, yThatIsOk, numThatIsOk);
                    change = true;
                }
            }
            return change;
        }
    }
}