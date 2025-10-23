using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Match3.DataClasses;
using Match3.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
// ReSharper disable StringCompareIsCultureSpecific.1

namespace Match3
{
    //Holds data related to a single bone on the match grid
    public class MatchObject : MonoBehaviour, IPointerDownHandler, IComparable<MatchObject>
    {
        public enum MatchType
        {
            identical,
            sameBone,
            sameSpecies
        }
        //the type of comparison on the meshes
        private static MatchType mtype = MatchType.identical;
        
        //a list containing all the possible bones that match object could be
        private static List<MeshDataObj> _meshes;

        //constant gravity scalar for manually moving bones into position
        private const float _gravity = 250;

        //a list containing indices in the main list of the possible bones a match object may become when initialized
        public static List<int> validMeshes;

        [NonSerialized]public int myType;

        [NonSerialized]public String myAnimal;
        
        [NonSerialized]public String myBoneType;

        //index of the object in the match line
        [NonSerialized]public int index;

        [NonSerialized]public MatchLine parent;

        //loads in the main mesh list if needed and initializes itself as a random valid bone
        private void Start()
        {
            _meshes ??= Resources.Load<MeshDataList>("Meshes/Match3Meshes").meshes;
            int temp = Random.Range((int) 0, (int) validMeshes.Count);
            myType = validMeshes[temp];
            Instantiate(_meshes[myType].meshPrefab, transform);
            myAnimal = _meshes[myType].animal.ToLower();
            myBoneType = _meshes[myType].boneType.ToLower();
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

        public static void ChangeMatchType(MatchType change)
        {
            mtype = change;
        }
        
        //when a match object is clicked, tells the grid that if the user tries to move a bone, it should be that one
        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
                MatchGrid.matchGrid.RegisterActiveMatchObj(this);
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                SecondaryViewManager.secondaryViewManager.SetView(_meshes[myType]);
            }
        }

        //compares itself to another matchobject per IComparable, obeying the matchtype specification
        public int CompareTo(MatchObject other)
        {
            if (mtype == MatchType.sameBone)
            {
                return String.Compare(myBoneType, other.myBoneType);
            }
            if (mtype == MatchType.sameSpecies)
            {
                return String.Compare(myAnimal, other.myAnimal);
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

        //returns the group data associated with this bone instance
        public MeshDataObj GetMyType()
        {
            return _meshes[myType];
        }
    }
}
