using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class PopulateLevelList : MonoBehaviour
    {
        //level button prefab
        public GameObject levelButton;

        //scroll under which to instantiate buttons
        public Transform scrollContent;

        //when the gui loads, initialize it
        void Start()
        {
            List<LevelData> levels = MatchLevelManager.matchLevelManager.levels;
            bool shouldBeEnabled = true;
            for (int i = 1; i < levels.Count; i++)
            {
                if (i > 1)
                {
                    shouldBeEnabled = MatchLevelManager.matchLevelManager.levelsComplete[i-1];
                }
                createButtonPrefab(levels[i], shouldBeEnabled);
            }
            GameObject newButton = Instantiate(levelButton, scrollContent, false);
            newButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Endless";
            newButton.GetComponent<OpenLevel>().levelIndex = 0;
        }

        //creates a button prefab with the details of the passed level
        private void createButtonPrefab(LevelData level, bool shouldBeEnabled)
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
