using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* current issue is that the mouseover works great, but I don't want infinite mouseover distances!
 * https://gamedevbeginner.com/input-in-unity-made-easy-complete-guide-to-the-new-system/#input_system_get_mouse_position
 * this helped me get the mouse position but I don't have the new input system. 
 * If I ever GET the new input system then I will need to update the "input.mouseposition" to be  
 * Mouse.current.position.ReadValue() with Unity's input system called at the top
 * https://gamedevbeginner.com/how-to-convert-the-mouse-position-to-world-space-in-unity-2d-3d/
 * this one might be better tbh
 
 */



public class InteractableRenderer : MonoBehaviour
{
    //renderer is the color we add when its moused over
    private Renderer rendererRed;
    Vector3 worldPosition;
    Plane plane = new Plane(Vector3.up, 0);


    void Start()
    {
        //when we start, make renderer the render of the object tied to this script
        rendererRed = GetComponent<Renderer>();

    }

    public void OnMouseEnter()
    {
        //when mouse enters that thing, turn it a color
        //red and green are defailt in here
        //https://docs.unity3d.com/ScriptReference/SpriteRenderer-color.html might help with other colors
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        //This works! If within 10, we can highlight the target.
        if (Physics.Raycast(ray, out hitData, 10))
        {
            worldPosition = hitData.point;
            rendererRed.material.color = Color.red;
            print(worldPosition);
        }
    }

    private void OnMouseExit()
    {
        //when mouse exits, turn that color highlight off. White is default, no overlay.
        rendererRed.material.color = Color.white;
    }
}


