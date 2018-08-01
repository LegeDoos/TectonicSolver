using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegeDoos.TectonicSolver;
using Newtonsoft.Json;

namespace TectonicSolve
{
    class Program
    {
        static void Main(string[] args)
        {
            ITectonicPuzzle puzzle;

            void WriteHeader()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Tectonic Solver by LegeDoos - (c) 2018");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }

            void Menu()
            {
                WriteHeader();
                Console.WriteLine("Menu");
                Console.WriteLine("1. Load from file");
                Console.WriteLine("2. Manual input");
                Console.WriteLine("3. Load from text file");
                Console.WriteLine("0. Quit");

                var choice = Console.ReadKey().KeyChar.ToString();
                switch (choice)
                {
                    case "1":
                        WriteHeader();
                        LoadFromFile();
                        break;
                    case "2":
                        WriteHeader();
                        ManualInput();
                        break;
                    case "3":
                        WriteHeader();
                        FromTextFile();
                        break;
                    case "0":
                        break;
                    default:
                        Menu();
                        break;
                }
            }

            void WriteState(PuzzleState state)
            {
                switch (state)
                {
                    case PuzzleState.Empty:
                        Console.WriteLine("Puzzle is empty!");
                        break;
                    case PuzzleState.InvalidStructure:
                        Console.WriteLine("Puzzle has invalid structure");
                        break;
                    case PuzzleState.Valid:
                        Console.WriteLine("Puzzle is in valid state but not solved");
                        break;
                    case PuzzleState.Error:
                        Console.WriteLine("Puzzle is not solved and in error state");
                        break;
                    case PuzzleState.Solved:
                        Console.WriteLine("Puzzle is solved!");
                        break;
                    default:
                        break;
                }
            }

            void WritePuzzle(ITectonicPuzzle p)
            {
                Console.WriteLine("");
                Console.WriteLine("Result:");
                for (int y = 0; y < p.Height; y++)
                {
                    var line = string.Empty;
                    for (int x = 0; x < p.Width; x++)
                    {
                        var element = p.PuzzleElements.FirstOrDefault(e => e.X == x + 1 && e.Y == y + 1);
                        line = string.Format("{0} : {1}_{2}", line, element?.Section, element?.Value);
                    }
                    Console.WriteLine(line);
                }
            }

            void LoadFromFile()
            {
                // Load from file
                Console.WriteLine("Load puzzle from file");
                var json = string.Empty;
                using (StreamReader reader = new StreamReader(@"c:\temp\puzzle.json"))
                {
                    json = reader?.ReadToEnd();
                }
                puzzle = JsonConvert.DeserializeObject<TectonicPuzzle>(json);
                if (puzzle != null)
                {
                    WriteState(puzzle.State);
                }
                Solve();
            }

            void ManualInput()
            {
                puzzle = new TectonicPuzzle
                {
                    PuzzleElements = new List<PuzzleElement>()
                };

                Console.WriteLine("Write the puzzle structure line by line, seperated by semicolon. Return after each line. Empty line to finish.");
                Console.WriteLine("Example:");
                Console.WriteLine();
                Console.WriteLine("A;A;A;A");
                Console.WriteLine("B;C4;C;A2");
                Console.WriteLine("D;C;C;C");
                Console.WriteLine("D;D;E;E3");
                Console.WriteLine("D;D5;E;E");
                Console.WriteLine();
                Console.WriteLine("Enter line and press 'Enter':");
                var line = Console.ReadLine();
                var lineCounter = 0;
                var charCounter = 0;

                while (!string.IsNullOrWhiteSpace(line))
                {
                    lineCounter++;
                    charCounter = 0;
                    foreach (var el in line.ToUpperInvariant().Split(';'))
                    {
                        charCounter++;
                        var section = el;
                        var initial = 0;
                        if (el.Length > 1)
                        {
                            section = el.Substring(0, 1);
                            Int32.TryParse(el.Substring(1, 1), out initial);
                        }
                        puzzle.PuzzleElements.Add(new PuzzleElement(charCounter, lineCounter, section, initial));
                    }
                    line = Console.ReadLine();
                }
                Solve();
            }

            void FromTextFile()
            {
                puzzle = new TectonicPuzzle
                {
                    PuzzleElements = new List<PuzzleElement>()
                };
                var file = string.Empty;

                using (StreamReader reader = new StreamReader(@"c:\temp\puzzle.txt"))
                {
                    var line = reader.ReadLine();
                    var lineCounter = 0;
                    var charCounter = 0;

                    while (!string.IsNullOrWhiteSpace(line))
                    {
                        lineCounter++;
                        charCounter = 0;
                        foreach (var el in line.ToUpperInvariant().Split(';'))
                        {
                            charCounter++;
                            var section = el;
                            var initial = 0;
                            if (el.Length > 1)
                            {
                                section = el.Substring(0, 1);
                                Int32.TryParse(el.Substring(1, 1), out initial);
                            }
                            puzzle.PuzzleElements.Add(new PuzzleElement(charCounter, lineCounter, section, initial));
                        }
                        line = reader.ReadLine();
                    }


                }

                Solve();
            }


            void Solve()
            {
                Console.WriteLine("Initialize puzzle");
                puzzle.Initialize();
                WriteState(puzzle.State);

                Console.WriteLine("Solve the puzzle");
                puzzle.SolvePuzzle();
                WritePuzzle(puzzle);

                Console.ForegroundColor = ConsoleColor.Red;
                WriteState(puzzle.State);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key...");
                Console.ReadLine();
                Menu();
            }

            Menu();

        }

        
    }
}
