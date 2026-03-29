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

    public void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("PleistoceneMap"))
        {
            SetNewDest(false, 1);
            SetNewDest(newDestinationModern, 0);
            curMap = 1;
        }
        else 
        {
            SetNewDest(false, 0);
            SetNewDest(newDestinationPleist, 1);
            curMap = 0;
        }
        if(manager != null)
            Destroy(gameObject);
        manager = this;
    }

    //0 for modern 1 for pleistocene
    public void SetNewDest(bool set, int map)
    {
        if (map == 0)
        {
            newDestinationModern = set;
        }
        else
        {
            newDestinationPleist = set;
        }
        if (map != curMap && set)
        {
            myImage.sprite = newThingSprite;
        }
        else
        {
            myImage.sprite = defaultSprite;
        }
    }

    public static bool GetNewDest(int map = 0)
    {
        if (map == 0)
            return newDestinationModern;
        return newDestinationPleist;
    }
}
