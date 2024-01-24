using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    //pins and unpins an attached quest on click
    public class PinButton : MonoBehaviour
    {
        //the quest to be pinned
        public QuestNode node;

        private void OnEnable()
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            this.gameObject.GetComponent<Button>().onClick.RemoveListener(OnClick);
        }

        //pin or unpin the quest
        private void OnClick()
        {
            node.ChangePinned();
            if (node.isPinned)
            {
                gameObject.GetComponent<Image>().sprite = DescriptionBoxManager.descriptionBoxManager.check;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = DescriptionBoxManager.descriptionBoxManager.x;
            }
        }
    }
}
