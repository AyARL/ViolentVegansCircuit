using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utility
{
    // Shuffle a list of objects T using the modern Fisher-Yates shuffle
    public static void Shuffle<T>(List<T> items)
    {
        for (int i = items.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            T temp = items[rand];
            items[rand] = items[i];
            items[i] = temp;
        }
    }
}
