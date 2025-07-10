using UnityEngine;
using System.Collections.Generic;

// This attribute allows you to create an instance of this object from the Assets menu.
[CreateAssetMenu(fileName = "GlyphLibrary", menuName = "Rebinding/Glyph Library", order = 1)]
public class GlyphLibrary : ScriptableObject
{
    // Use the same helper struct we defined before.
    [System.Serializable]
    public struct KeyGlyphMap
    {
        public string keyName;
        public Sprite glyphSprite;
    }

    // This is the single, central list of all your glyphs.
    public List<KeyGlyphMap> glyphs;

    // We can also create the lookup dictionary here to be used by other scripts.
    private Dictionary<string, Sprite> _glyphDictionary;

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