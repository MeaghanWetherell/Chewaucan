using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragTarget : MonoBehaviour
{
    public Image mainImg;

    public TextMeshProUGUI partOfSpeech;

    public TextMeshProUGUI altLangText;

    public TextMeshProUGUI englishText;

    public Image englishHighlight;

    public Image altLangHighlight;

    public ClipListPlayer player;
    
    [NonSerialized]public DraggableImageFactory parent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        DraggableImage img = other.GetComponent<DraggableImage>();
        if (img != null)
        {
            //Debug.Log("Trigger enter");
            img.myTarget = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        DraggableImage img = other.GetComponent<DraggableImage>();
        if (img != null && img.myTarget != null && img.myTarget.Equals(this))
        {
            //Debug.Log("Trigger exit, resetting");
            img.myTarget = null;
        } 
    }

    public void Highlight(bool isEnglish)
    {
        if (isEnglish)
        {
            englishHighlight.enabled = true;
        }
        else
        {
            altLangHighlight.enabled = true;
        }
    }
    
    public void Dehighlight(bool isEnglish)
    {
        if (isEnglish)
        {
            englishHighlight.enabled = false;
        }
        else
        {
            altLangHighlight.enabled = false;
        }
    }

    public void SetMyValue(DraggableImage img)
    {
        player.Stop();
        parent = img.parent;
        mainImg.sprite = parent.myImg.sprite;
        SetText();
        partOfSpeech.text = parent.myWordType.ToString();
    }

    public void SetText()
    {
        altLangText.text = parent.GetAltLangText();
        englishText.text = parent.GetEnglishText();
    }
}
