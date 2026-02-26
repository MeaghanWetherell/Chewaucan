using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    //handles presenting information in the description panel of the quest menu
    public class DescriptionBoxManager : MonoBehaviour
    {
        //current quest being described
        private QuestNode _cur;

        [Tooltip("Checkmark sprite")]
        public Sprite check;

        [Tooltip("X sprite")]
        public Sprite x;
        
        public static DescriptionBoxManager descriptionBoxManager;

        private void Awake()
        {
            descriptionBoxManager = this;
        }

        //tells the description box to reset itself to describe the passed node
        public void SendNewNode(QuestNode newNode)
        {
            _cur = newNode;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _cur.name;
            TextMeshProUGUI longDescTMP = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (_cur.longDescription.Equals(""))
            {
                longDescTMP.text = _cur.shortDescription;
            }
            else
            {
                longDescTMP.text = _cur.longDescription;
            }

            for (int i = 0; i < newNode.updateUnlocks.Count; i++)
            {
                if (newNode.updateUnlocks[i])
                {
                    longDescTMP.text += newNode.qUpdates[i];
                }
            }

            if (newNode.isComplete)
            {
                longDescTMP.text += newNode.compText;
            }
            TextMeshProUGUI text = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = "";
            for (int i = 0; i < newNode.objectives.Count; i++)
            {
                text.text += +newNode.counts[i] + "/" + newNode.requiredCounts[i] + " " + newNode.objectives[i]+"\n";
            }
            GameObject pinObj = transform.GetChild(4).gameObject;
            if (_cur.isComplete)
            {
                transform.GetChild(3).gameObject.SetActive(true);
                pinObj.SetActive(false);
            }
            else
            {
                transform.GetChild(3).gameObject.SetActive(false);
                pinObj.SetActive(true);
                Image pin = pinObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                if (_cur.isPinned)
                {
                    pin.sprite = check;
                }
                else
                {
                    pin.sprite = x;
                }
                pin.GetComponent<PinButton>().node = _cur;
            }
            /*
            TextMeshProUGUI temp = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
            switch (newNode.type)
            {
                case SaveDialProgressData.Dial.NONE:
                    temp.text = "";
                    break;
                case SaveDialProgressData.Dial.ARCHEOLOGY:
                    temp.text = "Type: Archaeology";
                    break;
                case SaveDialProgressData.Dial.BIOLOGY:
                    temp.text = "Type: Biology";
                    break;
                case SaveDialProgressData.Dial.GEOLOGY:
                    temp.text = "Type: Geology";
                    break;
            }*/
        }
    }
}
