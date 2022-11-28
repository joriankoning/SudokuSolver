using System.Text.RegularExpressions;

namespace SudokuSolver
{
    /// <summary>
    /// Deze class geeft een interface tussen het programma en de gebruiker, daarna wordt de oplosser gestart
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// de Main methode zet een nieuw bord op, voert de oplosser uit en laat het resultaat zien in de console
        /// </summary>
        public static void Main()
        {
            Sudoku sudoku = new();
            Console.WriteLine("Vul voor per rij alle getallen in, gebruik voor lege vakjes het getal 0");
            FillFieldWithUserInput(sudoku);
            Console.WriteLine("Ingevulde sudoku:");
            sudoku.PrintField(true);
            if (sudoku.HasDuplicates())
            {
                Console.WriteLine("Ongeldige sudoku ingevoerd!");
            }
            else
            {
                int steps = new Solver(sudoku).SolveSudoku();
                if (!sudoku.isDone())
                {
                    Console.WriteLine($"Tussenresultaat na {steps} tussenstap(pen)");
                    sudoku.PrintField(true);
                    steps += new BackTracker(sudoku).Start();
                    Console.WriteLine($"Eindresultaat met backtracking na {steps} tussenstap(pen)");
                    sudoku.PrintField(true);
                }
                else
                {
                    Console.WriteLine($"Eindresultaat na {steps} tussenstap(pen)");
                    sudoku.PrintField(true);
                }
            }
        }

        /// <summary>
        /// Deze methode laat de gebruiker een nieuwe sudoku invullen
        /// </summary>
        /// <param name="sudoku">Het sudoku veld dat gevuld wordt</param>
        private static void FillFieldWithUserInput(Sudoku sudoku)
        {
            int size = sudoku.Fieldsize;
            for (int y = 0; y < size;)
            {
                Console.WriteLine($"Nummers van de {y + 1}e rij:");
                string row = Console.ReadLine() ?? "";

                if (!InputCheck(row, size))
                {
                    continue;
                }
                char[] chars = row.ToCharArray();
                int[] numbers = new int[size];
                for (int x = 0; x < chars.Length; x++)
                {
                    numbers[x] = chars[x] - '0';
                }
                sudoku.FillRow(y, numbers);
                y++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// De ingevulde regel wordt gecontroleerd op lengte en het de nummers
        /// </summary>
        /// <param name="row">de ingevulde regel</param>
        /// <param name="size">de grootte van een regel van het sudoku veld</param>
        /// <returns></returns>
        private static bool InputCheck(string row, int size)
        {
            if (row.Length != size)
            {
                Console.WriteLine($"FOUT: rij moet {size} nummers lang zijn");
                return false;
            }
            if (!Regex.IsMatch(row, $"^[0-{size}]+$"))
            {
                Console.WriteLine($"FOUT: alleen nummers van 0-{size} invullen aub.");
                return false;
            }
            return true;
        }
    }
}
/*
Voorbeelden om de oplosser te testen:

Zonder backtracker
080500091
015040723
000001000
000624000
008000300
000005906
009000050
000070000
271000409

Met backtracker
070008504
690100000
003002007
010090000
004005000
500700000
080000900
000207100
056000078

Nog een met backtracker
025000490
400206005
900040006
100000002
300060007
060000050
001000300
000501000
800070009
*/