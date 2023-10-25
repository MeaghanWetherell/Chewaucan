using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Match3
{
    public class MatchGrid : MonoBehaviour
    {
        public List<MatchLine> lines;
        
        private List<MatchObject[]> objects = new List<MatchObject[]>();

        private bool waitingOnMatchCheck = false;

        private Queue<GridCoordinate> removalQueue = new Queue<GridCoordinate>();

        //TODO set up for different levels
        private void Awake()
        {
            MatchObject.validMeshes = new List<int>();
            MatchObject.validMeshes.Add(0);
            MatchObject.validMeshes.Add(1);
            MatchObject.validMeshes.Add(2);
            MatchObject.validMeshes.Add(3);
            MatchObject.validMeshes.Add(4);
            MatchObject.validMeshes.Add(5);
            MatchObject.validMeshes.Add(6);
            StartCoroutine(waitToCheckMatch(1.5f));
            StartCoroutine(removeObjs());
        }

        private void Start()
        {
            foreach(MatchLine line in lines)
            {
                objects.Add(line.myObjects);
            }
        }

        private void FixedUpdate()
        {
            GridCoordinate temp;
            if (removalQueue.TryPeek(out temp))
                return;
            checkMatches();
        }

        private void checkMatches()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].Length; j++)
                {
                    List<GridCoordinate> temp = detectMatch(i, j);
                    if (temp != null)
                    {
                        for (int k = 0; k < temp.Count; k++)
                        {
                            removalQueue.Enqueue(temp[k]);
                        }
                        return;
                    }
                }
            }
        }
        
        IEnumerator removeObjs()
        {
            while (true)
            {
                GridCoordinate coord;
                if (removalQueue.TryDequeue(out coord))
                {
                    lines[coord.x].removeObject(coord.y);
                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }
        }

        private IEnumerator waitToCheckMatch(float time)
        {
            waitingOnMatchCheck = true;
            yield return new WaitForSeconds(time);
            waitingOnMatchCheck = false;
        }

        private List<GridCoordinate> detectMatch(int i, int j)
        {
            if (objects[i][j] == null)
                return null;
            List<GridCoordinate> validCoords = new List<GridCoordinate>();
            validCoords.Add(new GridCoordinate(i, j));
            if (detectMatchHelper(i, j, 1, 0, 1, objects[i][j].myType, validCoords) >= 3)
            {
                return findAltMatches(validCoords);
            }
            validCoords = new List<GridCoordinate>();
            validCoords.Add(new GridCoordinate(i, j));
            if (detectMatchHelper(i, j, 0, 1, 1, objects[i][j].myType, validCoords) >= 3)
            {
                return findAltMatches(validCoords);
            }
            return null;
        }

        private List<GridCoordinate> findAltMatches(List<GridCoordinate> coords)
        {
            HashSet<GridCoordinate> checkedCoords = new HashSet<GridCoordinate>();
            int checkType = objects[coords[0].x][coords[0].y].myType;
            List<GridCoordinate> temp = new List<GridCoordinate>();
            while (coords.Count > 0)
            {
                GridCoordinate toCheck = coords[^1];
                coords.RemoveAt(coords.Count-1);
                if(checkedCoords.Contains(toCheck))
                    continue;
                checkedCoords.Add(toCheck);
                if(detectMatchHelper(toCheck.x, toCheck.y, 1, 0, 1, checkType, temp) >= 3)
                    coords.AddRange(temp);
                temp = new List<GridCoordinate>();
                if(detectMatchHelper(toCheck.x, toCheck.y, -1, 0, 1, checkType, temp) >= 3)
                    coords.AddRange(temp);
                temp = new List<GridCoordinate>();
                if(detectMatchHelper(toCheck.x, toCheck.y, 0, 1, 1, checkType, temp) >= 3)
                    coords.AddRange(temp);
                temp = new List<GridCoordinate>();
                if(detectMatchHelper(toCheck.x, toCheck.y, 0, -1, 1, checkType, temp) >= 3)
                    coords.AddRange(temp);
                temp = new List<GridCoordinate>();
            }
            foreach(GridCoordinate coordinate in checkedCoords)
            {
                temp.Add(coordinate);
            }
            temp.Sort();
            return temp;
        }

        private int detectMatchHelper(int i, int j, int dirx, int diry, int numInRow, int matchType, List<GridCoordinate> coords)
        {
            i += dirx;
            j += diry;
            if (i < objects.Count && i >= 0 && j < objects[i].Length && j >= 0 && objects[i][j] != null && objects[i][j].myType == matchType)
            {
                coords.Add(new GridCoordinate(i, j));
                return detectMatchHelper(i, j, dirx, diry, numInRow + 1, matchType, coords);
            }
            return numInRow;
        }
    }
}
