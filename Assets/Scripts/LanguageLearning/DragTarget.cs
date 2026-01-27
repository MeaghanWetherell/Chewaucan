using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragTarget : MonoBehaviour
{
    public Image mainImg;

    public TextMeshProUGUI altLangText;

    public TextMeshProUGUI englishText;
    
    [NonSerialized]public DraggableImageFactory parent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        DraggableImage img = other.GetComponent<DraggableImage>();
        if (img != null)
        {
            Debug.Log("Trigger enter");
            img.myTarget = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        DraggableImage img = other.GetComponent<DraggableImage>();
        if (img != null && img.myTarget != null && img.myTarget.Equals(this))
        {
            Debug.Log("Trigger exit, resetting");
            img.myTarget = null;
        } 
    }

    public void SetMyValue(DraggableImage img)
    {
        parent = img.parent;
        mainImg.sprite = parent.img;
        altLangText.text = parent.GetAltLangText();
        englishText.text = parent.GetEnglishText();
    }
}
