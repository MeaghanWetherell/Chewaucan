using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevertAfterTime : MonoBehaviour
{
    public TextMeshProUGUI revertText;

    public float timeToRevert = 20;

    public Button closeButton;

    private void Update()
    {
        timeToRevert -= Time.deltaTime;
        int displayTime = Mathf.CeilToInt(timeToRevert);
        revertText.text = "Keep applied graphics settings? Changes will revert in " + displayTime + " seconds.";
        if (displayTime <= 0)
        {
            closeButton.onClick.Invoke();
        }
    }
}
