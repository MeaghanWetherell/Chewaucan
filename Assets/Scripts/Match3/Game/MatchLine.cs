using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3
{
    public class MatchLine : MonoBehaviour
    {
        public GameObject snapPoint;

        public static bool shouldRotate = false;
        
        [System.NonSerialized]
        public static int height;
        
        [System.NonSerialized]
        public int index;
        
        [System.NonSerialized]
        public MatchObject[] myObjects;
        
        [System.NonSerialized]
        public List<Transform> snapPoints = new List<Transform>();

        private static float _waitTime = 0.25f;

        public GameObject matchObjPrefab;

        private int _toSpawn;

        private float _spawnTime;

        private float scaleFactor = 0;

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

        private void UpdateScale()
        {
            Rect temp2 = ((RectTransform) snapPoints[0]).rect;
            scaleFactor = temp2.width;
            if (temp2.height < scaleFactor)
                scaleFactor = temp2.height;
        }

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

        private void AddObject()
        {
            if(scaleFactor <= 0)
                UpdateScale();
            GameObject newObj = Instantiate(matchObjPrefab, new Vector3(this.transform.position.x, 3.5f, -2), Quaternion.identity);
            newObj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
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
