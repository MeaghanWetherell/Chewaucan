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

    // change this to true if you want the debug canvas to show up
    private const bool DEBUG_CANVAS_ENABLED = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(debugCanvas != null)
            debugCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        #pragma warning disable 0162 //diable unreachable code warning
        if (DEBUG_CANVAS_ENABLED)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                debugCanvas.SetActive(!debugCanvas.activeSelf);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        #pragma warning restore 0162
    }

    /**
     * 0 = Archeology
     * 1 = Geology
     * 2 = Biology
     * Completes a single quest. The astrolobe UI will update accordingly.
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
