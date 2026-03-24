using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using KeyRebinding;
using TMPro;

public class RebindWithGlyphs : MonoBehaviour
{
    [Header("Action to Rebind")]
    public InputAction actionToRebind;

    [Tooltip("index of that action to rebind")]
    public int index;
    [Header("Default Key")]
    public string defaultKeyName = "n";

    [Header("UI References")]
    public Image glyphImage;

    [Tooltip("Text component ref")] public TextMeshProUGUI mainText;
    
    //holds all key glyph images 
    private static GlyphLibrary glyphLibrary;

    //maps key names to glyph sprites
    private static Dictionary<string, Sprite> _glyphDictionary;
    
    private static InputActionRebindingExtensions.RebindingOperation _rebindOp;

    private void Awake()
    {
        if (glyphLibrary == null)
        {
            glyphLibrary = Resources.Load<GlyphLibrary>("MasterGlyphLibrary");
        }
        if(_glyphDictionary == null)
            _glyphDictionary = glyphLibrary.GetGlyphDictionary();
    }
    
    private void Start()
    {
        UpdateGlyph();
        
        BindingManager.bindingManager.bindChange.AddListener(UpdateGlyph);
    }

    //listen for user input. rebind to the next key they input
    public void StartListening()
    {
        actionToRebind.Disable();
        
        if(_rebindOp != null)
            _rebindOp.Cancel();

        _rebindOp = actionToRebind.PerformInteractiveRebinding(index).WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .WithControlsExcluding("<Mouse>/leftButton")
            .WithControlsExcluding("<Mouse>/rightButton")
            .WithControlsExcluding("<Mouse>/press")
            .WithControlsExcluding("<Keyboard>/anyKey")
            .WithControlsExcluding("<Pointer>/press")
            .WithControlsExcluding("<Mouse>/backButton")
            .WithControlsExcluding("<Mouse>/scroll")
            .WithControlsExcluding("<Mouse>/forwardButton")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(
                operation => { RebindComplete(); operation.Dispose();}).OnCancel(operation => {operation.Dispose();});

        _rebindOp.Start();
    }

    //when the user completes the rebind operation, notify the binding manager and update the key glyph
    private void RebindComplete()
    {
        _rebindOp = null;
        BindingManager.bindingManager.SetBind(actionToRebind, index);

        if (GetGlyph.listenerGlyphs != null)
        {
            foreach (GetGlyph glyph in GetGlyph.listenerGlyphs)
            {
                glyph.UpdateGlyph();
            }
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);
        actionToRebind.Enable();
    }

    //update the glyph used for this key
    public void UpdateGlyph()
    {
        string keyName = actionToRebind.bindings[index].effectivePath;

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