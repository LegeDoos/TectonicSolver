using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegeDoos.TectonicSolver
{
    static class IListExtensions
    {
        /// <summary>
        /// Clone a list of objects
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="listToClone">The list to clone</param>
        /// <returns>The cloned list</returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
