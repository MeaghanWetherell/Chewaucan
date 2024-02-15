using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

public class AstrolabeUI : LoadGUI
{
    public static AstrolabeUI astrolabeUI;

    private void Awake()
    {
        if (astrolabeUI != null)
        {
            Debug.LogError("Loaded persistent objects twice!");
            Destroy(astrolabeUI.gameObject);
        }
        astrolabeUI = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
