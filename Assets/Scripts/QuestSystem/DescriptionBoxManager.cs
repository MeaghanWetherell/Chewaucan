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
        private QuestNode cur;

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
        public void sendNewNode(QuestNode newNode)
        {
            cur = newNode;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cur.name;
            if (cur.longDescription.Equals(""))
            {
                transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cur.shortDescription;
            }
            else
            {
                transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cur.longDescription;
            }
            TextMeshProUGUI text = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = "";
            for (int i = 0; i < newNode.objectives.Count; i++)
            {
                text.text += +newNode.counts[i] + "/" + newNode.requiredCounts[i] + " " + newNode.objectives[i]+"\n";
            }
            GameObject pinObj = transform.GetChild(4).gameObject;
            if (cur.isComplete)
            {
                transform.GetChild(3).gameObject.SetActive(true);
                pinObj.SetActive(false);
            }
            else
            {
                transform.GetChild(3).gameObject.SetActive(false);
                pinObj.SetActive(true);
                Image pin = pinObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                if (cur.isPinned)
                {
                    pin.sprite = check;
                }
                else
                {
                    pin.sprite = x;
                }
                pin.GetComponent<PinButton>().node = cur;
            }
        }
    }
}
