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

        private static int size = 7;

        private static float waitTime = 0.25f;

        public GameObject matchObjPrefab;

        private int toSpawn;

        private float spawnTime;

        public int index;

        private void OnEnable()
        {
            if (myObjects.Length > 0)
            {
                while(myObjects[^1] != null)
                    removeObject(myObjects.Length-1);
            }
            toSpawn = size;
            spawnTime = Time.time + waitTime;
            myObjects = new MatchObject[size];
            StartCoroutine(spawner());
        }

        private IEnumerator spawner()
        {
            while (true)
            {
                if (toSpawn > 0 && Time.time > spawnTime)
                {
                    toSpawn--;
                    addObject();
                    yield return new WaitForSeconds(waitTime);
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }
        }

        private void addObject()
        {
            GameObject newObj = Instantiate(matchObjPrefab, new Vector3(this.transform.position.x, 3.5f, -2), Quaternion.identity);
            myObjects[0] = newObj.GetComponent<MatchObject>();
            myObjects[0].index = 0;
            myObjects[0].parent = this;
            bubbleDown(0);
        }

        private void bubbleDown(int index)
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

        public void removeObject(int indexToRemove)
        {
            myObjects[indexToRemove].remove();
            myObjects[indexToRemove] = null;
            for (int i = indexToRemove-1; i >= 0; i--)
            {
                bubbleDown(i);
            }
            toSpawn++;
        }
    }
}
