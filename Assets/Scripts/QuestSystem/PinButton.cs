using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class PinButton : MonoBehaviour
    {
        public QuestNode node;

        private void OnEnable()
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            this.gameObject.GetComponent<Button>().onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            node.changePinned();
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
