using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class MatchGrid : MonoBehaviour
    {
        public List<MatchLine> lines;
        
        private List<MatchObject[]> objects;

        private void Start()
        {
            foreach(MatchLine line in lines)
            {
                objects.Add(line.myObjects);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].Length; j++)
                {
                    (int, bool) temp = detectMatch(i, j);
                    if (temp.Item1 >= 3)
                    {
                        
                    }
                }
            }
        }

        private void removeByDir(int i, int j, bool dir, int amt)
        {
            if (dir)
            {
                amt = i + amt-1;
                while (i < amt)
                {
                    lines[i].removeObject(j);
                    i++;
                }
            }
            else
            {
                amt = j + amt-1;
                while (j < amt)
                {
                    lines[i].removeObject(j);
                    j++;
                }
            }
        }

        private (int, bool) detectMatch(int i, int j)
        {
            int temp = detectMatchHelper(i, j, 1, 0, 1, objects[i][j].myType);
            if (temp >= 3)
            {
                return (temp, true);
            }
            return (detectMatchHelper(i, j, 0, 1, 1, objects[i][j].myType), false);
        }

        private int detectMatchHelper(int i, int j, int dirx, int diry, int numInRow, int matchType)
        {
            i += dirx;
            j += diry;
            if (i < objects.Count && j < objects[i].Length && objects[i][j].myType == matchType)
            {
                return detectMatchHelper(i, j, dirx, diry, numInRow + 1, matchType);
            }
            return numInRow;
        }
    }
}
