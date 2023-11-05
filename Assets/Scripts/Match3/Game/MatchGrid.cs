using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;

namespace Match3
{
    public class MatchGrid : MonoBehaviour
    {
        public static MatchGrid matchGrid;
        
        public List<MatchLine> lines;

        public InputActionReference mousePos;

        public InputActionReference click;

        public float minMovementMagnitude;
        
        private List<MatchObject[]> objects = new List<MatchObject[]>();

        private bool waitingOnMatchCheck;

        private Queue<List<GridCoordinate>> removalQueue = new Queue<List<GridCoordinate>>();

        private MatchObject activeObject;

        private Vector2 initialMousePos;

        private float timeLeft;

        private void OnDisable()
        {
            click.action.canceled -= clickAction;
        }

        private void clickAction(InputAction.CallbackContext context)
        {
            activeObject = null;
        }
        
        private void OnEnable()
        {
            click.action.canceled += clickAction;
            matchGrid = this;
            StartCoroutine(removeObjs());
            StartCoroutine(waitToCheckMatch(2));
            StartCoroutine(checkForLock());
        }

        private void Start()
        {
            foreach(MatchLine line in lines)
            {
                line.index = objects.Count;
                objects.Add(line.myObjects);
            }
        }

        private void FixedUpdate()
        {
            List<GridCoordinate> temp;
            if (removalQueue.TryPeek(out temp) || waitingOnMatchCheck)
                return;
            checkMatches();
            if (removalQueue.TryPeek(out temp) || waitingOnMatchCheck)
                return;
            if (activeObject != null)
            {
                Vector2 changeInMousePos = mousePos.action.ReadValue<Vector2>()-initialMousePos;
                if (changeInMousePos.magnitude > minMovementMagnitude)
                {
                    moveMatchObj(changeInMousePos);
                }
            }
        }

        IEnumerator checkForLock()
        {
            while (true)
            {
                if (!waitingOnMatchCheck)
                {
                    if (!detectMatchesPossible())
                    {
                        MatchUIManager.matchUIManager.endGame("No valid matches remaining!");
                    }
                }
                yield return new WaitForSeconds(0.6f);
            }
        }

        IEnumerator removeObjs()
        {
            while (true)
            {
                List<GridCoordinate> coords;
                if (removalQueue.TryDequeue(out coords))
                {
                    StartCoroutine(waitToCheckMatch(0.4f));
                    yield return new WaitForSeconds(0.4f);
                    ScoreTracker.scoreTracker.addScore(coords.Count);
                    foreach(GridCoordinate coord in coords)
                        lines[coord.x].removeObject(coord.y);
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
        
        private IEnumerator waitToSwapBack(float time, GridCoordinate a, GridCoordinate b)
        {
            StartCoroutine(waitToCheckMatch(time));
            yield return new WaitForSeconds(time);
            swap(a, b);
        }

        private bool detectMatchesPossible()
        {
            foreach (MatchObject[] line in objects)
            {
                if (line.Contains(null))
                    return true;
            }
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].Length; j++)
                {
                    if (i - 1 > 0 && checkSwapValid(new GridCoordinate(i,j), new GridCoordinate(i-1, j)))
                    {
                        return true;
                    }
                    if (i + 1 < objects.Count && checkSwapValid(new GridCoordinate(i,j), new GridCoordinate(i+1, j)))
                    {
                        return true;
                    }
                    if (j - 1 > 0 && checkSwapValid(new GridCoordinate(i,j), new GridCoordinate(i, j-1)))
                    {
                        return true;
                    }
                    if (j + 1 < objects[i].Length && checkSwapValid(new GridCoordinate(i,j), new GridCoordinate(i, j+1)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void checkMatches()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                for (int j = 0; j < objects[i].Length; j++)
                {
                    List<GridCoordinate> temp = detectMatch(i, j);
                    if (temp.Count >= 3)
                    {
                        removalQueue.Enqueue(temp);
                        return;
                    }
                }
            }
        }

        private List<GridCoordinate> detectMatch(int i, int j)
        {
            List<GridCoordinate> validCoords = new List<GridCoordinate>();
            if (objects[i][j] == null)
                return validCoords;
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
            return validCoords;
        }

        private List<GridCoordinate> findAltMatches(GridCoordinate coord)
        {
            List<GridCoordinate> coordActual = new List<GridCoordinate>();
            coordActual.Add(coord);
            return findAltMatches(coordActual);
        }

        private List<GridCoordinate> findAltMatches(List<GridCoordinate> coords)
        {
            HashSet<GridCoordinate> checkedCoords = new HashSet<GridCoordinate>();
            int checkType = objects[coords[0].x][coords[0].y].myType;
            List<GridCoordinate> temp = new List<GridCoordinate>();
            GridCoordinate temp2;
            while (coords.Count > 0)
            {
                GridCoordinate toCheck = coords[^1];
                coords.RemoveAt(coords.Count-1);
                if(checkedCoords.Contains(toCheck))
                    continue;
                checkedCoords.Add(toCheck);
                temp2 = findNextInDir(toCheck, -1, 0);
                temp.Add(temp2);
                if(detectMatchHelper(temp2.x, temp2.y, 1, 0, 1, checkType, temp) >= 3)
                    coords.AddRange(temp);
                temp = new List<GridCoordinate>();
                temp2 = findNextInDir(toCheck, 0, -1);
                temp.Add(temp2);
                if(detectMatchHelper(temp2.x, temp2.y, 0, 1, 1, checkType, temp) >= 3)
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

        private GridCoordinate findNextInDir(GridCoordinate coord, int dirx, int diry)
        {
            if (objects[coord.x][coord.y] == null)
                return coord;
            while (true)
            {
                if (coord.x + dirx < 0)
                    return coord;
                if (coord.x + dirx > objects.Count - 1)
                    return coord;
                if (coord.y + diry < 0)
                    return coord;
                if (coord.y + diry > objects[coord.x + dirx].Length - 1)
                    return coord;
                if (objects[coord.x + dirx][coord.y + diry] == null)
                    return coord;
                if (objects[coord.x + dirx][coord.y + diry].myType != objects[coord.x][coord.y].myType)
                    return coord;
                coord = new GridCoordinate(coord.x + dirx, coord.y + diry);
            }
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

        public void registerActiveMatchObj(MatchObject obj)
        {
            activeObject = obj;
            initialMousePos = mousePos.action.ReadValue<Vector2>();
        }

        private void moveMatchObj(Vector2 change)
        {
            if (Mathf.Abs(change.x) > Mathf.Abs(change.y))
            {
                if (change.x > 0)
                {
                    if (activeObject.parent.index + 1 < objects.Count)
                        swapWithValidityDetection(new GridCoordinate(activeObject.parent.index, activeObject.index), new GridCoordinate(activeObject.parent.index + 1, activeObject.index));
                }
                else
                {
                    if (activeObject.parent.index - 1 >= 0)
                        swapWithValidityDetection(new GridCoordinate(activeObject.parent.index, activeObject.index), new GridCoordinate(activeObject.parent.index-1, activeObject.index));
                }
            }
            else
            {
                if (change.y > 0)
                {
                    if(activeObject.index-1 >= 0)
                        swapWithValidityDetection(new GridCoordinate(activeObject.parent.index, activeObject.index), new GridCoordinate(activeObject.parent.index, activeObject.index-1));
                }
                else
                {
                    if(activeObject.index+1 < activeObject.parent.myObjects.Length)
                        swapWithValidityDetection(new GridCoordinate(activeObject.parent.index, activeObject.index), new GridCoordinate(activeObject.parent.index, activeObject.index+1));
                }
            }
            activeObject = null;
        }

        private void swapWithValidityDetection(GridCoordinate a, GridCoordinate b)
        {
            if (!checkSwapValid(a,b))
            {
                StartCoroutine(waitToSwapBack(1f, a, b));
            }
            swap(a, b);
        }

        private bool checkSwapValid(GridCoordinate a, GridCoordinate b)
        {
            if (objects[a.x][a.y] == null || objects[b.x][b.y] == null)
                return false;
            swap(a,b);
            if (findAltMatches(a).Count >= 3 ||
                findAltMatches(b).Count >= 3)
            {
                swap(a, b);
                return true;
            }
            swap(a,b);
            return false;
        }

        private void swap(GridCoordinate a, GridCoordinate b)
        {
            MatchObject vala = objects[a.x][a.y];
            MatchLine aParent = vala.parent;
            MatchObject valb = objects[b.x][b.y];
            vala.parent = valb.parent;
            vala.index = b.y;
            valb.parent = aParent;
            valb.index = a.y;
            objects[a.x][a.y] = valb;
            objects[b.x][b.y] = vala;
        }
    }
}
