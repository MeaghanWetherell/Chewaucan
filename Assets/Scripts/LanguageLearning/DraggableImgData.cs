using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DraggableImgData : ScriptableObject
{
    public string englishWord;
    public string altLangWord;
    public AudioClip altLangClip;
    public AudioClip englishClip;
    [Tooltip("Icon that represents this word")]
    public Sprite img;
    [Tooltip("The English part of speech that this word is")]
    public DraggableImageFactory.wordType myWordType;

}
