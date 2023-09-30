using System;
using System.Collections.Generic;

namespace Misc
{
    public static class InsertionSort
    {
        //extends List to perform insertion sort in place
        //T must implement IComparable
        public static void insertionSort<T>(this List<T> list) where T : IComparable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = i;
                while (j > 0 && list[j-1].CompareTo(list[j]) > 0)
                {
                    (list[j], list[j - 1]) = (list[j - 1], list[j]);
                    j -= 1;
                }
            }
        }
    }
}