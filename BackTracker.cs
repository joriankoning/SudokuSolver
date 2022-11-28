namespace SudokuSolver
{
    /// <summary>
    /// Wanneer de sudoku niet op de logische manier opgelost kan worden wordt het geprobeerd met een backtracker
    /// Een algoritme dat elke combinatie probeert om tot oplossing te komen
    /// </summary>
    internal class BackTracker
    {
        private readonly Sudoku sudoku;

        internal BackTracker(Sudoku sudoku)
        {
            this.sudoku = sudoku;
        }

        /// <summary>
        /// Voert de backtracker uit
        /// </summary>
        /// <returns>het aantal stappen die genomen zijn</returns>
        public int Start()
        {
            // een Stack om bij te houden welke stappen er zijn gemaakt
            Stack<FieldSituation> fieldSituations = new();

            int positioningCounter = 0; // houdt de positie bij waar de backtracker bezig is, x en y coordinaten kunnen hieruit berekend worden
            int numberCounter = 1;      // nummer dat ingevuld is bij een stap van de backtracker
            int totalsteps = 0;         // totaal aantal stappen genomen

            // een loop over het hele veld totdat er een oplossing gevonden wordt
            while (positioningCounter < sudoku.Fieldsize * sudoku.Fieldsize)
            {
                int x = positioningCounter % sudoku.Fieldsize;
                int y = positioningCounter / sudoku.Fieldsize;
                if (sudoku.Field[y][x] == 0)
                {
                    // een kopie van het sudoku veld maken
                    int[][] FieldBefore = sudoku.Field.Select(a => a.ToArray()).ToArray();
                    int placedNumber = SetNextAvailableNumber(x, y, numberCounter);

                    if (placedNumber != 0)
                    {
                        // er wordt geprobeerd om de sudoku op te lossen met het zojuist ingevulde cijfer
                        sudoku.PrintFieldWithMessage("Nieuwe poging vanuit de backtracker:");
                        totalsteps += new Solver(sudoku).SolveSudoku();
                        if (sudoku.isDone())
                        {
                            return totalsteps;
                        }
                        else
                        {
                            // de sudoku is nog niet opgelost, de huidige situatie wordt opgeslagen en er wordt naar het volgende veld gekeken
                            fieldSituations.Push(new FieldSituation(FieldBefore, positioningCounter, placedNumber));
                            positioningCounter++;
                            numberCounter = 1;
                        }
                    }
                    else
                    {
                        // er is geen beschikbare oplossing meer met de huidige backtracker stap
                        if(fieldSituations.Count > 0)
                        {
                            // er wordt een stap terug gedaan
                            FieldSituation fh = fieldSituations.Pop();
                            numberCounter = fh.Number + 1;
                            sudoku.Field = fh.Field;
                            positioningCounter = fh.Position;
                        }
                        else
                        {
                            // alle stappen zijn geprobeerd, er is geen oplossing voor de ingevulde sudoku
                            return totalsteps;
                        }
                    }
                }
                else
                {
                    // de cel is niet leeg
                    positioningCounter++;
                }
            }
            // alle stappen zijn geprobeerd, er is geen oplossing voor de ingevulde sudoku
            return totalsteps;
        }

        /// <summary>
        /// zoekt het volgende beschikbare nummer voor de backtracker en vult deze in wanneer er een gevonden is
        /// </summary>
        /// <param name="x">x coordinaat</param>
        /// <param name="y">y coordinaat</param>
        /// <param name="startAt"></param>
        /// <returns>het volgende beschikbare nummer of 0 wanneer er geen nummer meer beschikbaar is</returns>
        private int SetNextAvailableNumber(int x, int y, int startAt)
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
                    // als het nummer een onoplosbare puzzel creeert wordt de stap ongedaan gemaakt
                    sudoku.SetNumber(x, y, 0);
                }
            }
            return 0;
        }

        /// <summary>
        /// class om een vorige situatie van de puzzel op te slaan
        /// </summary>
        private class FieldSituation
        {
            public int[][] Field;
            public int Position { get; private set; }
            public int Number { get; private set; }

            /// <param name="field">de invulling van het veld</param>
            /// <param name="position">positie die aangepast is</param>
            /// <param name="number">nummer die op position is gezet</param>
            internal FieldSituation(int[][] field, int position, int number)
            {
                Field = field;
                Position = position;
                Number = number;
            }
        }
    }
}