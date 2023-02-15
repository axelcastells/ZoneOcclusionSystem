using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void RemoveDuplicates<T>(this List<T> elementsList)
    {
        for (int i = 0; i < elementsList.Count; i++)
        {
            for (int j = i + 1; j < elementsList.Count; j++)
            {
                if (elementsList[i].Equals(elementsList[j]))
                {
                    elementsList.RemoveAt(j);
                    j--;
                }
            }
        }
    }
}
