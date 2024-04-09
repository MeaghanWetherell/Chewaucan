using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// switches between viewing the modern map and the pleistocene map images
public class ToggleMapView : MonoBehaviour
{
    [SerializeField] private GameObject modernMapView;
    [SerializeField] private GameObject pleistoceceMapView;

    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private Animator teleportSidebarAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // by default, we view the modern map
        //      might change this later to view the map of the scene we were in
        modernMapView.SetActive(true);
        pleistoceceMapView.SetActive(false);
        mapName.text = "Modern Map";
    }

    // enables/disables game objects so we view the map we are currently not seeing
    public void SwitchMapView()
    {
        if (modernMapView.activeInHierarchy) // switch to view the pleistocene map
        {
            modernMapView.SetActive(false);
            pleistoceceMapView.SetActive(true);
            mapName.text = "Pleistocene Map";
        }
        else if (pleistoceceMapView.activeInHierarchy) // switch to view the modern map
        {
            modernMapView.SetActive(true);
            pleistoceceMapView.SetActive(false);
            mapName.text = "Modern Map";
        }

        // if the teleport menu is open, close it
        if (teleportSidebarAnimator.GetBool("active") == true)
        {
            teleportSidebarAnimator.SetBool("active", false);
        }
    }
}
