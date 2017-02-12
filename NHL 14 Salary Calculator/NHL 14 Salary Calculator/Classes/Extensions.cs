using System;
using System.Collections.Generic;
using System.Linq;

namespace NHL_14_Salary_Calculator.Classes
{
    public static class Extensions
    {
        public static IEnumerable<T> Map<T>(this IEnumerable<T> enumerable, Func<T, T> func)
        {
            var items = enumerable.ToList();

            for (int index = 0; index < items.Count; index++)
            {
                items[index] = func(items[index]);
            }

            return items;
        }
    }
}