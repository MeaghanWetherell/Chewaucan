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
            try
            {
                string savePath = SaveHandler.saveHandler.getSavePath();
                v3Wrapper temp = JsonSerializer.Deserialize<v3Wrapper>(File.ReadAllText(savePath+"/astrolabeteleposition1.json"));
                Teleposition1 = temp.getVector();
            }
            catch (IOException){ }
            try
            {
                string savePath = SaveHandler.saveHandler.getSavePath();
                v3Wrapper temp = JsonSerializer.Deserialize<v3Wrapper>(File.ReadAllText(savePath+"/astrolabeteleposition2.json"));
                Teleposition2 = temp.getVector();
            }
            catch (IOException){ }
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
                PlayerPositionManager.playerPositionManager.setPlayerPosition(Teleposition2, 1);
                SceneLoadWrapper.sceneLoadWrapper.LoadScene("PleistoceneMap");
            }
            else
            {
                PlayerPositionManager.playerPositionManager.setPlayerPosition(Teleposition1, 0);
                SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
            }
        }
    }
}
