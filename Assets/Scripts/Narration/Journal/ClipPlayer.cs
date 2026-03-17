using System;
using System.Collections.Generic;
using Audio;
using LoadGUIFolder;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Narration.Journal
{
    public class ClipPlayer : MonoBehaviour
    {
        private static ClipPlayer curPlayer;
        
        public Narration clip;

        public Sprite playSprite;

        public Sprite stopSprite;

        private bool playing = false;

        private void OnEnable()
        {
            LoadGUIManager.loadGUIManager.SubtoUnload(StopAudio);
        }

        private void OnDisable()
        {
            LoadGUIManager.loadGUIManager.UnsubtoUnload(StopAudio);
        }

        private void StopAudio(string GUIName)
        {
            if (playing)
            {
                SoundManager.soundManager.StopNarration();
            }
        }

        public void OnButtonPress()
        {
            if (playing)
            {
                StopAudio("");
                GetComponent<Image>().sprite = playSprite;
            }
            else
            {
                if(curPlayer != null && curPlayer.playing)
                    curPlayer.OnButtonPress();
                GetComponent<Image>().sprite = stopSprite;
                AudioListener.pause = false;
                clip.Begin(new List<UnityAction<string>>() {StopListener});
                curPlayer = this;
            }
            playing = !playing;
        }

        private void StopListener(string id)
        {
            AudioListener.pause = true;
        }
    }
}
