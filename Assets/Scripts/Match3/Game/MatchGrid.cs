using System.Collections;
using System.Collections.Generic;
using Match3.DataClasses;
using Match3.Game;
using Misc;
using QuestSystem;
using QuestSystem.Quests.QScripts;
using TMPro;
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

        [Tooltip("Ref to the sound effect maker")] public MatchSoundEffects matchSound;

        [Tooltip("The desired height of the grid. Grid will autoscale based on this")]public int height;

        [Tooltip("The desired width of the grid. Grid will autoscale based on this")]public int width;

        [Tooltip("Prefab for the match lines")]public GameObject matchLinePrefab;

        [Tooltip("Vector 2 mouse position from the input sys")]public InputActionReference mousePos;

        [Tooltip("Left click from the input sys")]public InputActionReference click;

        [Tooltip("The minimum required amount of mouse movement for the system to register the user trying to move a bone")]
        public float minMovementMagnitude;

        [Tooltip("The amount of point bonus the player should receive for matching new bones")]
        public float bonusPoints;

        public Canvas matchCanvas;

        public GameObject flyingTextPrefab;
        
        //All match objects in the grid. Each matchobject array is a line DOWNWARDS
        private List<MatchObject[]> _objects = new List<MatchObject[]>();
        
        //all subordinate match lines
        private List<MatchLine> _lines = new List<MatchLine>();

        //whether the grid's activities should pause so that the user can visually see things
        private bool _waitingOnMatchCheck;

        //queue of coordinates of match objects to remove
        private List<List<GridCoordinate>> _removalQueue = new List<List<GridCoordinate>>();

        //the object that the user is currently holding down the cursor on
        private MatchObject _activeObject;

        //mouse position when a click on a match object is recorded
        private Vector2 _initialMousePos;
        
        //whether the game is paused
        private bool _isPaused;

        //list containing grid coords representing each valid direction on the grid (horizontal/vertical both ways)
        private GridCoordinate[][] allDirs = new GridCoordinate[2][];

        //subscribe to pause callbacks
        private void Awake()
        {
            PauseCallback.pauseManager.SubscribeToPause(PauseGrid);
            PauseCallback.pauseManager.SubscribeToResume(OnResume);
            matchGrid = this;
            allDirs[0] = new GridCoordinate[2];
            allDirs[0][0] = new GridCoordinate(1, 0);
            allDirs[0][1] = new GridCoordinate(-1, 0);
            allDirs[1] = new GridCoordinate[2];
            allDirs[1][0] = new GridCoordinate(0, 1);
            allDirs[1][1] = new GridCoordinate(0, -1);
            FillGrid();
            StartCoroutine(WaitForSpawnComplete());
            StartCoroutine(CheckForRemoval());
            StartCoroutine(CheckForLock());
        }
        
        //if the grid isn't busy, lets the user move a match object
        private void FixedUpdate()
        {
            if (_removalQueue.Count > 0 || _waitingOnMatchCheck)
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
        
        //unsubscribe to prevent leaks
        private void OnDestroy()
        {
            PauseCallback.pauseManager.UnsubToPause(PauseGrid);
            PauseCallback.pauseManager.UnsubToResume(OnResume);
        }

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
        }

        private IEnumerator WaitForSpawnComplete()
        {
            _waitingOnMatchCheck = true;
            bool continueToWait = true;
            while (continueToWait)
            {
                continueToWait = false;
                foreach (MatchLine line in _lines)
                {
                    if (line.GetToSpawn() > 0)
                        continueToWait = true;
                }
                yield return new WaitForSeconds(0);
            }
            _waitingOnMatchCheck = false;
        }

        private IEnumerator WaitForMatchCheck(float time)
        {
            _waitingOnMatchCheck = true;
            yield return new WaitForSeconds(time);
            _waitingOnMatchCheck = false;
        }

        private IEnumerator CheckForLock()
        {
            while (true)
            {
                if (_waitingOnMatchCheck || DetectMatches())
                {
                    yield return new WaitForSeconds(0.6f);
                    continue;
                }
                bool found = false;
                if (_removalQueue.Count == 0)
                {
                    for (int i = 0; i < _objects.Count; i++)
                    {
                        for (int j = 0; j < _objects[i].Length; j++)
                        {
                            GridCoordinate cur = new GridCoordinate(i, j);
                            if (i > 0)
                            {
                                Swap(new GridCoordinate(i-1, j), cur);
                                found = DetectMatches();
                                Swap(new GridCoordinate(i-1, j), cur);
                                if (found)
                                    break;
                            }
                            if (i < _objects.Count-1)
                            {
                                Swap(new GridCoordinate(i+1, j), cur);
                                found = DetectMatches();
                                Swap(new GridCoordinate(i+1, j), cur);
                                if (found)
                                    break;
                            }
                            if (j > 0)
                            {
                                Swap(new GridCoordinate(i, j-1), cur);
                                found = DetectMatches();
                                Swap(new GridCoordinate(i, j-1), cur);
                                if (found)
                                    break;
                            }
                            if (j < _objects[i].Length-1)
                            {
                                Swap(new GridCoordinate(i, j+1), cur);
                                found = DetectMatches();
                                Swap(new GridCoordinate(i, j+1), cur);
                                if (found)
                                    break;
                            }
                        }
                        if(found)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.6f);
                    continue;
                }
                if (found)
                {
                    _removalQueue = new List<List<GridCoordinate>>();
                    yield return new WaitForSeconds(0.6f);
                }
                else
                {
                    MatchLevelManager.matchLevelManager.EndGame("No matches remaining!");
                    break;
                }
            }
        }

        //check each frame if there are any matches that have been made
        private IEnumerator CheckForRemoval()
        {
            while (true)
            {
                if (_waitingOnMatchCheck)
                {
                    yield return new WaitForSeconds(0);
                    continue;
                }
                if (_removalQueue.Count == 0)
                {
                    DetectMatches();
                }
                if (_removalQueue.Count > 0)
                {
                    StartCoroutine(Remove());
                }
                yield return new WaitForSeconds(0);
            }
        }

        //while there are matches waiting for removal, remove them
        private IEnumerator Remove()
        {
            _waitingOnMatchCheck = true;
            int index = 0;
            int[] missing = new int[_lines.Count];
            while (index < _removalQueue.Count)
            {
                GameObject flyingText = Instantiate(flyingTextPrefab, matchCanvas.transform);
                RectTransform flyingTextTransform = flyingText.GetComponent<RectTransform>();
                MatchObject targ = _lines[_removalQueue[index][0].x]
                    .myObjects[_removalQueue[index][0].y];
                flyingTextTransform.anchoredPosition = targ.parent.snapPoints[targ.index].position;
                flyingText.transform.position =
                    new Vector3(flyingText.transform.position.x, flyingText.transform.position.y, -3);
                flyingTextTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
                flyingTextTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 12);
                flyingText.GetComponent<TextMeshProUGUI>().text = targ.myAnimal+" "+targ.myBoneType;
                foreach (GridCoordinate coord in _removalQueue[index])
                {
                    if (MatchLevelManager.matchLevelManager.curIndex ==
                        MatchLevelManager.matchLevelManager.levels.Count - 1)
                    {
                        MeshDataObj type = _lines[coord.x].myObjects[coord.y].GetMyType();
                        if (type.AddToMatchCount() <= EndlessModeQuestHandler.GetCountRequired(type))
                        {
                            ScoreTracker.scoreTracker.AddScore(bonusPoints);
                        }
                    }
                    _lines[coord.x].RemoveObject(coord.y);
                    missing[coord.x] += 1;
                }
                ScoreTracker.scoreTracker.AddScore(_removalQueue[index].Count);
                index++;
                yield return new WaitForSeconds(0.4f);
            }
            int max = 0;
            for (int i = 0; i < missing.Length; i++)
            {
                _lines[i].SetToSpawn(missing[i]);
                if (missing[i] > max)
                    max = missing[i];
            }
            StartCoroutine(WaitForSpawnComplete());
            _removalQueue = new List<List<GridCoordinate>>();
        }
        
        //after the passed time, swaps coordinates a and b in the grid
        private IEnumerator WaitToSwapBack(float time, GridCoordinate a, GridCoordinate b)
        {
            _waitingOnMatchCheck = true;
            yield return new WaitForSeconds(time);
            _waitingOnMatchCheck = false;
            Swap(a, b);
        }

        //registers that the user has clicked on a match object
        public void RegisterActiveMatchObj(MatchObject obj)
        {
            if (_isPaused)
                return;
            _activeObject = obj;
            _initialMousePos = mousePos.action.ReadValue<Vector2>();
        }

        private bool DetectMatches()
        {
            bool retval = false;
            for (int i = 0; i < _objects.Count; i++)
            {
                for (int j = 0; j < _objects[i].Length; j++)
                {
                    GridCoordinate cur = new GridCoordinate(i, j);
                    if(CheckContains(cur))
                        continue;
                    List<GridCoordinate> matchMain = new List<GridCoordinate>();
                    matchMain.Add(cur);
                    int index = 0;
                    while (index < matchMain.Count)
                    {
                        foreach (GridCoordinate[] hv in allDirs)
                        {
                            cur = matchMain[index];
                            List<GridCoordinate> matchTemp = new List<GridCoordinate>();
                            cur = FindFurthestInDir(cur, hv[0]);
                            matchTemp.Add(cur);
                            DetectMatchHelper(matchTemp, cur, hv[1]);
                            if (matchTemp.Count >= 3)
                            {
                                foreach (GridCoordinate coord in matchTemp)
                                {
                                    AddIfNotExist(matchMain, coord);
                                }
                            }
                        }
                        index++;
                    }
                    if (matchMain.Count >= 3)
                    {
                        _removalQueue.Add(matchMain);
                        retval = true;
                    }
                    
                }
            }
            return retval;
        }

        private GridCoordinate FindFurthestInDir(GridCoordinate cur, GridCoordinate dir)
        {
            GridCoordinate nextCandidate = new GridCoordinate(cur.x + dir.x, cur.y + dir.y);
            if (nextCandidate.x < 0 || nextCandidate.x >= _objects.Count || nextCandidate.y < 0 ||
                nextCandidate.y >= _objects[nextCandidate.x].Length)
                return cur;
            //null ref at below line possible. unsure of source but it seems very rare.
            //update: possibly fixed. should now be impossible for this to run if match lines haven't finished spawning, regardless of coroutine execution order
            if (GetByCoordinate(nextCandidate).CompareTo(GetByCoordinate(cur)) == 0)
            {
                return FindFurthestInDir(nextCandidate, dir);
            }
            return cur;
        }

        private void DetectMatchHelper(List<GridCoordinate> vals, GridCoordinate last, GridCoordinate dir)
        {
            GridCoordinate nextCandidate = new GridCoordinate(last.x + dir.x, last.y + dir.y);
            if (nextCandidate.x < 0 || nextCandidate.x >= _objects.Count || nextCandidate.y < 0 ||
                nextCandidate.y >= _objects[nextCandidate.x].Length)
                return;
            if (GetByCoordinate(nextCandidate).CompareTo(GetByCoordinate(last)) == 0)
            {
                AddIfNotExist(vals, nextCandidate);
                DetectMatchHelper(vals, nextCandidate, dir);
            }
        }

        private bool CheckContains(GridCoordinate check)
        {
            foreach (List<GridCoordinate> match in _removalQueue)
            {
                foreach (GridCoordinate coord in match)
                {
                    if (check.CompareTo(coord) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddIfNotExist(List<GridCoordinate> coords, GridCoordinate coord)
        {
            foreach (GridCoordinate check in coords)
            {
                if (check.CompareTo(coord) == 0)
                    return;
            }
            coords.Add(coord);
        }

        private MatchObject GetByCoordinate(GridCoordinate coord)
        {
            return _objects[coord.x][coord.y];
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

        private void PauseGrid()
        {
            _isPaused = true;
        }

        private void OnResume()
        {
            _isPaused = false;
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
            Swap(a, b);
            if (!DetectMatches())
            {
                //matchSound.PlayAw();
                StartCoroutine(WaitToSwapBack(1f, a, b));
            }
            else
            {
                //matchSound.PlayYay();
                StartCoroutine(WaitForMatchCheck(0.4f));
            }
        }
    }
}
