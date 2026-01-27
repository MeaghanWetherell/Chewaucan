using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImageFactory : MonoBehaviour, IPointerDownHandler
{
    public GameObject draggableImagePrefab;

    public AudioClip wordClipAltLang;

    public String wordAltLang;

    public AudioClip wordClipEnglish;

    public String wordEnglish;

    public Sprite img;

    [NonSerialized]public bool unlockState;

    public TextMeshProUGUI altLangText;

    public TextMeshProUGUI englishText;

    public Canvas myCanvas;
    
    public void Start()
    {
        altLangText.text = GetAltLangText();
        englishText.text = GetEnglishText();
    }

    public string GetEnglishText()
    {
        if (unlockState)
        {
            return "<style=\"LLUnlockedWord\">"+wordEnglish+"</style>";
        }
        else
        {
            return "<style=\"LLLockedWord\">??????</style>";
        }
    }

    public string GetAltLangText()
    {
        if (unlockState)
        {
            return "<style=\"LLUnlockedWord\">" + wordAltLang + "</style>";
        }
        else
        {
            return "<style=\"LLLockedWord\">??????</style>";
        }
    }

    public void Unlock()
    {
        unlockState = true;
        altLangText.text = GetAltLangText();
        englishText.text = GetEnglishText();
    }

    public void OnClick()
    {
        if (DraggableImage.activeImage != null)
            return;
        GameObject newImg = Instantiate(draggableImagePrefab, myCanvas.transform);
        DraggableImage dImg = newImg.GetComponent<DraggableImage>();
        dImg.myImg.sprite = img;
        dImg.parent = this;
        dImg.myCanvas = myCanvas;
        newImg.transform.position = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick();
    }
}
