using System;
using TMPro;
using UnityEngine;

namespace Narration.Journal
{
    //instantiates all the narration clips the player has unlocked in the journal
    public class LoadClips : MonoBehaviour
    {
        [Tooltip("Prefab for a single narration clip player")]
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

        //finds the correct resource folder that a narration can be found in
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
