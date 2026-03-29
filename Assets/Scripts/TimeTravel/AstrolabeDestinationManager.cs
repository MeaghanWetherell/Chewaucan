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

        //send map = 1 for modern, map = 2 for pleistocene
        public static void SetDestination(Vector3 dest, int map = 2)
        {
            v3Wrapper toSerialize = new v3Wrapper(dest);
            string json = JsonSerializer.Serialize(toSerialize);
            string savePath = SaveHandler.saveHandler.getSavePath();
            File.WriteAllText(savePath+"/astrolabeteleposition"+map+".json", json);
            AstrolabeUIIconManager.SetNewDest(true, map-1);
        }
        
        private void Awake()
        {
            Teleposition1 = GetTeleposition();
            Teleposition2 = GetTeleposition(2);
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

        //1 for modern 2 for pleistocene
        public static Vector3 GetTeleposition(int map = 1)
        {
            try
            {
                string savePath = SaveHandler.saveHandler.getSavePath();
                Debug.Log(savePath + "/astrolabeteleposition" + map + ".json");
                v3Wrapper temp =
                    JsonSerializer.Deserialize<v3Wrapper>(
                        File.ReadAllText(savePath + "/astrolabeteleposition" + map + ".json"));
                return temp.getVector();
            }
            catch (IOException)
            {
                return Vector3.negativeInfinity;
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
