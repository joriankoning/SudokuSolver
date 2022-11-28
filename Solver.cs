namespace SudokuSolver
{
    /// <summary>
    /// deze class bevat de methodes die een sudoku op kunnen lossen
    /// </summary>
    internal class Solver
    {
        private Sudoku Sudoku { get; set; }
        private int Steps { get; set; }

        public Solver(Sudoku sudoku)
        {
            Sudoku = sudoku;
        }

        /// <summary>
        /// Roept de recursieve methode LogicalSolve aan
        /// </summary>
        /// <returns>Geeft het aantal tussenstappen terug dat de solver nodig heeft gehad om tot een oplossing te komen</returns>
        public int SolveSudoku()
        {
            LogicalSolve();
            return Steps;
        }

        /// <summary>
        /// Roept alle methodes voor logische oplossingen aan
        /// Wanneer er een verbetering is gemaakt wordt deze methode opnieuw recursief aangeroepen
        /// </summary>
        private void LogicalSolve()
        {
            bool emptySpots = CheckEmptySpots();
            bool rows = CheckRows();
            bool columns = CheckColumns();
            bool blocks = CheckBlocks();

            if (emptySpots || rows || columns || blocks)
            {
                Steps++;
                Sudoku.PrintFieldWithMessage($"Na {Steps} tussenstap(pen):");
                LogicalSolve();
            }
            return;
        }

        /// <summary>
        /// Elke lege plek in het sudoku veld wordt gecontroleerd of er precies 1 cijfer is die op die plek kan staan
        /// wanneer dit het geval is wordt het cijfer in de betreffende cel gezet
        /// </summary>
        /// <returns>true wanneer er een verandering is geweest, anders false</returns>
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

        /// <summary>
        /// Iedere rij van het sudoku veld wordt gecontroleerd of een cijfer precies op 1 positie kan staan in de betreffende rij
        /// wanneer dit het geval is wordt het cijfer in de betreffende cel gezet
        /// </summary>
        /// <returns>true wanneer er een verandering is geweest, anders false</returns>
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

        /// <summary>
        /// Iedere kolom van het sudoku veld wordt gecontroleerd of een cijfer precies op 1 positie kan staan in de betreffende kolom
        /// wanneer dit het geval is wordt het cijfer in de betreffende cel gezet
        /// </summary>
        /// <returns>true wanneer er een verandering is geweest, anders false</returns>
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

        /// <summary>
        /// Ieder blok in het sudoku veld wordt gecontroleerd vanuit deze methode
        /// </summary>
        /// <returns>true wanneer er een verandering is geweest, anders false</returns>
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

        /// <summary>
        /// Een blok in het sudoku veld controleren of een cijfer op precies 1 locatie kan staan
        /// wanneer dit het geval is wordt het cijfer in de betreffende cel gezet 
        /// </summary>
        /// <param name="left">de linker zijde van het blok</param>
        /// <param name="top">de bovenkant van het blok</param>
        /// <returns>true wanneer er een verandering is geweest, anders false</returns>
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