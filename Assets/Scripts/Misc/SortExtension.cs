using System;
using System.Collections.Generic;

namespace Misc
{
    public static class SortExtension
    {
        //extends List to perform insertion sort in place
        //T must implement IComparable
        //any null values must be at the end of the list
        public static void InsertionSort<T>(this List<T> list) where T : IComparable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    return;
                }
                int j = i;
                while (j > 0 && list[j-1].CompareTo(list[j]) > 0)
                {
                    (list[j], list[j - 1]) = (list[j - 1], list[j]);
                    j -= 1;
                }
            }
        }
        
        //extends arrays to perform insertion sort in place
        //T must implement IComparable
        //any null values must be at the end of the list
        public static void InsertionSort<T>(this T[] list) where T : IComparable<T>
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == null)
                {
                    return;
                }
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