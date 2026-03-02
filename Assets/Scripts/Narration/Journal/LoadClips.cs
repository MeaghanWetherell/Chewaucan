using System;
using TMPro;
using UnityEngine;

namespace Narration.Journal
{
    public class LoadClips : MonoBehaviour
    {
        public GameObject playerPrefab;
        void Start()
        {
            foreach (string id in NarrationManager.narrationManager.hasRun)
            {
                Narration narr = Resources.Load<Narration>("Objs/"+GetFolder(id) +"/"+ id);
                GameObject fab = Instantiate(playerPrefab, transform);
                fab.GetComponent<TextMeshProUGUI>().text = narr.name;
                fab.GetComponentInChildren<ClipPlayer>().clip = narr;
            }
        }

        private string GetFolder(string id)
        {
            string folder = "";
            foreach (char c in id)
            {
                if (Char.IsLetter(c))
                    folder += c;
                else
                {
                    break;
                }
            }

            return folder;
        }
    }
}
