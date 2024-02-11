using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Match3
{
    //Holds data related to a single bone on the match grid
    public class MatchObject : MonoBehaviour, IPointerDownHandler, IComparable<MatchObject>
    {
        //whether match objects should check if they are in the same general group as another when being compared
        //or check if they are of the same specific type
        public static bool compareByGroup = false;
        
        //a list containing all the possible bones that match object could be
        private static List<MeshDataObj> _meshes;

        //constant gravity scalar for manually moving bones into position
        private const float _gravity = 20;

        //a list containing indices in the main list of the possible bones a match object may become when initialized
        public static List<int> validMeshes;

        [NonSerialized]public int myType;

        [NonSerialized]public String myGroup;

        //index of the object in the match line
        [NonSerialized]public int index;

        [NonSerialized]public MatchLine parent;
        
        //loads in the main mesh list if needed and initializes itself as a random valid bone
        private void Start()
        {
            _meshes ??= Resources.Load<MeshDataList>("Meshes/Match3Meshes").meshes;
            int temp = Random.Range((int) 0, (int) validMeshes.Count);
            myType = validMeshes[temp];
            this.gameObject.GetComponent<MeshFilter>().mesh = _meshes[myType].mesh;
            this.gameObject.GetComponent<MeshRenderer>().material = _meshes[myType].material;
            myGroup = _meshes[myType].group.ToLower();
        }

        //move toward a snap point
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

        public void RemoveFromGrid()
        {
            parent.RemoveObject(index);
        }
        
        //when a match object is clicked, tells the grid that if the user tries to move a bone, it should be that one
        public void OnPointerDown(PointerEventData eventData)
        {
            MatchGrid.matchGrid.RegisterActiveMatchObj(this);
        }

        //compares itself to another matchobject per IComparable, obeying compareByGroup
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
