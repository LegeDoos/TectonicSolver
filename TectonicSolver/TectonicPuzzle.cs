using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegeDoos.TectonicSolver
{
    /// <summary>
    /// Implementation of the tectonic puzzle
    /// </summary>
    public class TectonicPuzzle : ITectonicPuzzle
    {

        #region -- properties

        /// <summary>
        /// Gets the state of the puzzle
        /// </summary>
        public PuzzleState State { get; private set; } = PuzzleState.Empty;

        /// <summary>
        /// Gets the width of the puzzle
        /// </summary>
        public int Width { get; private set; } = 0;

        /// <summary>
        /// Gets the Heidht of the puzzle
        /// </summary>
        public int Height { get; private set; } = 0;

        /// <summary>
        /// Gets a list of sections of the puzzle (A, B, C, ...)
        /// </summary>
        public IList<string> Sections { get; private set; }

        /// <summary>
        /// Gets the puzzle elements
        /// </summary>
        public IList<PuzzleElement> PuzzleElements { get; set; }


        #endregion

        #region -- constructors

        #endregion

        #region -- public methods

        /// <summary>
        /// Initialize the puzzle
        /// </summary>
        public void Initialize()
        {
            // Set height and width
            this.Width = this.PuzzleElements.Max(e => e.X);
            this.Height = this.PuzzleElements.Max(e => e.Y);

            // Determine sections
            this.Sections = this.PuzzleElements.GroupBy(e => e.Section).Select(s => s.First().Section).ToList();

            // Validate stucture
            this.ValidateStructure();

            // Initialize possible values
            foreach (var section in this.Sections)
            {
                var numberOfElementsInSection = this.PuzzleElements.Where(e => e.Section.Equals(section, StringComparison.OrdinalIgnoreCase)).Count();

                // init all possible values
                foreach (var element in this.PuzzleElements.Where(e => e.Section.Equals(section, StringComparison.OrdinalIgnoreCase)).Select(s => s))
                {
                    element.InitListOfPossibleValues(numberOfElementsInSection);
                }
            }

            // Set initial values
            foreach (var initialElement in this.PuzzleElements.Where(e => e.InitalValue > 0).Select(v => v))
            {
                this.SetDefiniteValueForElement(initialElement, initialElement.InitalValue);
            }

            // Validate values
            //this.ValidateValues();
        }

        /// <summary>
        /// Set the deinite value for an element
        /// </summary>
        /// <param name="element">the element</param>
        /// <param name="value">the value to set</param>
        private bool SetDefiniteValueForElement(PuzzleElement element, int value)
        {
            var success = true;

            // set element
            if (element.SetDefiniteValue(value))
            {
                // update surrounding elements
                for (int i = element.X - 1; i <= element.X + 1; i++)
                {
                    for (int j = element.Y - 1; j <= element.Y + 1; j++)
                    {
                        success = this.PuzzleElements.Where(e => e.X == i && e.Y == j).FirstOrDefault()?.RemovePossibleValue(value) ?? true && success;
                    }
                }

                // update section elements
                foreach (var e in this.PuzzleElements.Where(e => e.Section.Equals(element.Section)))
                {
                    success = e.RemovePossibleValue(value) && success;
                }

                // Validate state
                if (!success)
                    this.State = PuzzleState.Error;
                else if (this.PuzzleElements.Count(e => e.Value.Equals(0)).Equals(0))
                    this.State = PuzzleState.Solved;
            }
            else
            {
                this.State = PuzzleState.Error;
            }
            return this.State != PuzzleState.Error;
        }


        /// <summary>
        /// Recursivce method to solve the puzzle
        /// </summary>
        public void SolvePuzzle()
        {
            // Solve elements that are sure the right values
            bool cont = true;
            while (cont)
            {
                cont = false;

                // make definite items with only one possible value
                foreach (var item in this.PuzzleElements.Where(e => e.Value.Equals(0) && e.PossibleValues.Count == 1))
                {
                    cont = true && this.SetDefiniteValueForElement(item, item.PossibleValues.FirstOrDefault());
                }

                // make definite sections where only one value can be result
                foreach (var section in this.Sections)
                {
                    var elementsInSection = this.PuzzleElements.Where(e => e.Section.Equals(section, StringComparison.OrdinalIgnoreCase)).Count();
                    for (int i = 1; i <= elementsInSection; i++)
                    {
                        var elementsForI = this.PuzzleElements
                            // all elements from the section that don't have a value yet
                            .Where(e => e.Section.Equals(section, StringComparison.OrdinalIgnoreCase) && e.Value.Equals(0) && e.PossibleValues.Contains(i));

                        // if count == 1 than only one element has i
                        if (elementsForI.ToList().Count == 1)
                        {
                            cont = true && this.SetDefiniteValueForElement(elementsForI.FirstOrDefault(), i);
                        }
                    }
                }
            }

            // if not solved and not in error (valid) continue
            if (this.State.Equals(PuzzleState.Valid))
            {
                // create copy, fill in value and try to solve. If copy is solved replace values on tectonicPuzzle orig
                foreach (var element in this.PuzzleElements.Where(e => e.PossibleValues.Count > 1).OrderBy(o => o.PossibleValues.Count))
                {
                    foreach (var possibleValue in element.PossibleValues)
                    {
                        var copy = this.Copy();
                        var copyElement = copy.PuzzleElements.Where(e => e.X.Equals(element.X) && e.Y.Equals(element.Y)).FirstOrDefault();
                        copy.SetDefiniteValueForElement(copyElement, possibleValue);
                        copy.SolvePuzzle();
                        if (copy.State.Equals(PuzzleState.Solved))
                        {
                            this.PuzzleElements = copy.PuzzleElements;
                            this.State = copy.State;
                            break;
                        }
                    }
                    if (this.State.Equals(PuzzleState.Solved))
                        break;
                }
            }
        }



        #endregion

        #region -- private methods

        /// <summary>
        /// Copies the puzzle to a new object
        /// </summary>
        /// <returns>A copied instance</returns>
        private TectonicPuzzle Copy()
        {
            TectonicPuzzle tectonicPuzzleNew = new TectonicPuzzle();
            tectonicPuzzleNew.Height = this.Height;
            tectonicPuzzleNew.Width = this.Width;
            tectonicPuzzleNew.State = this.State;
            tectonicPuzzleNew.PuzzleElements = this.PuzzleElements.Clone();
            tectonicPuzzleNew.Sections = this.Sections.Clone();
            return tectonicPuzzleNew;
        }

        private void ValidateStructure()
        {
            // todo: implement
            this.State = PuzzleState.Valid;
        }

        #endregion
    }
}
