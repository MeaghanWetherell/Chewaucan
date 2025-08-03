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

    public static List<GetGlyph> listenerGlyphs;
    
    private static GlyphLibrary glyphLibrary;

    private static Dictionary<string, Sprite> _glyphDictionary;
    
    [Header("UI References")]
    public Image glyphImage;

    [Header("Default Key")]
    public string defaultKeyName = "w";

    private PlayerInput playerInput;

    private int index;

    private void Start()
    {
        UpdateGlyph();
    }

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

    public void UpdateGlyph()
    {
        if (actionToGet.action.controls.Count == 0) return;
        string keyName = actionToGet.action.bindings[index].effectivePath;

        if(keyName.Split("/").Length == 2)
            keyName = keyName.Split("/")[1];


        //Debug.Log($"Attempting to find glyph for key: '{keyName}' on action '{actionToRebind.name}'", this);

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
