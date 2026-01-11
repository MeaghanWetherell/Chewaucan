using System;
using TMPro;
using UnityEngine;

namespace Misc
{
    public class PopUpTextManager : MonoBehaviour
    {
        public TextMeshProUGUI titleActual;

        public TextMeshProUGUI mainTextActual;

        [NonSerialized]public string title;

        public void SetText(string inTitle, string mainText)
        {
            title = inTitle;
            if(titleActual != null)
                titleActual.text = title;
            if(mainTextActual != null)
                mainTextActual.text = mainText;
        }
    }
}
