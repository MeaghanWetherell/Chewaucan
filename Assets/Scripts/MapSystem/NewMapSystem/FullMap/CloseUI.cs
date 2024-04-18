using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    [SerializeField] private GameObject sidebarUI;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = sidebarUI.GetComponent<Animator>();
    }

    // animates the UI out when the X is pressed
    public void CloseSideMenu()
    {
        animator.SetBool("active", false);
    }
}
