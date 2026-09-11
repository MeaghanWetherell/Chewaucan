using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Misc;
using QuestSystem;
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

        [Tooltip("Wall hit narration from pleistocene for bonepile")]
        public Narration.Narration bpilePleistNarr;

        private void Awake()
        {
            RefreshTeleportAvailability();
        }

        private void OnEnable()
        {
            AstrolabeQueueManager.DestinationQueued += HandleDestinationQueued;
            RefreshTeleportAvailability(); //covers the case where the menu was closed
                                           //(and thus unsubscribed) when a destination got queued
        }

        private void OnDisable()
        {
            AstrolabeQueueManager.DestinationQueued -= HandleDestinationQueued;
        }

        private void HandleDestinationQueued(int map)
        {
            RefreshTeleportAvailability();
        }

        //re-checks the queue state and sets button interactability accordingly.
        //called on Awake and whenever a new destination is queued, so a quest
        //completing without a scene load (e.g. the plateau quest) still updates this.
        private void RefreshTeleportAvailability()
        {
            Teleposition1 = AstrolabeQueueManager.peekModern();
            Teleposition2 = AstrolabeQueueManager.peekPleist();
            int curScene = SceneLoadWrapper.sceneLoadWrapper.currentSceneType;

            bool hasDestination = curScene == 0
                ? !Teleposition2.Equals(Vector3.negativeInfinity)
                : !Teleposition1.Equals(Vector3.negativeInfinity);

            bool pleistGateCleared = curScene != 1 || bpilePleistNarr.HasPlayed();

            PastTeleportButton.interactable = hasDestination && pleistGateCleared;
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