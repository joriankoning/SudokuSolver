namespace SudokuSolver
{
    internal class BackTracker
    {
        private Stack<FieldHistory> fieldHistories;
        private Sudoku sudoku;

        internal BackTracker(Sudoku sudoku)
        {
            this.sudoku = sudoku;
            fieldHistories = new Stack<FieldHistory>();
        }

        public int Start()
        {
            int positioningCounter = 0;
            int numberCounter = 1;
            int totalsteps = 0;

            while (positioningCounter < sudoku.Fieldsize * sudoku.Fieldsize)
            {
                int x = positioningCounter % sudoku.Fieldsize;
                int y = positioningCounter / sudoku.Fieldsize;
                if (sudoku.Field[y][x] == 0)
                {
                    int[][] FieldBefore = sudoku.Field.Select(a => a.ToArray()).ToArray();
                    int placedNumber = DoTurn(x, y, numberCounter);

                    if (placedNumber != 0)
                    {
                        totalsteps += new Solver(sudoku).SolveSudoku();
                        if (sudoku.isDone())
                        {
                            return totalsteps;
                        }
                        else
                        {
                            fieldHistories.Push(new FieldHistory(FieldBefore, positioningCounter, placedNumber));
                            positioningCounter++;
                            numberCounter = 1;
                            sudoku.PrintField(false);
                        }
                    }
                    else
                    {
                        try
                        {
                            FieldHistory fh = fieldHistories.Pop();
                            numberCounter = fh.Number + 1;
                            sudoku.Field = fh.Field;
                            positioningCounter = fh.Position;
                        }
                        catch
                        {
                            return totalsteps;
                        }
                    }
                }
                else
                {
                    positioningCounter++;
                }
            }
            return totalsteps;
        }

        private int DoTurn(int x, int y, int startAt)
        {
            for (int number = startAt; number <= sudoku.Fieldsize; number++)
            {
                if (sudoku.IsNumberOk(x, y, number))
                {
                    sudoku.SetNumber(x, y, number);
                    if (sudoku.IsStillSolvable(x, y, number))
                    {
                        return number;
                    }
                    sudoku.SetNumber(x, y, 0);
                }
            }
            return 0;
        }

        // de naam later veranderen
        private class FieldHistory
        {
            public int[][] Field;
            public int Position { get; set; }
            public int Number { get; set; }


            internal FieldHistory(int[][] field, int position, int number)
            {
                Field = field;
                Position = position;
                Number = number;
            }
        }
    }
}