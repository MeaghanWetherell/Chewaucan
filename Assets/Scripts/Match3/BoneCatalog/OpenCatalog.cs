using System.Collections;
using System.Collections.Generic;
using Match3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCatalog : MonoBehaviour
{
    [Tooltip("Button to go to level select")]
    public Button levelButton;

    [Tooltip("scroll under which to instantiate buttons")]
    public Transform scrollContent;

    [Tooltip("bone button prefab")] 
    public GameObject boneButton;

    //clean up the gui and repopulate when the user moves to the bone menu
    public void OnBoneButtonClicked()
    {
        levelButton.interactable = true;
        for (int i = 0; i < scrollContent.transform.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        populateGUI();
    }
    
    //add buttons to the GUI
    private void populateGUI()
    {
        List<MeshDataObj> bones = Resources.Load<MeshDataList>("Match3Meshes").meshes;
        bones.Sort();
        for (int i = 0; i < bones.Count; i++)
        {
            createButtonPrefab(bones[i]);
        }
    }

    //creates a button prefab with the details of the passed level
    private void createButtonPrefab(MeshDataObj bone)
    {
        GameObject newButton = Instantiate(boneButton, scrollContent, false);
        newButton.transform.GetChild(0).GetComponent<Image>().sprite = bone.flatImage;
        newButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = bone.boneName;
    }
    
    
}
