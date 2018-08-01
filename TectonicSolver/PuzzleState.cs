using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegeDoos.TectonicSolver
{
    /// <summary>
    /// Represents the state of the puzzle
    /// </summary>
    public enum PuzzleState
    {
        /// <summary>
        /// Represents empty (no values loaded)
        /// </summary>
        Empty = 0,
        /// <summary>
        /// Structure of the puzzle is invaluid
        /// </summary>
        InvalidStructure = 10,
        /// <summary>
        /// Structure and values are valid
        /// </summary>
        Valid = 30,
        /// <summary>
        /// Invalid values are loaded
        /// </summary>
        Error = 20,
        /// <summary>
        /// Puzzle is solved
        /// </summary>
        Solved = 100
    }
}
