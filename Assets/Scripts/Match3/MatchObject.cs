using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3
{
    public class MatchObject : MonoBehaviour
    {
        private static List<Mesh> meshes;

        private static List<Material> materials;
        
        private static float gravity = 20;

        public static List<int> validMeshes;

        public int myType;

        public int index;

        public MatchLine parent;
        
        private void Start()
        {
            meshes ??= Resources.Load<MeshDataList>("Match3Meshes").meshes;
            materials ??= Resources.Load<MeshDataList>("Match3Meshes").materials;
            myType = validMeshes[Random.Range((int) 0, (int) validMeshes.Count)];
            this.gameObject.GetComponent<MeshFilter>().mesh = meshes[myType];
            this.gameObject.GetComponent<MeshRenderer>().material = materials[myType];
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
    }
}
