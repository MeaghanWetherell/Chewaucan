using System;
using LoadGUIFolder;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    public class PopUpManager : MonoBehaviour
    {
        [Tooltip("Pop-Up title")]
        public TextMeshProUGUI titleActual;

        [Tooltip("Pop-Up body")]
        public TextMeshProUGUI mainTextActual;

        [Tooltip("Runs when the pop-up closes")]
        public UnityEvent<string> onClose;

        [NonSerialized]public string title;
        
        //index of this pop-up in the LoadGUIManager
        [NonSerialized] public int index;
        
        public virtual void SetText(string inTitle, string mainText)
        {
            title = inTitle;
            if(titleActual != null)
                titleActual.text = title;
            if(mainTextActual != null)
                mainTextActual.text = mainText;
        }
        public virtual void OnClick()
        {
            onClose.Invoke(title);
            LoadGUIManager.loadGUIManager.ClosePopUp(index);
        }
    }
}
