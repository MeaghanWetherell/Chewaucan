using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GlyphLibrary", menuName = "Rebinding/Glyph Library", order = 1)]
public class GlyphLibrary : ScriptableObject
{
    [System.Serializable]
    public struct KeyGlyphMap
    {
        public string keyName;
        public Sprite glyphSprite;
    }

    [Tooltip("All key glyphs and their corresponding button")]
    public List<KeyGlyphMap> glyphs;

    //Internal glyph dict.
    [NonSerialized]private Dictionary<string, Sprite> _glyphDictionary;

    public Dictionary<string, Sprite> GetGlyphDictionary()
    {
        if (_glyphDictionary == null)
        {
            _glyphDictionary = new Dictionary<string, Sprite>(System.StringComparer.InvariantCultureIgnoreCase);
            foreach (var map in glyphs)
            {
                if (!string.IsNullOrEmpty(map.keyName) && map.glyphSprite != null)
                {
                    _glyphDictionary[map.keyName] = map.glyphSprite;
                }
            }
        }
        return _glyphDictionary;
    }
}