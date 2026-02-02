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
    public Sprite img;
    public DraggableImageFactory.wordType myWordType;

}
