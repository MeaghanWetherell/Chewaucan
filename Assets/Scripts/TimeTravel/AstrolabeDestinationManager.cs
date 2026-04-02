using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Misc;
using QuestSystem.Quests.QScripts;
using UnityEngine;
using UnityEngine.UI;

namespace TimeTravel
{
    public class AstrolabeDestinationManager : MonoBehaviour
    {
        public Button PastTeleportButton;
        
        private Vector3 Teleposition1 = Vector3.negativeInfinity;
        
        private Vector3 Teleposition2 = Vector3.negativeInfinity;
        
        private void Awake()
        {
            Teleposition1 = AstrolabeQueueManager.peekModern();
            Teleposition2 = AstrolabeQueueManager.peekPleist();
            int curScene = SceneLoadWrapper.sceneLoadWrapper.currentSceneType;
            if (Teleposition2.Equals(Vector3.negativeInfinity) && curScene == 0)
            {
                PastTeleportButton.interactable = false;
            }
            else if (Teleposition1.Equals(Vector3.negativeInfinity) && curScene == 1)
            {
                PastTeleportButton.interactable = false;
            }
        }

        public void OnClick()
        {
            int curScene = SceneLoadWrapper.sceneLoadWrapper.currentSceneType;
            if (curScene == 0)
            {
                SceneLoadWrapper.sceneLoadWrapper.LoadScene("PleistoceneMap");
            }
            else
            {
                SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
            }
        }
    }
}
