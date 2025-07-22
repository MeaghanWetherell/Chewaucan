using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using KeyRebinding;
using TMPro;

// No need to change the KeyGlyphMap struct if it's in its own file.
// If it was inside the old script, move it to the GlyphLibrary.cs file.

public class RebindWithGlyphs : MonoBehaviour
{
    [Header("Action to Rebind")]
    public InputAction actionToRebind;

    [Tooltip("index of that action to rebind")]
    public int index;
    [Header("Default Key")]
    public string defaultKeyName = "w";

    [Header("UI References")]
    public Image glyphImage;

    [Tooltip("Text component ref")] public TextMeshProUGUI mainText;
    
    private static GlyphLibrary glyphLibrary;

    private Dictionary<string, Sprite> _glyphDictionary;
    private InputActionRebindingExtensions.RebindingOperation _rebindOp;

    private void Awake()
    {
        glyphLibrary = Resources.Load<GlyphLibrary>("MasterGlyphLibrary");
        
        if (glyphLibrary == null)
        {
            Debug.LogError("Glyph Library is not assigned!", this);
            return;
        }
        _glyphDictionary = glyphLibrary.GetGlyphDictionary();
    }
    
    private void Start()
    {

        if (actionToRebind.controls.Count == 0)
        {
            Debug.LogWarning($"On Start, the action '{actionToRebind.name}' has NO controls. This is why the glyph isn't showing up.", this);
        }

        UpdateGlyph();
        
        BindingManager.bindingManager.bindChange.AddListener(UpdateGlyph);
    }

    public void StartListening()
    {
        actionToRebind.Disable();

        _rebindOp = actionToRebind.PerformInteractiveRebinding(index).WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta").WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape").WithControlsExcluding("<Mouse>/leftButton").OnMatchWaitForAnother(0.1f)
            .OnComplete(
                operation => { RebindComplete(); operation.Dispose();}).OnCancel(operation => {operation.Dispose();});

        _rebindOp.Start();
    }

    private void RebindComplete()
    {
        BindingManager.bindingManager.SetBind(actionToRebind, index);
            
        actionToRebind.Enable();
    }

    
    public void UpdateGlyph()
    {
        if (actionToRebind.controls.Count == 0) return;
        string keyName = actionToRebind.bindings[index].effectivePath;

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