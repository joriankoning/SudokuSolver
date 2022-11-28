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

        /// <summary>
        /// Vult rij met getallen
        /// </summary>
        /// <param name="y">De rij die gevuld wordt</param>
        /// <param name="numbers">De getallen die in de rij komen te staan</param>
        internal void FillRow(int y, int[] numbers)
        {
            for (int x = 0; x < Fieldsize; x++)
            {
                SetNumber(x, y, numbers[x]);
            }
        }

        /// <summary>
        /// Vult een cel in het sudoku veld
        /// </summary>
        /// <param name="x">x coordinaat</param>
        /// <param name="y">y coordinaat</param>
        /// <param name="num">Nummer dat in de cel komt te staan</param>
        internal void SetNumber(int x, int y, int num)
        {
            Field[y][x] = num;
        }

        /// <summary>
        /// Controleert of een nummer op bepaalde plek mag staan
        /// </summary>
        /// <param name="x">x coordinaat</param>
        /// <param name="y">y coordinaat</param>
        /// <param name="number">Nummer dat gecontroleerd wordt</param>
        /// <returns>true als het mag volgens de sudoku regels, anders false</returns>
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

        /// <summary>
        /// Wanneer de optie ShowAllSteps op true staat of het is niet de laatste print, wordt het huidige sudoku veld op het scherm getoont
        /// De boolean lastOne zorgt er voor dat wanneer alle stappen getoont worden (ShowAllSteps), dat de laatste stap niet twee keer getoont wordt
        /// </summary>
        /// <param name="lastOne">boolean om aan te geven dat het de laatse print is van het programma</param>
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

        /// <summary>
        /// Wanneer de optie ShowAllSteps op true staat, wordt er een bericht weergegeven 
        /// en wordt vervolgens de methode aangeroepen om het veld op het scherm te tonen
        /// </summary>
        /// <param name="message">bericht dat weerggegeven wordt</param>
        internal void PrintFieldWithMessage(string message)
        {
            if (!ShowAllSteps)
                return;
            Console.WriteLine(message);
            PrintField(false);
        }

        /// <summary>
        /// Er wordt een rij nummers uit het sudoku veld geprint met leegtes tussen de blokken van een sudoku veld
        /// Een lege cel is opgeslagen als 0 en wordt hier weergegeven als een .
        /// </summary>
        /// <param name="numbers">rij nummers uit een sudoku veld</param>
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

        /// <summary>
        /// Geeft een array met nummers uit een rij terug
        /// </summary>
        /// <param name="y">y coordinaat van de rij die opgevraagd wordt</param>
        /// <returns>array met getallen uit rij y</returns>
        internal int[] GetRow(int y)
        {
            return Field[y];
        }

        /// <summary>
        /// Geeft een array met nummers uit een kolom terug
        /// </summary>
        /// <param name="x">x coordinaat van de kolom die opgevraagd wordt</param>
        /// <returns>array met getallen uit kolom x</returns>
        internal int[] GetColumn(int x)
        {
            int[] result = new int[Fieldsize];
            for (int y = 0; y < Fieldsize; y++)
            {
                result[y] = Field[y][x];
            }
            return result;
        }

        /// <summary>
        /// Zoekt welk blok er bij de x en y coordinaten hoort en geeft de getallen die hier in staan terug als matrix
        /// </summary>
        /// <param name="x">het blok bevat deze x coordinaat</param>
        /// <param name="y">het blok bevat deze y coordinaat</param>
        /// <returns>de nummers uit het blok als matrix</returns>
        internal int[][] GetBlock(int x, int y)
        {
            // x en y worden bijgesteld zodat de coordinaten aan de bovenkant en linker zijkant van het blok staan
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

        /// <summary>
        /// Geeft de getallen uit het blok met coordinaten x en y terug als array
        /// </summary>
        /// <param name="x">het blok bevat deze x coordinaat</param>
        /// <param name="y">het blok bevat deze y coordinaat</param>
        /// <returns>de nummers uit het blok als array</returns>
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

        /// <summary>
        /// Zoekt naar duplicaten in rijen, kolommen en blokken op basis van de huidige situatie van het sudoku veld
        /// </summary>
        /// <returns>true wanneer er een duplicaat is gevonden, anders false</returns>
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

        /// <summary>
        /// Controleert of de sudoku puzzel nog op te lossen is wanneer er een nummer op plek (x,y) wordt gezet
        /// dit kan voorkomen dat de backtracker een getal invult op een plek wat de sudoku onoplosbaar maakt
        /// door alle blokken er naast en er onder/boven te controleren zijn alle gevallen afgedekt
        /// </summary>
        /// <param name="x">x coordinaat</param>
        /// <param name="y">y coordinaat</param>
        /// <param name="number">nummer dat geplaatst wordt</param>
        /// <returns>true als er geen problemen gevonden worden, anders false</returns>
        internal bool IsStillSolvable(int x, int y, int number)
        {
            // x en y worden bijgesteld zodat de coordinaten aan de bovenkant en linker zijkant van het blok staan
            x = (x / Blocksize) * Blocksize;
            y = (y / Blocksize) * Blocksize;

            for (int i = 0; i < Fieldsize; i += Blocksize)
            {
                if (i == x) continue;
                if (!IsPossibleForBlock(i, y, number))
                {
                    return false;
                }
            }
            for (int i = 0; i < Fieldsize; i += Blocksize)
            {
                if (i == y) continue;
                if (!IsPossibleForBlock(x, i, number))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Controleert of er een plek is voor number in het blok met coordinaten x en y
        /// als het blok dit getal al bevat, is het goed
        /// als het blok dit getal nog niet bevat, wordt er gekeken of er een plek is waar het nummer kan staan
        /// </summary>
        /// <param name="x">het blok bevat deze x coordinaat</param>
        /// <param name="y">het blok bevat deze y coordinaat</param>
        /// <param name="number">het nummmer waar op gecontroleerd wordt</param>
        /// <returns>true als er een plek is voor number, anders false</returns>
        private bool IsPossibleForBlock(int x, int y, int number)
        {
            // x en y worden bijgesteld zodat de coordinaten aan de bovenkant en linker zijkant van het blok staan
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

        /// <summary>
        /// Zoekt duplicaten in een array, de 0 uitgesloten
        /// </summary>
        /// <param name="arryToCheck">array die gecontroleerd wordt op duplicaten</param>
        /// <returns>true als er een of meer duplicaten zijn, anders false</returns>
        private bool FindDuplicate(int[] arryToCheck)
        {
            var duplicates = arryToCheck.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Where(y => y.Key > 0)
              .Select(z => z.Key)
              .ToList();
            return duplicates.Count > 0;
        }

        /// <summary>
        /// Controleert of de sudoku opgelost is
        /// </summary>
        /// <returns>false wanneer er nog een leeg veld is, anders true</returns>
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