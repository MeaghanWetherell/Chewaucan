using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragTarget : MonoBehaviour
{
    [Tooltip("The image component of the target")]
    public Image mainImg;

    [Tooltip("Text object to display the part of speech the attached word is")]
    public TextMeshProUGUI partOfSpeech;

    [Tooltip("Text object to display the alt language text of the word")]
    public TextMeshProUGUI altLangText;

    [Tooltip("Text object to display the English text of the word")]
    public TextMeshProUGUI englishText;

    [Tooltip("Image used to highlight the English word")]
    public Image englishHighlight;

    [Tooltip("Image used to highlight the alt language word")]
    public Image altLangHighlight;

    public ClipListPlayer player;
    
    //ref to the draggable factory for the image the target currently displays
    [NonSerialized]public DraggableImageFactory parent;
    
    //when we collide with a draggable image, let it know
    private void OnTriggerEnter2D(Collider2D other)
    {
        DraggableImage img = other.GetComponent<DraggableImage>();
        if (img != null)
        {
            img.myTarget = this;
        }
    }

    //when the draggable leaves, let it know
    private void OnTriggerExit2D(Collider2D other)
    {
        DraggableImage img = other.GetComponent<DraggableImage>();
        if (img != null && img.myTarget != null && img.myTarget.Equals(this))
        {
            img.myTarget = null;
        } 
    }

    //highlight the word of the appropriate language on this target
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
    
    //dehighlight the word of the appropriate language on this target
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

    //stop the clips playing and set the target up based on the passed image
    public void SetMyValue(DraggableImage img)
    {
        player.Stop();
        parent = img.parent;
        mainImg.sprite = parent.myImg.sprite;
        SetText();
        partOfSpeech.text = parent.myWordType.ToString();
    }

    //set the text object values based on the factory this object is attached to
    public void SetText()
    {
        altLangText.text = parent.GetAltLangText();
        englishText.text = parent.GetEnglishText();
    }
}
