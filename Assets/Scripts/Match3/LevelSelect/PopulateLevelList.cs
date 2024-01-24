using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    //manages the list of match 3 levels on the ui
    public class PopulateLevelList : MonoBehaviour
    {
        [Tooltip("level button prefab")]
        public GameObject levelButton;

        [Tooltip("scroll under which to instantiate buttons")]
        public Transform scrollContent;

        [Tooltip("Bone Collection Button")] 
        public Button boneButton;

        private void Awake()
        {
            if (!OpenLevelSelect.openLevelSelect.shouldLoadBone)
            {
                boneButton.interactable = true;
                this.GetComponent<Button>().interactable = false;
                PopulateGUI();
            }
        }

        //clean up the gui and repopulate when the user returns to the level menu
        public void OnLevelButtonClicked()
        {
            boneButton.interactable = true;
            this.GetComponent<Button>().interactable = false;
            for (int i = 0; i < scrollContent.transform.childCount; i++)
            {
                Destroy(scrollContent.GetChild(i).gameObject);
            }
            PopulateGUI();
        }
        
        //add buttons to the GUI
        private void PopulateGUI()
        {
            List<LevelData> levels = MatchLevelManager.matchLevelManager.levels;
            bool shouldBeEnabled = true;
            for (int i = 1; i < levels.Count; i++)
            {
                if (i > 1)
                {
                    shouldBeEnabled = MatchLevelManager.matchLevelManager.levelsComplete[i-1];
                }
                CreateButtonPrefab(levels[i], shouldBeEnabled);
            }
            GameObject newButton = Instantiate(levelButton, scrollContent, false);
            newButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Endless";
            newButton.GetComponent<OpenLevel>().levelIndex = 0;
        }

        //creates a button prefab with the details of the passed level
        private void CreateButtonPrefab(LevelData level, bool shouldBeEnabled)
        {
            GameObject newButton = Instantiate(levelButton, scrollContent, false);
            newButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Level "+level.levelNum;
            if (!shouldBeEnabled)
            {
                newButton.GetComponent<Button>().interactable = false;
            }
            newButton.GetComponent<OpenLevel>().levelIndex = level.levelNum;
        }
    }
}
