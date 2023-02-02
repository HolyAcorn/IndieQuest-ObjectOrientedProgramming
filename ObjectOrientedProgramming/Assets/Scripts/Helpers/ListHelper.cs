using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

namespace MonsterQuest
{
    public static class ListHelper
    {
        public static IList<T> Shuffle<T>(this IList<T> t)
        {
            int count = t.Count;
            for (int i = count - 1; i >= 0; --i)
            {
                int j = Random.Range(i, count);
                T tempI = t[i];
                t[i] = t[j];
                t[j] = tempI;
            }
            return t;
        }
    }
}
