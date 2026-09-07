using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AstrolabeUIIconManager : MonoBehaviour
{
    private static bool newDestinationModern;

    private static bool newDestinationPleist;

    private int curMap;

    public static AstrolabeUIIconManager manager;

    public Image myImage;

    public Sprite defaultSprite;

    public Sprite newThingSprite;

    private void OnEnable()
    {
        if (manager != null)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(WaitToInitialize());
        manager = this;
    }

    //wait a frame to initialize so player position manager can finish
    private IEnumerator WaitToInitialize()
    {
        yield return null;
        if (SceneManager.GetActiveScene().name.Equals("PleistoceneMap"))
        {
            SetNewDest(false, 1); //Turn off the astrolabe when we return to the Modern; you've completed this pleistocene spot
            SetNewDest(newDestinationModern, 0); //new destination to return to the modern.
            curMap = 1;
        }
        else 
        {
            SetNewDest(false, 0); //no new destination in the modern map
            SetNewDest(newDestinationPleist, 1); //new destination in the pleistocene
            curMap = 0;
        }
    }
    

    //0 for modern 1 for pleistocene
    public static void SetNewDest(bool set, int map)
    {
        if (map == 0)
        {
            newDestinationModern = set;
        }
        else
        {
            newDestinationPleist = set;
        }
        if (manager != null && map != manager.curMap && set)
        {
            manager.myImage.sprite = manager.newThingSprite;
        }
        else if(manager != null)
        {
            manager.myImage.sprite = manager.defaultSprite;
        }
    }

    public static bool GetNewDest(int map = 0)
    {
        if (map == 0)
            return newDestinationModern;
        return newDestinationPleist;
    }
}
