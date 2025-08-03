using System.IO;
using System.Text.Json;
using UnityEngine;

namespace QuestSystem.Quests.QScripts
{
    public class SubAstrolabeTeleport : MonoBehaviour
    {
        [Tooltip("the position the player should teleport to on completion of the associated quest")]
        public Vector3 playerPosition;

        [Tooltip("The quest to sub to")]
        public string subToId;

        private void Start()
        {
            QuestManager.questManager.SubToCompletion(subToId, toSub =>
            {
                v3Wrapper toSerialize = new v3Wrapper(playerPosition);
                string json = JsonSerializer.Serialize(toSerialize);
                string savePath = SaveHandler.saveHandler.getSavePath();
                File.WriteAllText(savePath+"/astrolabeteleposition.json", json);
            });
        }
    }
}
