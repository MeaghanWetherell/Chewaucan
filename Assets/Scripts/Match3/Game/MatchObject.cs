using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Match3
{
    public class MatchObject : MonoBehaviour, IPointerDownHandler, IComparable<MatchObject>
    {
        public static bool compareByGroup = false;
        
        private static List<MeshDataObj> _meshes;

        private static float _gravity = 20;

        public static List<int> validMeshes;

        public int myType;

        public String myGroup;

        public int index;

        public MatchLine parent;
        
        private void Start()
        {
            _meshes ??= Resources.Load<MeshDataList>("Match3Meshes").meshes;
            int temp = Random.Range((int) 0, (int) validMeshes.Count);
            myType = validMeshes[temp];
            this.gameObject.GetComponent<MeshFilter>().mesh = _meshes[myType].mesh;
            this.gameObject.GetComponent<MeshRenderer>().material = _meshes[myType].material;
            myGroup = _meshes[myType].type.ToLower();
        }

        private void Update()
        {
            Vector3 snapPoint = parent.snapPoints[index].position;
            snapPoint = new Vector3(snapPoint.x, snapPoint.y, -1);
            if (this.transform.position != snapPoint)
            {
                this.transform.position = Vector3.MoveTowards(transform.position, snapPoint,
                    _gravity * Time.deltaTime);
            }
        }

        public void Remove()
        {
            Destroy(this.gameObject);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            MatchGrid.matchGrid.RegisterActiveMatchObj(this);
        }

        public int CompareTo(MatchObject other)
        {
            if (compareByGroup)
            {
                // ReSharper disable once StringCompareIsCultureSpecific.1
                return String.Compare(myGroup, other.myGroup);
            }
            if (myType == other.myType)
            {
                return 0;
            }
            if (myType > other.myType)
            {
                return 1;
            }
            return -1;
        }
    }
}
