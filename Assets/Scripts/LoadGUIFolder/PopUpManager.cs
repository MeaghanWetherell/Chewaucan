using System;
using LoadGUIFolder;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    public class PopUpManager : MonoBehaviour
    {
        public TextMeshProUGUI titleActual;

        public TextMeshProUGUI mainTextActual;

        public UnityEvent<string> onClose;

        [NonSerialized]public string title;
        
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
