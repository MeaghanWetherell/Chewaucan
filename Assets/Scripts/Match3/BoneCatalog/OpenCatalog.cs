using System.Collections.Generic;
using Match3.DataClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class OpenCatalog : MonoBehaviour
    {
        [Tooltip("Button to go to level select")]
        public Button levelButton;

        [Tooltip("Scroll under which to instantiate buttons")]
        public Transform scrollContent;

        [Tooltip("Bone button prefab")] 
        public GameObject boneButton;

        private void Awake()
        {
            if (OpenLevelSelect.openLevelSelect.shouldLoadBone)
            {
                levelButton.interactable = true;
                this.GetComponent<Button>().interactable = false;
                PopulateGUI();
            }
        }

        //clean up the gui and repopulate when the user moves to the bone menu
        public void OnBoneButtonClicked()
        {
            levelButton.interactable = true;
            this.GetComponent<Button>().interactable = false;
            for (int i = 0; i < scrollContent.transform.childCount; i++)
            {
                Destroy(scrollContent.GetChild(i).gameObject);
            }
            PopulateGUI();
        }
    
        //add buttons to the GUI
        private void PopulateGUI()
        {
            GameObject.Find("HighScoresBG").SetActive(false);
            List<MeshDataObj> bones = Resources.Load<MeshDataList>("Meshes/Match3Meshes").meshes;
            List<MeshDataObj> bonesActual = new List<MeshDataObj>();
            foreach(MeshDataObj mesh in bones)
            {
                bonesActual.Add(mesh);
            }
            bonesActual.Sort();
            for (int i = 0; i < bonesActual.Count; i++)
            {
                CreateButtonPrefab(bonesActual[i]);
            }
        }

        //creates a button prefab with the details of the passed level
        private void CreateButtonPrefab(MeshDataObj bone)
        {
            GameObject newButton = Instantiate(boneButton, scrollContent, false);
            newButton.GetComponent<BoneButton>().data = bone;
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = bone.flatImage;
            newButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = bone.boneName;
        }
    
    
    }
}
