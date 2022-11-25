# sudoku-solver
Console applicatie die sudoku puzzels oplost.

bin/Debug/net6.0/SudokuSolver.exe kan gebruikt worden voor tests

Vul elke rij in van de puzzel en gebruik de 0 voor een leeg vak.
Na het invullen van elke rij wordt de sudoku opgelost.

De applicatie laat iedere tussenstap zien wanneer het veld 'ShowAllSteps' in Sudoku.cs op 'true' staat.
Deze kan op false gezet worden om het overzicht te verbeteren bij het oplossen van ingewikkelde sudoku puzzels.

De applicatie probeert eerst de puzzel op te lossen door elke rij, elke kolom en elk blok te controleren.
Wanneer het niet lukt om de puzzel op deze klasieke manier op te lossen wordt de backtracker class aangeroepen en 
wordt de puzzel alsnog opgelost met behulp van een backtracking algoritme.

Onderaan in Program.cs staan voorbeelden van sudoku puzzels 