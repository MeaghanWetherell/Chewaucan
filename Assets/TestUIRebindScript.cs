using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using KeyRebinding;

public class TestUIRebindScript : MonoBehaviour
{
    public InputAction actionToRebind;
    public Image glyphImage;

    // Dictionary to store sliced sprites for each key (You could also use an array or List)
    public Sprite[] keyGlyphs;  // Store the sliced glyph sprites here, in order (A, B, C, etc.)

    private InputActionRebindingExtensions.RebindingOperation rebindOp;

    public void StartListening()
    {
        actionToRebind.Disable();

        rebindOp = actionToRebind.PerformInteractiveRebinding(0)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(op =>
            {
                actionToRebind.Enable();

                // Save and apply binding using your existing manager
                BindingManager.bindingManager.SetBind(actionToRebind, 0);

                // Get the key pressed and map it to the correct glyph index
                string keyName = actionToRebind.bindings[0].ToDisplayString().Replace(" ", "");
                int keyIndex = GetGlyphIndexForKey(keyName);  // You need to implement this method

                // Set the correct glyph
                if (keyIndex >= 0 && keyIndex < keyGlyphs.Length)
                    glyphImage.sprite = keyGlyphs[keyIndex];
                else
                    Debug.LogWarning($"Glyph not found for key: {keyName}");

                op.Dispose();
            });

        rebindOp.Start();
    }

    // Example method to map key names (or codes) to the correct index in the sprite array
    private int GetGlyphIndexForKey(string keyName)
    {
        // This is just a simple example. Adjust it according to how your keys are named or mapped.
        switch (keyName)
        {
            case "A": return 0;
            case "B": return 1;
            case "C": return 2;
            // Add cases for other keys...
            default: return -1;  // Return -1 if no matching key is found
        }
    }
}
