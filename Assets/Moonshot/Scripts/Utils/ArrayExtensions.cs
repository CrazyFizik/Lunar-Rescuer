using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public static class ArrayExtensions
    {
        public static T[] GetResizedCopy<T>(this T[] oldArray, int newSize)
        {
            T[] objArray = new T[newSize];
            int num = Mathf.Min(newSize, oldArray.Length);
            for (int index = 0; index < num; ++index)
                objArray[index] = oldArray[index];
            for (int index = num; index < objArray.Length; ++index)
                objArray[index] = default(T);
            return objArray;
        }
    }
}
