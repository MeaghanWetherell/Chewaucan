using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class MatchLine : MonoBehaviour
    {
        public List<Transform> snapPoints;
        
        public MatchObject[] myObjects;

        private static int _size = 7;

        private static float _waitTime = 0.25f;

        public GameObject matchObjPrefab;

        private int _toSpawn;

        private float _spawnTime;

        public int index;

        private void OnEnable()
        {
            if (myObjects.Length > 0)
            {
                while(myObjects[^1] != null)
                    RemoveObject(myObjects.Length-1);
            }
            _toSpawn = _size;
            _spawnTime = Time.time + _waitTime;
            myObjects = new MatchObject[_size];
            StartCoroutine(Spawner());
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
            GameObject newObj = Instantiate(matchObjPrefab, new Vector3(this.transform.position.x, 3.5f, -2), Quaternion.identity);
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
