using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GetGlyph : MonoBehaviour
{
    [Header("Action to Get Glyph for")]
    public InputActionReference actionToGet;
    
    [Tooltip("index of that action for control scheme: Keyboard and Mouse/Controller")]
    public List<int> indexList;

    //list of all glyphs actively listening for binding changes
    public static List<GetGlyph> listenerGlyphs;
    
    //reference to the library object that holds all glyph images
    private static GlyphLibrary glyphLibrary;

    //maps glyph names to sprites
    private static Dictionary<string, Sprite> _glyphDictionary;
    
    [Header("UI References")]
    public Image glyphImage;

    [Header("Default Key")]
    public string defaultKeyName = "w";

    private PlayerInput playerInput;

    //index of the binding this should get the glyph for
    private int index;

    private void Start()
    {
        UpdateGlyph();
    }

    //set up private variables add self as a listener
    private void OnEnable()
    {
        if (listenerGlyphs == null)
        {
            listenerGlyphs = new List<GetGlyph>();
        }
        
        if (glyphLibrary == null)
        {
            glyphLibrary = Resources.Load<GlyphLibrary>("MasterGlyphLibrary");
        }
        
        if(_glyphDictionary == null)
            _glyphDictionary = glyphLibrary.GetGlyphDictionary();

        if (playerInput == null)
        {
            playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        }

        if (playerInput.currentControlScheme.Equals("Keyboard & Mouse"))
            index = indexList[0];
        else
        {
            index = indexList[1];
        }
        
        listenerGlyphs.Add(this);
        UpdateGlyph();
    }

    private void OnDisable()
    {
        listenerGlyphs.Remove(this);
    }

    //find the correct glyph for the binding this corresponds to
    public void UpdateGlyph()
    {
        if (actionToGet.action.controls.Count == 0) return;
        string keyName = actionToGet.action.bindings[index].effectivePath;

        if(keyName.Split("/").Length == 2)
            keyName = keyName.Split("/")[1];

        if (_glyphDictionary.TryGetValue(keyName, out Sprite glyph))
        {
            glyphImage.sprite = glyph;
            glyphImage.enabled = true;
        }
        else if (!string.IsNullOrEmpty(defaultKeyName) && _glyphDictionary.TryGetValue(defaultKeyName, out Sprite fallbackGlyph))
        {
            Debug.LogWarning($"No glyph found for key: '{keyName}'. Falling back to default key: '{defaultKeyName}'.", this);
            glyphImage.sprite = fallbackGlyph;
            glyphImage.enabled = true;
        }
        else
        {
            Debug.LogWarning($"No valid glyph found for either '{keyName}' or default '{defaultKeyName}'. Hiding glyph.");
            glyphImage.enabled = false;
        }
    }
}
