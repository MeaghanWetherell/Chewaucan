using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Match3
{
    public class MatchObject : MonoBehaviour, IPointerDownHandler
    {
        private static List<MeshDataObj> meshes;

        private static float gravity = 20;

        public static List<int> validMeshes;

        public int myType;

        public String myGroup;

        public int index;

        public MatchLine parent;
        
        private void Start()
        {
            meshes ??= Resources.Load<MeshDataList>("Match3Meshes").meshes;
            int temp = Random.Range((int) 0, (int) validMeshes.Count);
            Debug.Log(temp);
            for (int i = 0; i < validMeshes.Count; i++)
            {
                int ree = validMeshes[i];
            }
            myType = validMeshes[temp];
            this.gameObject.GetComponent<MeshFilter>().mesh = meshes[myType].mesh;
            this.gameObject.GetComponent<MeshRenderer>().material = meshes[myType].material;
            myGroup = meshes[myType].type.ToLower();
        }

        private void Update()
        {
            Vector3 snapPoint = parent.snapPoints[index].position;
            if (this.transform.position != snapPoint)
            {
                this.transform.position = Vector3.MoveTowards(transform.position, snapPoint,
                    gravity * Time.deltaTime);
            }
        }

        public void remove()
        {
            Destroy(this.gameObject);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            MatchGrid.matchGrid.registerActiveMatchObj(this);
        }
    }
}
