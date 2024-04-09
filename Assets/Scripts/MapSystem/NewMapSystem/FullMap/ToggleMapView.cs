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
        modernMapView.SetActive(true);
        pleistoceceMapView.SetActive(false);
        mapName.text = "Modern Map";
    }

    public void SwitchMapView()
    {
        if (modernMapView.activeInHierarchy)
        {
            modernMapView.SetActive(false);
            pleistoceceMapView.SetActive(true);
            mapName.text = "Pleistocene Map";
        }
        else if (pleistoceceMapView.activeInHierarchy)
        {
            modernMapView.SetActive(true);
            pleistoceceMapView.SetActive(false);
            mapName.text = "Modern Map";
        }
        if (teleportSidebarAnimator.GetBool("active") == true)
        {
            teleportSidebarAnimator.SetBool("active", false);
        }
    }
}
