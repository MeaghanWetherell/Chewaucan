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
    [Tooltip("The image prefab this factory creates")]
    public GameObject draggableImagePrefab;

    [Tooltip("The image component on this factory")]
    public Image myImg;

    [Tooltip("The text object that should display what this word is in the alt language")]
    public TextMeshProUGUI altLangText;

    [Tooltip("The object with the button and audio source for playing the alt language word")]
    public GameObject altLangAudio;

    [Tooltip("The text object that should display what this word is in English")]
    public TextMeshProUGUI englishText;

    [Tooltip("The object with the button and audio source for playing the English word")]
    public GameObject engAudio;

    //what part of speech this word is
    [NonSerialized]public wordType myWordType;
    
    //the canvas this object is on
    [NonSerialized]public Canvas myCanvas;
    
    //whether this word has been unlocked
    [NonSerialized]public bool unlockState;
    
    [NonSerialized]public AudioClip wordClipAltLang;

    [NonSerialized]public String wordAltLang;

    [NonSerialized]public AudioClip wordClipEnglish;

    [NonSerialized]public String wordEnglish;

    //represents a part of speech
    public enum wordType
    {
        Noun,
        Verb,
        Adjective,
        Interrogative,
        Other
    }

    //intializes the factory based on the passed data object. canv should be the canvas the object was instantiated on, 
    //unlocked is whether the word should be unlocked already
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
    
    //set up should already have been called
    public void Start()
    {
        altLangText.text = GetAltLangText();
        englishText.text = GetEnglishText();
        altLangAudio.SetActive(unlockState);
        engAudio.SetActive(unlockState);
    }

    //returns the English text that should appear to the user
    public string GetEnglishText()
    {
        if (unlockState)
        {
            return "<style=\"LLUnlockedWord\">"+wordEnglish+"</style>";
        }
        return "<style=\"LLLockedWord\">??????</style>";
    }

    //returns the alt lang text that should appear to the user
    public string GetAltLangText()
    {
        if (unlockState)
        {
            return "<style=\"LLUnlockedWord\">" + wordAltLang + "</style>";
        }
        return "<style=\"LLLockedWord\">??????</style>";
    }

    //unlocks this word. should be called by the game manager. does not register the word unlocked with any other entity
    public void Unlock()
    {
        unlockState = true;
        altLangText.text = GetAltLangText();
        englishText.text = GetEnglishText();
        altLangAudio.SetActive(true);
        engAudio.SetActive(true);
    }

    //when clicked, create and set up a new draggable image based on this factory
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
