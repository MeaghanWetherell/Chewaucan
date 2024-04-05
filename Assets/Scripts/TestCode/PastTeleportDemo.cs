using Misc;
using QuestSystem;
using UnityEngine;

namespace TestCode
{
    public class PastTeleportDemo : MonoBehaviour
    {
        public void OnClick()
        {
            QuestNode node = QuestManager.questManager.GETNode("matchdemo");
            if (node is {isComplete: true})
            {
                if (SceneLoadWrapper.sceneLoadWrapper.currentSceneType == 0)
                {
                    SceneLoadWrapper.sceneLoadWrapper.LoadScene("PleistoceneMap");
                }
                else
                {
                    SceneLoadWrapper.sceneLoadWrapper.LoadScene("ModernMap");
                }
            }
        }
    }
}
