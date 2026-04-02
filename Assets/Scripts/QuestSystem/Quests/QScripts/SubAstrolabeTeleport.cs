using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Misc;
using ScriptTags;
using TimeTravel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace QuestSystem.Quests.QScripts
{
    public class SubAstrolabeTeleport : MonoBehaviour
    {
        [Tooltip("the position the player should teleport to on completion of the associated quest or narration")]
        public Vector3 playerPosition;

        [Tooltip("The quest to sub to. Provide either a quest or a narration, not both")]
        public string subToId;

        [Tooltip("0 for modern 1 for pleistocene")]
        public int sceneToTeleport = 1;

        public Narration.Narration playOnAstrolabeOpen;

        public InputActionReference openAstrolabe;
        
        [Tooltip("Narration that must be played first. Provide either a quest or a narration, not both")]public Narration.Narration narration;

        private void Start()
        {
            if (openAstrolabe != null)
                openAstrolabe.action.performed += OnAstrolabeOpen;
            QuestNode subbedNode = QuestManager.questManager.GETNode(subToId);
            
            if (narration == null)
            {
                if (playOnAstrolabeOpen != null && !playOnAstrolabeOpen.HasPlayed() && subbedNode is { isComplete: true })
                {
                    playOnAstrolabeOpen?.SetPlayability(true);
                }
                else if(subbedNode is {isComplete: false})
                {
                    QuestManager.questManager.SubToCompletion(subToId, OnComp);
                }
            }
            else if(!narration.HasPlayed())
            {
                narration.addToOnComplete(new List<UnityAction<string>>{
                    OnComp});
            }
            else
            {
                if (playOnAstrolabeOpen != null && !playOnAstrolabeOpen.HasPlayed() && subbedNode is { isComplete: true })
                {
                    playOnAstrolabeOpen?.SetPlayability(true);
                }
            }
        }

        private void OnComp(string n)
        {
            if(HUDManager.hudManager != null)
                HUDManager.hudManager?.astrolabeUI?.gameObject.SetActive(true);
            AstrolabeQueueManager.queueManager.EnqueueDestination(playerPosition, sceneToTeleport+1);
            playOnAstrolabeOpen?.SetPlayability(true);
        }

        private void OnDisable()
        {
            if(openAstrolabe != null)
                openAstrolabe.action.performed -= OnAstrolabeOpen;
        }

        private void OnAstrolabeOpen(InputAction.CallbackContext context)
        {
            if (playOnAstrolabeOpen.GetPlayability())
            {
                AudioListener.pause = false;
                playOnAstrolabeOpen.Begin();
                playOnAstrolabeOpen.SetPlayability(false);
            }
        }
    }
}
