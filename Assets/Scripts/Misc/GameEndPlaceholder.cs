using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using TMPro;
using UnityEngine;

public class GameEndPlaceholder : MonoBehaviour
{
    public TextMeshProUGUI archaeologyScore;

    public TextMeshProUGUI biologyScore;

    public TextMeshProUGUI geologyScore;

    public TextMeshProUGUI overallScore;

    public TextMeshProUGUI rating;

    private void Awake()
    {
        DialProgress prog = SaveDialProgressData.LoadDialProgress();
        int[] countsPerType = QuestManager.questManager.CountsPerQuestType;

        archaeologyScore.text = prog.A_progress + "/" + countsPerType[0];
        
        biologyScore.text = prog.B_progress + "/" + countsPerType[1];
        
        geologyScore.text = prog.G_progress + "/" + countsPerType[2];

        int overallCompletions = prog.A_progress + prog.B_progress + prog.G_progress;
        int overallCount = countsPerType[0] + countsPerType[1] + countsPerType[2];
        overallScore.text = overallCompletions + "/" + overallCount;

        float completion = overallCompletions / (float) overallCount;
        if (completion > 0.75)
        {
            rating.text = "You told an Excellent story! Rating: Excellent";
        }
        else if (completion > 0.5)
        {
            rating.text = "You told a Good story! Rating: Good";
        }
        else if (completion > 0.25)
        {
            rating.text = "You told an Okay story! Rating: Okay";
        }
        else
        {
            rating.text = "You told a Bad story! Rating: Bad";
        }
    }
}
