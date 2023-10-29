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

        private int toSpawn = 0;

        private List<float> spawns = new List<float>();

        public int index;

        private void Awake()
        {
            myObjects = new MatchObject[size];
            StartCoroutine(spawner());
            for (int i = 0; i < myObjects.Length; i++)
            {
                spawns.Add(Time.time);
            }
        }

        private IEnumerator spawner()
        {
            while (true)
            {
                if (spawns.Count > 0 && Time.time > spawns[0]+0.25f)
                {
                    spawns.Remove(0);
                    addObject();
                    yield return new WaitForSeconds(0.25f);
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
            addObject();
        }
    }
}
