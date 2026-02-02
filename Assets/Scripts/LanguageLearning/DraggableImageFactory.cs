using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImageFactory : MonoBehaviour, IPointerDownHandler
{
    public GameObject draggableImagePrefab;

    public AudioClip wordClipAltLang;

    public String wordAltLang;

    public AudioClip wordClipEnglish;

    public String wordEnglish;

    public Image myImg;

    [NonSerialized]public bool unlockState;

    public TextMeshProUGUI altLangText;

    public GameObject altLangAudio;

    public TextMeshProUGUI englishText;

    public GameObject engAudio;

    public Canvas myCanvas;

    public wordType myWordType;


    public enum wordType
    {
        Noun,
        Verb,
        Adjective,
        Interrogative,
        Other
    }

    public void SetUp(bool unlocked, Canvas canv, DraggableImgData data)
    {
        unlockState = unlocked;
        myCanvas = canv;
        myImg.sprite = data.img;
        wordClipAltLang = data.altLangClip;
        altLangAudio.GetComponent<AudioSource>().clip = data.altLangClip;
        wordClipEnglish = data.englishClip;
        engAudio.GetComponent<AudioSource>().clip = data.englishClip;
        wordAltLang = data.altLangWord;
        wordEnglish = data.englishWord;
        myWordType = data.myWordType;
    }
    
    public void Start()
    {
        altLangText.text = GetAltLangText();
        englishText.text = GetEnglishText();
        altLangAudio.SetActive(unlockState);
        engAudio.SetActive(unlockState);
    }

    public string GetEnglishText()
    {
        if (unlockState)
        {
            return "<style=\"LLUnlockedWord\">"+wordEnglish+"</style>";
        }
        return "<style=\"LLLockedWord\">??????</style>";
    }

    public string GetAltLangText()
    {
        if (unlockState)
        {
            return "<style=\"LLUnlockedWord\">" + wordAltLang + "</style>";
        }
        return "<style=\"LLLockedWord\">??????</style>";
    }

    public void Unlock()
    {
        unlockState = true;
        altLangText.text = GetAltLangText();
        englishText.text = GetEnglishText();
        altLangAudio.SetActive(true);
        engAudio.SetActive(true);
    }

    public void OnClick()
    {
        if (DraggableImage.activeImage != null)
            return;
        GameObject newImg = Instantiate(draggableImagePrefab, myCanvas.transform);
        DraggableImage dImg = newImg.GetComponent<DraggableImage>();
        dImg.myImg.sprite = myImg.sprite;
        dImg.parent = this;
        dImg.myCanvas = myCanvas;
        newImg.transform.position = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick();
    }
}
