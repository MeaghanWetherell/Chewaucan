using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class DescriptionBoxManager : MonoBehaviour
    {
        private QuestNode cur;

        public Sprite check;

        public Sprite x;

        public static DescriptionBoxManager descriptionBoxManager;

        private void Awake()
        {
            descriptionBoxManager = this;
        }

        public void sendNewNode(QuestNode newNode)
        {
            cur = newNode;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cur.name;
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cur.longDescription;
            transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = cur.count+"/"+cur.requiredCount+" "+cur.objective;
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
