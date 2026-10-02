using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace Language
{
    [CreateAssetMenu(menuName = "Language/LanguageObject")]
    //scriptable object to store all text data for each language.
    public class LanguageObject : ScriptableObject
    {
        [Tooltip("Language name")]
        public string langName;
        
        [Header("Main Menu Buttons")]
        public static string continueBtn;
        public static string continueWith;
        public static string load;
        public static string newGame;
        public static string loadGame;
        public static string options;
        public static string credits;
        public static string help;
        public static string quit;
        public string[] mainMenuButtonArray = { continueBtn, continueWith, load, newGame, loadGame, options, credits, help, quit };
        
        [Header("Options Menu Buttons")]
        public static string graphics;
        public static string controls;
        public static string audio;
        public static string language;
        public static string back;
        public string[] optionsMenuButtonArray = { graphics, controls, audio, language, back };
        
        [Header("Graphics Menu Text")]
        public static string fullScreen;
        public static string vSync;
        public static string resolution;
        public static string cameraSensitivity;
        public static string applyChanges;
        public string[] graphicsMenuText = { fullScreen, vSync, resolution, cameraSensitivity, applyChanges };

        [Header("Dropdown Buttons")]
        public static string veryLow;
        public static string low;
        public static string medium;
        public static string high;
        public static string veryHigh;
        public static string ultra;
        public string[] dropdownButtons = { veryLow, low, medium, high, veryHigh, ultra};
        
        [Header("Controls Menu Text")]
        public static string moveForward;
        public static string moveBackward;
        public static string strafeLeft;
        public static string strafeRight;
        public static string jump;
        public static string questMenu;
        public static string hideMap;
        public static string astrolabe;
        public static string turnRight;
        public static string turnLeft;
        public static string sprint;
        public static string openMap;
        public static string interact;
        public static string skip;
        public static string flipBone;
        public static string openBoneUI;
        public static string stopClimbing;
        public static string endGame;
        public static string resetToDefaults;
        public string[] controlsMenuText =
        {
            moveForward, moveBackward, strafeLeft, strafeRight, jump, questMenu, hideMap, astrolabe,
            turnRight, turnLeft, sprint, openMap, interact, skip, flipBone, openBoneUI, stopClimbing, endGame, resetToDefaults
        };
        
        [Header("Audio Menu Text")]
        public static string master;
        public static string sfx;
        public static string dialogue;
        public static string music;
        public static string subtitles;
        public string[] audioMenuText = {master, sfx, dialogue, music, subtitles };
        
        [Header("Credits Menu Text Asset")]
        public TextAsset creditsText;
        
        [Header("Help Menu Text")]
        public string backToMainMenu;
        public string backToHelpMenu;
        
        //What am I supposed to do?
        public static string question1;
        //How do I fast travel and teleport?
        public static string question2;
        //How do I get rid of these flies?
        public static string question3;
        //Help, my astrolabe won't work!
        public static string question4;

        public string[] questions = { question1, question2, question3, question4 };
        
        public static TextAsset explanation1Text;
        public static TextAsset explanation2Text;
        public static TextAsset explanation3Text;
        public static TextAsset explanation4Text;

        public TextAsset[] explanations = { explanation1Text, explanation2Text, explanation3Text, explanation4Text};
        
        [Header("Create Save")]
        //Are you sure you would like to delete this save file and start a new game?
        public string deleteSave;
        [Tooltip("Slot number should be written as #slot and existing name should be #name")]
        //"Enter name for save in slot #slot or leave blank to use existing name #name"
        public string enterSaveName;
        //"Enter save name..."
        public string enterSaveNamePlaceholder;
        
        //"File name cannot be reserved name #name"
        [Tooltip("Reserved name should be #name")]
        public string fileNameInvalidReserved;
        //"File name cannot end with ."
        public string fileNameInvalidEnd;
        //"A save with that name exists in a different slot!"
        public string fileNameInvalidExists;
        
        public string submit;
        public string cancel;
        public string yes;
        public string no;
        
        
        [Header("Map Text")]
        public string modernMap;
        public string pleistoceneMap;
        //Press Escape or #openmap to close
        public string mapControls1;
        //scroll to zoom
        public string mapControls2;
        //Switch map view
        public string switchMapView;
        public string teleport;
        //waypoints
        public static string smallLake;
        public static string bonePile;
        public static string strangePlateaus;
        public static string caveSite;
        public static string thePast;
        public static string ancientLake;
        public static string pikaTerritory;
        public static string mammothFootprints;
        public static string tuiChub;
        public string[] waypoints = {smallLake, bonePile, strangePlateaus, caveSite, thePast, ancientLake, pikaTerritory, mammothFootprints, tuiChub};
        
        [Header("Subtitles Text Assets")]
        public static TextAsset BP1;
        public static TextAsset BP2;
        public static TextAsset BP3;
        public static TextAsset BP5_2;
        public static TextAsset BP6_1;
        public static TextAsset BP6_2;
        public static TextAsset BP6_3;
        public static TextAsset BP6_4;
        public static TextAsset BP6_5;
        public static TextAsset BP7_1;
        public static TextAsset BP7_2;
        public static TextAsset BP7_3;
        public static TextAsset BP8;
        public static TextAsset BP10;
        public static TextAsset BP11;
        public static TextAsset BP12;
        public static TextAsset FC1;
        public static TextAsset J1;
        public static TextAsset J2;
        public static TextAsset J3;
        public static TextAsset J4;
        public static TextAsset J5;
        public static TextAsset JL2;
        public static TextAsset J2_W;
        public static TextAsset JL3;
        public static TextAsset JL3_U;
        public static TextAsset MC1;
        public static TextAsset MC2_1;
        public static TextAsset MC2_2;
        public static TextAsset MC2_3;
        public static TextAsset MC2_4;
        public static TextAsset MC2_5;
        public static TextAsset MC2_7;
        public static TextAsset MC2_8;
        public static TextAsset MC2_9;
        public static TextAsset PQ1;
        public static TextAsset PQ1_3;
        public static TextAsset PQ3;
        public static TextAsset PQ4_1;
        public static TextAsset PQ4_2;
        public static TextAsset PQ4_3;
        public static TextAsset PQ5_1;
        public static TextAsset PQ5_2;
        public static TextAsset PQ5_3;
        public static TextAsset PQ5_4;
        public static TextAsset PQ5_5;
        public static TextAsset PQ5_6;
        public static TextAsset PQ6_1;
        public static TextAsset PQ6_2;
        public static TextAsset PQ6_3;
        public static TextAsset PQ7;
        public static TextAsset PQ8;
        public static TextAsset PQ9;
        public static TextAsset PQ10_1;
        public static TextAsset PQ10_2;
        public static TextAsset PQ10_3;
        public static TextAsset PQ10_4;
        public static TextAsset PQ10_5;
        public static TextAsset PQ10_6;
        public static TextAsset PQ10_7;
        public static TextAsset PQ10_8;
        public static TextAsset PQ10_9;
        public static TextAsset PQ10_10;
        public static TextAsset RC0;
        public static TextAsset RC0_1;
        public static TextAsset RC1;
        public static TextAsset RC2;
        public static TextAsset RC3;
        public static TextAsset RC4;
        public static TextAsset RC5;
        public static TextAsset RC6_1;
        public static TextAsset RC6_2;
        public static TextAsset RC7;
        public static TextAsset RC8;
        public static TextAsset RC8_1;
        public static TextAsset SC1;
        public static TextAsset SC2;
        public static TextAsset SC3;
        public static TextAsset UP1;
        public TextAsset[] subtitlesArr = { BP1, BP2, BP3, BP5_2, BP6_1, BP6_2, BP6_3, BP6_4, BP6_5, BP7_1, BP7_2, BP7_3, BP8, BP10, BP11, BP12, FC1, J1, J2, J3, J4, J5, JL2, J2_W, JL3, JL3_U, MC1, MC2_1, MC2_2, MC2_3, MC2_4, MC2_5, MC2_7, MC2_8, MC2_9, PQ1, PQ1_3,  PQ3, PQ4_1, PQ4_2, PQ4_3, PQ5_1 ,PQ5_2, PQ5_3, PQ5_4, PQ5_5, PQ5_6, PQ6_1, PQ6_2, PQ6_3, PQ7, PQ8, PQ9, PQ10_1, PQ10_2, PQ10_3 ,PQ10_4, PQ10_5, PQ10_6, PQ10_7, PQ10_8, PQ10_9, PQ10_10, RC0, RC0_1, RC1, RC2, RC3, RC4, RC5, RC6_1, RC6_2, RC7, RC8, RC8_1, SC1, SC2, SC3, UP1};

        
        
        
        public TextAsset GetSubtitles(string narrName)
        {
            foreach (TextAsset file in subtitlesArr)
            {
                if (narrName == file.name)
                {
                    return file;
                }
            }
            return null;
        }

        public string GetSaveHeaderText(int slot, string saveName)
        {
            enterSaveName = enterSaveName.Replace("#slot",  slot.ToString());
            enterSaveName = enterSaveName.Replace("#name",  saveName);
            return enterSaveName;
        }
        
        public string GetFileNameInvalidReserved(string inText)
        {
            fileNameInvalidReserved = fileNameInvalidReserved.Replace("#name", inText);
            return fileNameInvalidReserved;
        }

        public string GetMapControls(string inText)
        {
            mapControls1 = mapControls1.Replace("#openmap",  inText);
            return mapControls1 + "\n" + mapControls2;
        }
    }
}