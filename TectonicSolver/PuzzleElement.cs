using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegeDoos.TectonicSolver
{
    /// <summary>
    /// Represents one cell of the puzzle
    /// </summary>
    public class PuzzleElement : ICloneable
    {
        #region -- properties

        /// <summary>
        /// Gets the X value of the cell, from left to right
        /// </summary>
        public int X { get; private set; }
        
        /// <summary>
        /// Gets the Y value of the cell, from top to bottom
        /// </summary>
        public int Y { get; private set; }
        
        /// <summary>
        /// Gets the definite value of the cell
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets the inital value of the cell
        /// </summary>
        public int InitalValue { get; private set; }

        /// <summary>
        /// Gets or sets the list with possible (remaining) values for this cell
        /// </summary>
        public List<int> PossibleValues { get; set; }
        
        /// <summary>
        /// Gets or sets the section for this cell
        /// </summary>
        public string Section { get; private set; }

        #endregion

        #region -- constructors

        /// <summary>
        /// Construct a new puzzle element
        /// </summary>
        /// <param name="x">The x value of the element</param>
        /// <param name="y">The y value of the element</param>
        /// <param name="section">The section of the element</param>
        public PuzzleElement(int x, int y, string section, int initalValue)
        {
            this.X = x > 0 ? x : throw new Exception("X should be > 0");
            this.Y = y > 0 ? y : throw new Exception("Y should be > 0");
            this.InitalValue = initalValue;
            this.Section = section ?? throw new ArgumentNullException(nameof(section));
        }

        #endregion

        #region -- public methods

        /// <summary>
        /// Initializeas the list of possible values for this element
        /// </summary>
        /// <param name="numberOfElements"></param>
        public void InitListOfPossibleValues(int numberOfElements)
        {
            if (numberOfElements < 1)
                throw new Exception("Invalid number of elements");

            this.PossibleValues = new List<int>();
            
            for (int i = 1; i <= numberOfElements; i++)
            {
                this.PossibleValues.Add(i);
            }
        }

        /// <summary>
        /// Removes a possible value from the list
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if successfull, false on failure</returns>
        public bool RemovePossibleValue(int value)
        {
            // value already set
            if (this.Value > 0)
                return true;

            if (this.PossibleValues.Exists(v => v.Equals(value)))
                this.PossibleValues.Remove(value);

            return this.PossibleValues.Count > 0;
        }

        /// <summary>
        /// Sets the definite value of the element
        /// </summary>
        /// <param name="value">The value set</param>
        /// <returns>True if successfull, false on failure</returns>
        public bool SetDefiniteValue(int value)
        {
            if (this.PossibleValues.Exists(v => v.Equals(value)))
            {
                this.Value = value;
                this.PossibleValues = new List<int>() { value };
                return true;
            }
            return false;
        }

        public object Clone()
        {
            return new PuzzleElement(this.X, this.Y, this.Section, this.InitalValue)
            {
                Value = this.Value,
                PossibleValues = new List<int>(this.PossibleValues)
            };
            
        }

        #endregion
    }
}
