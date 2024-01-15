using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3
{
    public class MatchLine : MonoBehaviour
    {
        [Tooltip("Snap point prefab")]public GameObject snapPoint;

        //whether bone meshes should be randomly rotated after being instantiated
        public static bool shouldRotate = false;
        
        [System.NonSerialized]
        public static int height;
        
        //this line's index in the main match grid
        [System.NonSerialized]
        public int index;
        
        //all match objects subordinate to this line
        [System.NonSerialized]
        public MatchObject[] myObjects;
        
        [System.NonSerialized]
        public List<Transform> snapPoints = new List<Transform>();

        //how long the line should wait between spawning new bones
        private static float _waitTime = 0.25f;

        [Tooltip("Match object prefab")]public GameObject matchObjPrefab;

        //the number of match objects queued to be spawned
        private int _toSpawn;

        //the time at which the line should start spawning objects initially, set on enable
        private float _spawnTime;

        //determines the size of newly created match objects
        private float _scaleFactor = 0;

        //generates the snap point list and starts the spawner
        private void OnEnable()
        {
            for (int i = 0; i < height; i++)
            {
                Transform temp = Instantiate(snapPoint).transform;
                temp.SetParent(transform);
                snapPoints.Add(temp);
            }
            myObjects = new MatchObject[height];
            _spawnTime = Time.time + _waitTime;
            _toSpawn = height;
            StartCoroutine(Spawner());
        }

        //determines the scale factor for spawned match objects based on the automatic sizing given to the snap points by
        //the vertical layout group
        private void UpdateScale()
        {
            Rect temp2 = ((RectTransform) snapPoints[0]).rect;
            _scaleFactor = temp2.width;
            if (temp2.height < _scaleFactor)
                _scaleFactor = temp2.height;
        }

        //checks every frame for whether objects remain to be spawned. if any do, spawns it and sleeps for _waitTime
        private IEnumerator Spawner()
        {
            while (true)
            {
                if (_toSpawn > 0 && Time.time > _spawnTime)
                {
                    _toSpawn--;
                    AddObject();
                    yield return new WaitForSeconds(_waitTime);
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }
        }

        //spawns a new match object, setting up its scale and parentage, then moves it to the appropriate position in
        //the internal match object array
        private void AddObject()
        {
            if(_scaleFactor <= 0)
                UpdateScale();
            GameObject newObj = Instantiate(matchObjPrefab, new Vector3(this.transform.position.x, 3.5f, -2), Quaternion.identity);
            newObj.transform.localScale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);
            if (shouldRotate)
            {
                Vector3 rot = new Vector3(Random.Range(0, 45), Random.Range(0, 45), 0);
                newObj.transform.localEulerAngles = rot;
            }
            myObjects[0] = newObj.GetComponent<MatchObject>();
            myObjects[0].index = 0;
            myObjects[0].parent = this;
            BubbleDown(0);
        }

        //bubbles the object at the passed index to the lowest open spot in the match object array
        private void BubbleDown(int index)
        {
            if (myObjects[index] == null)
                return;
            for (int i = index; i <= myObjects.Length - 2; i++)
            {
                if (myObjects[i + 1] == null)
                {
                    myObjects[i + 1] = myObjects[i];
                    myObjects[i + 1].index = i + 1;
                    myObjects[i] = null;
                }
                else
                {
                    break;
                }
            }
        }

        //removes a matchobject at the specified index, performing clean up to keep the internal array accurate
        public void RemoveObject(int indexToRemove)
        {
            myObjects[indexToRemove].Remove();
            myObjects[indexToRemove] = null;
            for (int i = indexToRemove-1; i >= 0; i--)
            {
                BubbleDown(i);
            }
            _toSpawn++;
        }
    }
}
