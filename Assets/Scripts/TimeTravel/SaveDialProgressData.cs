using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// helper class to be used for saving the dial progress to json file
public class DialProgress
{
    public int A_progress; // Archeo quests completed
    public int G_progress; // Geology quests completed
    public int B_progress; // Biology quests completed
}

public static class SaveDialProgressData
{
    public enum Dial
    {
        ARCHEOLOGY,
        BIOLOGY,
        GEOLOGY,
        NONE
    }

    public static string saveDataPath;

    /* These ints represent the total number of quests of each type
     * They are set to placeholder values for now, but when we do know how
     * many quests of each type there are, we should change these numbers
     *
     * EDIT: QuestManager now handles this. It'll set them automatically to the right number
     */
    public static int archeologyQuestNum = 10;
    public static int geologyQuestNum = 8;
    public static int biologyQuestNum = 5;

    public static void CompleteOneQuest(Dial dial)
    {
        DialProgress currentProgress = LoadDialProgress();
        if (currentProgress == null)
        {
            currentProgress = new DialProgress();
            currentProgress.A_progress = 0;
            currentProgress.G_progress = 0;
            currentProgress.B_progress = 0;
        }

        if (dial == Dial.ARCHEOLOGY && currentProgress.A_progress < archeologyQuestNum)
        {
            currentProgress.A_progress += 1;
        }
        else if (dial == Dial.GEOLOGY && currentProgress.G_progress < geologyQuestNum)
        {
            currentProgress.G_progress += 1;
        }
        else if (dial == Dial.BIOLOGY && currentProgress.B_progress < biologyQuestNum)
        {
            currentProgress.B_progress += 1;
        }

        SaveDialProgress(currentProgress);
    }

    public static void SaveDialProgress(DialProgress dp)
    {
        if (saveDataPath == null || saveDataPath.Equals(""))
        {
            saveDataPath = SaveHandler.saveHandler.getSavePath() + "/DialProgess.json";
        }
        string saveDialProgress = JsonUtility.ToJson(dp);
        File.WriteAllText(saveDataPath, saveDialProgress);
    }

    public static DialProgress LoadDialProgress()
    {
        if (File.Exists(saveDataPath))
        {
            string loadDialProgress = File.ReadAllText(saveDataPath);
            DialProgress dp = JsonUtility.FromJson<DialProgress>(loadDialProgress);
            return dp;
        }

        return null;
    }

    //used mostly for debugging if needed
    public static void DeleteDialProgress()
    {
        if (File.Exists(saveDataPath))
        {
            File.Delete(saveDataPath);
        }
    }

    public static void SetArcheologyQuestNum(int n)
    {
        archeologyQuestNum = n;
    }

    public static void SetGeologyQuestNum(int n)
    {
        geologyQuestNum = n;
    }

    public static void SetBiologyQuestNum(int n) 
    { 
        biologyQuestNum = n;
    }
}
