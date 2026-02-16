using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameEndScript : MonoBehaviour
{
    public TextMeshProUGUI archaeologyScore;

    public TextMeshProUGUI biologyScore;

    public TextMeshProUGUI geologyScore;

    public TextMeshProUGUI overallScore;

    public TextMeshProUGUI rating;

    public Narration.Narration startNarration;
    
    public Narration.Narration MC2_1;

    public Narration.Narration MC2_2;

    public Narration.Narration MC2_3;

    public Narration.Narration MC2_4;

    public Narration.Narration MC2_5;

    private float completion = 0;
    
    private void Awake()
    {
        PauseCallback.pauseManager.Resume();
        
        DialProgress prog = SaveDialProgressData.LoadDialProgress();
        int[] countsPerType = QuestManager.questManager.CountsPerQuestType;

        archaeologyScore.text = prog.A_progress + "/" + countsPerType[0];
        
        biologyScore.text = prog.B_progress + "/" + countsPerType[1];
        
        geologyScore.text = prog.G_progress + "/" + countsPerType[2];

        int overallCompletions = prog.A_progress + prog.B_progress + prog.G_progress;
        int overallCount = countsPerType[0] + countsPerType[1] + countsPerType[2];
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
        
        startNarration.SetPlayability(true);
        startNarration.Begin(new List<UnityAction<string>>() {RunStoryNarr});
    }

    private void RunStoryNarr(string none)
    {
        QuestNode bonePile = QuestManager.questManager.GETNode("bonepile");
        
        //TODO change to appropriate id for levels quest
        QuestNode lakeLevels = QuestManager.questManager.GETNode("lakelevels");

        QuestNode boneJeweled1 = QuestManager.questManager.GETNode("match31");

        if (bonePile is not { isComplete: true })
        {
            MC2_1.SetPlayability(true);
            MC2_1.Begin();
        }
        else if (lakeLevels is not { isComplete: true })
        {
            MC2_2.SetPlayability(true);
            MC2_2.Begin();
        }
        else if (boneJeweled1 is not { isComplete: true })
        {
            MC2_3.SetPlayability(true);
            MC2_3.Begin();
        }
        else if (completion < 1)
        {
            MC2_4.SetPlayability(true);
            MC2_4.Begin();
        }
        else
        {
            MC2_5.SetPlayability(true);
            MC2_5.Begin();
        }
    }
}
