using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Game;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

// ReSharper disable once CheckNamespace
namespace Match3
{
    //main monolithic manager for the match 3 system.
    public class MatchGrid : MonoBehaviour
    {
        public static MatchGrid matchGrid;

        [FormerlySerializedAs("sound")] [Tooltip("Ref to the sound effect maker")] public MatchSoundEffects matchSound;

        [Tooltip("The desired height of the grid. Grid will autoscale based on this")]public int height;

        [Tooltip("The desired width of the grid. Grid will autoscale based on this")]public int width;

        [Tooltip("Prefab for the match lines")]public GameObject matchLinePrefab;

        [Tooltip("Vector 2 mouse position from the input sys")]public InputActionReference mousePos;

        [Tooltip("Left click from the input sys")]public InputActionReference click;

        [Tooltip("The minimum required amount of mouse movement for the system to register the user trying to move a bone")]
        public float minMovementMagnitude;
        
        //All match objects in the grid
        private List<MatchObject[]> _objects = new List<MatchObject[]>();
        
        //all subordinate match lines
        private List<MatchLine> _lines = new List<MatchLine>();

        //whether the grid's activities should pause so that the user can visually see things
        private bool _waitingOnMatchCheck;

        //queue of coordinates of match objects to remove
        private Queue<List<MatchObject>> _removalQueue = new Queue<List<MatchObject>>();

        //the object that the user is currently holding down the cursor on
        private MatchObject _activeObject;

        //mouse position when a click on a match object is recorded
        private Vector2 _initialMousePos;

        private void OnDisable()
        {
            click.action.canceled -= ClickAction;
        }

        private void ClickAction(InputAction.CallbackContext context)
        {
            _activeObject = null;
        }
        
        private void OnEnable()
        {
            click.action.canceled += ClickAction;
            matchGrid = this;
            FillGrid();
            StartCoroutine(RemoveObjs());
            StartCoroutine(WaitToCheckMatch(2));
            StartCoroutine(CheckForLock());
        }

        //if the grid isn't busy, lets the user move a match object
        private void FixedUpdate()
        {
            // ReSharper disable once NotAccessedVariable
            List<MatchObject> temp;
            if (_removalQueue.TryPeek(out temp) || _waitingOnMatchCheck)
                return;
            CheckMatches();
            if (_removalQueue.TryPeek(out temp) || _waitingOnMatchCheck)
                return;
            if (_activeObject != null)
            {
                Vector2 changeInMousePos = mousePos.action.ReadValue<Vector2>()-_initialMousePos;
                if (changeInMousePos.magnitude > minMovementMagnitude)
                {
                    MoveMatchObj(changeInMousePos);
                }
            }
        }

        //initializes the grid
        private void FillGrid()
        {
            MatchLine.height = height;
            for (int i = 0; i < width; i++)
            {
                Transform temp = Instantiate(matchLinePrefab).transform;
                temp.SetParent(transform);
                _lines.Add(temp.GetComponent<MatchLine>());
            }
            foreach(MatchLine line in _lines)
            {
                line.index = _objects.Count;
                _objects.Add(line.myObjects);
            }
        }

        //every 0.6 seconds, checks the game for a lock state and ends the game on lock
        IEnumerator CheckForLock()
        {
            while (true)
            {
                if (!_waitingOnMatchCheck)
                {
                    if (!DetectMatchesPossible())
                    {
                        MatchUIManager.matchUIManager.EndGame("No valid matches remaining!");
                        break;
                    }
                }
                yield return new WaitForSeconds(0.6f);
            }
        }

        //coroutine to remove matched objects at 1 group per 0.4 seconds so the user can actually visibly see them being removed
        IEnumerator RemoveObjs()
        {
            while (true)
            {
                List<MatchObject> coords;
                if (_removalQueue.TryDequeue(out coords))
                {
                    StartCoroutine(WaitToCheckMatch(0.4f));
                    yield return new WaitForSeconds(0.4f);
                    ScoreTracker.scoreTracker.AddScore(coords.Count);
                    foreach(MatchObject coord in coords)
                        coord.RemoveFromGrid();
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }
        }

        //pauses certain activies of the manager for the specified time, such as lock checks and moving match objects
        private IEnumerator WaitToCheckMatch(float time)
        {
            _waitingOnMatchCheck = true;
            yield return new WaitForSeconds(time);
            _waitingOnMatchCheck = false;
        }
        
        //after the passed time, swaps coordinates a and b in the grid
        private IEnumerator WaitToSwapBack(float time, GridCoordinate a, GridCoordinate b)
        {
            StartCoroutine(WaitToCheckMatch(time));
            yield return new WaitForSeconds(time);
            Swap(a, b);
        }

        //detects whether there are any valid matches to be made on the grid by brute force
        //performance intensive.
        private bool DetectMatchesPossible()
        {
            foreach (MatchObject[] line in _objects)
            {
                if (line.Contains(null))
                    return true;
            }
            for (int i = 0; i < _objects.Count; i++)
            {
                for (int j = 0; j < _objects[i].Length; j++)
                {
                    if (i - 1 > 0 && CheckSwapValid(new GridCoordinate(i,j), new GridCoordinate(i-1, j)))
                    {
                        return true;
                    }
                    if (i + 1 < _objects.Count && CheckSwapValid(new GridCoordinate(i,j), new GridCoordinate(i+1, j)))
                    {
                        return true;
                    }
                    if (j - 1 > 0 && CheckSwapValid(new GridCoordinate(i,j), new GridCoordinate(i, j-1)))
                    {
                        return true;
                    }
                    if (j + 1 < _objects[i].Length && CheckSwapValid(new GridCoordinate(i,j), new GridCoordinate(i, j+1)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //checks the grid for the first valid match from top to bottom right to left. 
        //on finding one, queues it for removal and returns.
        private void CheckMatches()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                for (int j = 0; j < _objects[i].Length; j++)
                {
                    List<GridCoordinate> temp = DetectMatch(i, j);
                    if (temp.Count >= 3)
                    {
                        List<MatchObject> temp2 = new List<MatchObject>();
                        foreach(GridCoordinate coord in temp)
                        {
                            temp2.Add(_objects[coord.x][coord.y]);
                        }
                        _removalQueue.Enqueue(temp2);
                        return;
                    }
                }
            }
        }

        //detects if there is a match either down or to the right from the passed indices
        private List<GridCoordinate> DetectMatch(int i, int j)
        {
            List<GridCoordinate> validCoords = new List<GridCoordinate>();
            if (_objects[i][j] == null)
                return validCoords;
            validCoords.Add(new GridCoordinate(i, j));
            if (DetectMatchHelper(i, j, 1, 0, 1, _objects[i][j], validCoords) >= 3)
            {
                return FindAltMatches(validCoords);
            }
            validCoords = new List<GridCoordinate>();
            validCoords.Add(new GridCoordinate(i, j));
            if (DetectMatchHelper(i, j, 0, 1, 1, _objects[i][j], validCoords) >= 3)
            {
                return FindAltMatches(validCoords);
            }
            return validCoords;
        }

        //finds if there any additional matches "hanging on" to another match, such as in an L or T pattern
        private List<GridCoordinate> FindAltMatches(GridCoordinate coord)
        {
            List<GridCoordinate> coordActual = new List<GridCoordinate>();
            coordActual.Add(coord);
            return FindAltMatches(coordActual);
        }

        //helper overload for FindAltMatches
        private List<GridCoordinate> FindAltMatches(List<GridCoordinate> coords)
        {
            HashSet<GridCoordinate> checkedCoords = new HashSet<GridCoordinate>();
            MatchObject checkAgainst = _objects[coords[0].x][coords[0].y];
            List<GridCoordinate> temp = new List<GridCoordinate>();
            GridCoordinate temp2;
            while (coords.Count > 0)
            {
                GridCoordinate toCheck = coords[^1];
                coords.RemoveAt(coords.Count-1);
                if(checkedCoords.Contains(toCheck))
                    continue;
                checkedCoords.Add(toCheck);
                temp2 = FindNextInDir(toCheck, -1, 0);
                temp.Add(temp2);
                if(DetectMatchHelper(temp2.x, temp2.y, 1, 0, 1, checkAgainst, temp) >= 3)
                    coords.AddRange(temp);
                temp = new List<GridCoordinate>();
                temp2 = FindNextInDir(toCheck, 0, -1);
                temp.Add(temp2);
                if(DetectMatchHelper(temp2.x, temp2.y, 0, 1, 1, checkAgainst, temp) >= 3)
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

        //finds the furthest valid coordinate in the direction given by dirx and diry that is equal to the object
        //at the passed coordinate. 
        private GridCoordinate FindNextInDir(GridCoordinate coord, int dirx, int diry)
        {
            if (_objects[coord.x][coord.y] == null)
                return coord;
            while (true)
            {
                if (coord.x + dirx < 0)
                    return coord;
                if (coord.x + dirx > _objects.Count - 1)
                    return coord;
                if (coord.y + diry < 0)
                    return coord;
                if (coord.y + diry > _objects[coord.x + dirx].Length - 1)
                    return coord;
                if (_objects[coord.x + dirx][coord.y + diry] == null)
                    return coord;
                if (_objects[coord.x + dirx][coord.y + diry].CompareTo(_objects[coord.x][coord.y]) != 0)
                    return coord;
                coord = new GridCoordinate(coord.x + dirx, coord.y + diry);
            }
        }

        //recursive helper function for match detection that checks if there is a match of 3 or more starting at
        //coordinate i j and moving in the passed direction
        private int DetectMatchHelper(int i, int j, int dirx, int diry, int numInRow, MatchObject checkAgainst, List<GridCoordinate> coords)
        {
            i += dirx;
            j += diry;
            if (i < _objects.Count && i >= 0 && j < _objects[i].Length && j >= 0 && _objects[i][j] != null && checkAgainst.CompareTo(_objects[i][j]) == 0)
            {
                coords.Add(new GridCoordinate(i, j));
                return DetectMatchHelper(i, j, dirx, diry, numInRow + 1, checkAgainst, coords);
            }
            return numInRow;
        }

        //registers that the user has clicked on a match object
        public void RegisterActiveMatchObj(MatchObject obj)
        {
            _activeObject = obj;
            _initialMousePos = mousePos.action.ReadValue<Vector2>();
        }

        //swaps the active match object based on the direction the user moved the mouse
        private void MoveMatchObj(Vector2 change)
        {
            if (Mathf.Abs(change.x) > Mathf.Abs(change.y))
            {
                if (change.x > 0)
                {
                    if (_activeObject.parent.index + 1 < _objects.Count)
                        SwapWithValidityDetection(new GridCoordinate(_activeObject.parent.index, _activeObject.index), new GridCoordinate(_activeObject.parent.index + 1, _activeObject.index));
                }
                else
                {
                    if (_activeObject.parent.index - 1 >= 0)
                        SwapWithValidityDetection(new GridCoordinate(_activeObject.parent.index, _activeObject.index), new GridCoordinate(_activeObject.parent.index-1, _activeObject.index));
                }
            }
            else
            {
                if (change.y > 0)
                {
                    if(_activeObject.index-1 >= 0)
                        SwapWithValidityDetection(new GridCoordinate(_activeObject.parent.index, _activeObject.index), new GridCoordinate(_activeObject.parent.index, _activeObject.index-1));
                }
                else
                {
                    if(_activeObject.index+1 < _activeObject.parent.myObjects.Length)
                        SwapWithValidityDetection(new GridCoordinate(_activeObject.parent.index, _activeObject.index), new GridCoordinate(_activeObject.parent.index, _activeObject.index+1));
                }
            }
            _activeObject = null;
        }

        //swaps the objects at a and b. if that swap does not cause a match, swaps them back after 1 second
        private void SwapWithValidityDetection(GridCoordinate a, GridCoordinate b)
        {
            if (!CheckSwapValid(a,b))
            {
                matchSound.PlayAw();
                StartCoroutine(WaitToSwapBack(1f, a, b));
            }
            else
            {
                matchSound.PlayYay();
            }
            Swap(a, b);
        }

        //checks if swapping a and b creates a match
        private bool CheckSwapValid(GridCoordinate a, GridCoordinate b)
        {
            if (_objects[a.x][a.y] == null || _objects[b.x][b.y] == null)
                return false;
            Swap(a,b);
            if (FindAltMatches(a).Count >= 3 ||
                FindAltMatches(b).Count >= 3)
            {
                Swap(a, b);
                return true;
            }
            Swap(a,b);
            return false;
        }

        //swaps the positions of the match objects at a and b
        private void Swap(GridCoordinate a, GridCoordinate b)
        {
            MatchObject vala = _objects[a.x][a.y];
            MatchLine aParent = vala.parent;
            MatchObject valb = _objects[b.x][b.y];
            vala.parent = valb.parent;
            vala.index = b.y;
            valb.parent = aParent;
            valb.index = a.y;
            _objects[a.x][a.y] = valb;
            _objects[b.x][b.y] = vala;
        }
    }
}
