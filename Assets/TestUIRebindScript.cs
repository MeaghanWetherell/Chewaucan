using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

// No need to change the KeyGlyphMap struct if it's in its own file.
// If it was inside the old script, move it to the GlyphLibrary.cs file.

public class TestUIRebindScript : MonoBehaviour
{
    [Header("Action to Rebind")]
    public InputAction actionToRebind;
    [Header("Fallback")]
    public string defaultKeyName = "w";

    [Header("UI References")]
    public Image glyphImage;

    [Header("Shared Data")]
    // Instead of a list, we now just need a reference to our central library.
    public GlyphLibrary glyphLibrary;

    private Dictionary<string, Sprite> _glyphDictionary;
    private InputActionRebindingExtensions.RebindingOperation _rebindOp;

    private void Awake()
    {
        if (glyphLibrary == null)
        {
            Debug.LogError("Glyph Library is not assigned!", this);
            return;
        }
        // Get the dictionary from the central library.
        _glyphDictionary = glyphLibrary.GetGlyphDictionary();
    }

    // The rest of the script (Start, StartListening, UpdateGlyph) remains exactly the same.
    // ...
    private void Start()
    {
        Debug.Log($"Executing Start() for action: '{actionToRebind.name}'.", this);

        if (actionToRebind.controls.Count == 0)
        {
            Debug.LogWarning($"On Start, the action '{actionToRebind.name}' has NO controls. This is why the glyph isn't showing up.", this);
        }

        UpdateGlyph();
    }

    public void StartListening()
    {
        actionToRebind.Disable();

        // Save the current binding path
        string previousBindingPath = actionToRebind.bindings[0].effectivePath;

        _rebindOp = actionToRebind.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Keyboard>/escape")
            .WithControlsExcluding("<Gamepad>/start")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(op =>
            {
                string newBindingPath = actionToRebind.bindings[0].effectivePath;
                string controlName = actionToRebind.controls.Count > 0 ? actionToRebind.controls[0].name : "";

                bool isInvalid = newBindingPath == "<Mouse>/leftButton"
                                 || string.IsNullOrEmpty(controlName)
                                 || !_glyphDictionary.ContainsKey(controlName);

                if (isInvalid)
                {
                    Debug.LogWarning($"Rejected binding: '{newBindingPath}' (Invalid or no glyph). Restoring previous binding.", this);
                    actionToRebind.ApplyBindingOverride(0, previousBindingPath);
                }

                actionToRebind.Enable();
                op.Dispose();
                UpdateGlyph();
            })
            .OnCancel(op =>
            {
                actionToRebind.Enable();
                op.Dispose();
            });

        _rebindOp.Start();
    }



    
    private void UpdateGlyph()
    {
        string keyName = actionToRebind.controls.Count > 0
            ? actionToRebind.controls[0].name
            : defaultKeyName;

        Debug.Log($"Attempting to find glyph for key: '{keyName}' on action '{actionToRebind.name}'", this);

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