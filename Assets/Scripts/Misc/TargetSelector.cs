using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    // Start is called before the first frame update
    // script from: https://www.ketra-games.com/2020/11/unity-game-tutorial-selecting-a-target-with-the-mouse.html
    //public Camera camera;
    //public LayerMask Default;
    
    void Start() {
       
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, 10)) {
                if (hit.collider.gameObject.GetComponent<Target>() != null) {
                      //print("you hit me");
                }
            }
        }
    }
}
