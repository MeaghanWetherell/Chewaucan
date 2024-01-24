using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    //opens a description of its quest in the gui on click
    public class QuestButton : MonoBehaviour
    {
        //the quest associated with this button
        public QuestNode node;

        private void OnEnable()
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            this.gameObject.GetComponent<Button>().onClick.RemoveListener(OnClick);
        }

        //when the button is clicked, bring up a more detailed description
        private void OnClick()
        {
            DescriptionBoxManager.descriptionBoxManager.SendNewNode(node);
        }
    }
}
