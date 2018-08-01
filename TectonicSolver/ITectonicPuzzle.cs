using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegeDoos.TectonicSolver
{
    /// <summary>
    /// Represents a tectonic puzzle
    /// </summary>
    public interface ITectonicPuzzle
    {
        /// <summary>
        /// Gets the state of the puzzle
        /// </summary>
        PuzzleState State { get; }
        
        /// <summary>
        /// Gets the width of the puzzle
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the puzzle
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the list of sections this puzzle uses
        /// </summary>
        IList<string> Sections { get; }

        /// <summary>
        /// Gets the elements of the puzzle
        /// </summary>
        IList<PuzzleElement> PuzzleElements { get; set; }

        /// <summary>
        /// Solve the puzzle, setting the PuzzleState of the puzzle
        /// </summary>
        void SolvePuzzle();

        /// <summary>
        /// Initialize the puzzle
        /// </summary>
        void Initialize();
    }
}
