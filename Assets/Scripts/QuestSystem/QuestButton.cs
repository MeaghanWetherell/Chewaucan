using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestButton : MonoBehaviour
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
            DescriptionBoxManager.descriptionBoxManager.sendNewNode(node);
        }
    }
}
