using QuestSystem;
using UnityEngine;

//Attach to an object present in the Modern Map scene.
//Once the assigned QuestCompletionGetter reports both required quests complete,
//this queues a teleport back to the player's last Pleistocene position -
//the reverse of AstrolabeReturner, which queues a teleport back to Modern.
public class AstrolabePleistoceneUnlocker : MonoBehaviour
{
   // [Tooltip("Checks whether the quests required to unlock Pleistocene travel are complete")]
   // public QuestCompletionGetter unlockCondition;

    [Tooltip("Id of the quest whose update must also be unlocked (e.g. the one WallUntilQuestCompletion unlocks once the player has physically walked past the wall)")]
    public string gatingQuestId = "MainQuest";

    [Tooltip("Index of the update that must be unlocked before this can trigger")]
    public int gatingUpdateIndex = 2;

    void Start()
    {
        //if (unlockCondition == null || !unlockCondition.isComplete())
          //  return;

        QuestNode gatingNode = QuestManager.questManager.GETNode(gatingQuestId);
        if (gatingNode == null || !gatingNode.isUpdateUnlocked(gatingUpdateIndex))
        {
            return;
        } 
        else
        {
            Vector3 lastPleistPos = PlayerPositionManager.playerPositionManager.getCurrentPlayerPosition(1);

            if (!float.IsNegativeInfinity(lastPleistPos.x))
            {
                // map = 2 targets the Pleistocene queue/stack (see AstrolabeQueueManager.EnqueueDestination)
                AstrolabeQueueManager.queueManager.EnqueueDestination(lastPleistPos, 2);
            }
        }


            
    }
}