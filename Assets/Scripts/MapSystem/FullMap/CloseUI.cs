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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        animator.SetBool("active", false);
    }
}
