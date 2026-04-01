using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class GameEndScript : MonoBehaviour
{
    public TextMeshProUGUI overallScore;

    public TextMeshProUGUI rating;

    public QuestObj scienceLogObj;

    [Tooltip("The default cutscene to play if the player doesn't meet the requirements for any cutscene")]
    public PlayableDirector defaultCutscene;

    [Tooltip("The list of all game ending cutscenes. Put these in descending order of priority-the script will play the first one in the list for which the player meets requirements.")]
    public List<PlayableDirector> unlockableCutscenes;

    [Tooltip("List of the unlock requirements for each cutscene, mapped by index to the cutscene that requires them.")]
    public List<QuestCompletionGetter> unlockRequirements;

    [Tooltip("Indices in the unlockable cutscenes list of cutscenes that unlock the science log")]
    public List<int> scienceLogUnlockIndices;

    public Canvas gameEndCanvas;
    
    private float completion = 0;

    public static float calcProg()
    {
        DialProgress prog = SaveDialProgressData.LoadDialProgress();
        int[] countsPerType = QuestManager.questManager.CountsPerQuestType;

        int overallCompletions = prog.A_progress + prog.B_progress + prog.G_progress+prog.N_progress;
        int overallCount = countsPerType[0] + countsPerType[1] + countsPerType[2]+countsPerType[3];
        return overallCompletions / (float) overallCount;
    }
    
    private void Awake()
    {
        gameEndCanvas.gameObject.SetActive(false);
        PlayableDirector playCutscene = null;
        for (int i = 0; i < unlockableCutscenes.Count; i++)
        {
            if (unlockRequirements.Count > i && unlockRequirements[i].isComplete())
            {
                playCutscene = unlockableCutscenes[i];
                if(scienceLogUnlockIndices.Contains(i))
                    QuestManager.questManager.CreateQuestNode(scienceLogObj);
                break;
            }
        }
        if (playCutscene == null)
            playCutscene = defaultCutscene;
        playCutscene.Play();
    }

    public void OnCutsceneComplete()
    {
        gameEndCanvas.gameObject.SetActive(true);
        PauseCallback.pauseManager.Resume();
        DialProgress prog = SaveDialProgressData.LoadDialProgress();
        int[] countsPerType = QuestManager.questManager.CountsPerQuestType;
        int overallCompletions = prog.A_progress + prog.B_progress + prog.G_progress+prog.N_progress;
        int overallCount = countsPerType[0] + countsPerType[1] + countsPerType[2]+countsPerType[3];
        overallScore.text = overallCompletions + "/" + overallCount;
        completion = overallCompletions / (float) overallCount;
        if (completion >= 0.75)
        {
            rating.text = "You told an Excellent story! Rating: Excellent";
            SteamAPIManager.UnlockAch("BestStory");
        }
        else if (completion >= 0.5)
        {
            rating.text = "You told a Good story! Rating: Good";
            SteamAPIManager.UnlockAch("GoodStory");
        }
        else if (completion >= 0.25)
        {
            rating.text = "You told an Okay story! Rating: Okay";
        }
        else
        {
            rating.text = "You told a Bad story! Rating: Bad";
            SteamAPIManager.UnlockAch("BadEnding");
        }
            

        if (completion >= 1)
        {
            SteamAPIManager.UnlockAch("FullComp");
        }
    }
    
}
