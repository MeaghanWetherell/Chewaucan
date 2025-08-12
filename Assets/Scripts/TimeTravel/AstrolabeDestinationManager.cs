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
        
        private Vector3 Teleposition = Vector3.negativeInfinity;
        private void Awake()
        {
            try
            {
                string savePath = SaveHandler.saveHandler.getSavePath();
                v3Wrapper temp = JsonSerializer.Deserialize<v3Wrapper>(File.ReadAllText(savePath+"/astrolabeteleposition.json"));
                Teleposition = temp.getVector();
            }
            catch (IOException){ }

            if (Teleposition.Equals(Vector3.negativeInfinity))
            {
                PastTeleportButton.interactable = false;
            }
        }

        public void OnClick()
        {
            int curScene = SceneLoadWrapper.sceneLoadWrapper.currentSceneType;
            if (curScene == 0)
            {
                PlayerPositionManager.playerPositionManager.setPlayerPosition(Teleposition, 1);
                SceneLoadWrapper.sceneLoadWrapper.LoadScene("PleistoceneMap");
            }
            else
            {
                PlayerPositionManager.playerPositionManager.setPlayerPosition(Teleposition, 0);
                SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
            }
        }
    }
}
