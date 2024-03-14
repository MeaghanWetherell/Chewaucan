using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/** To be attached to the player prefab
 * used to debug and add to completion of quests
 */
public class ProgressTracker : MonoBehaviour
{
    public GameObject debugCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        debugCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            debugCanvas.SetActive(!debugCanvas.activeSelf);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /**
     * 0 = Archeology
     * 1 = Geology
     * 2 = Biology
     * Completes a single quest. That is, it moves the 
     */
    public void CompleteQuest(int n)
    {
        Debug.Log("COMPLETE");
        if (n == 0)
        {
            SaveDialProgressData.CompleteOneQuest(SaveDialProgressData.Dial.ARCHEOLOGY);
        }
        else if (n == 1)
        {
            SaveDialProgressData.CompleteOneQuest(SaveDialProgressData.Dial.GEOLOGY);
        }
        else if (n == 2)
        {
            SaveDialProgressData.CompleteOneQuest(SaveDialProgressData.Dial.BIOLOGY);
        }
    }

    //only used for debug purposes
    public void DeleteProgress()
    {
        Debug.Log("Delete");
        SaveDialProgressData.DeleteDialProgress();
    }
}
