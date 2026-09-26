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
        public string continueBtn;
        public string resume;
        public string loadGame;
        public string options;
        public string credits;
        public string help;
        public string quit;
        
        [Header("Options Menu Buttons")]
        public string graphics;
        public string controls;
        public string audio;
        public string language;
        public string back;
        
        [Header("Graphics Menu Text")]
        public string fullScreen;
        public string vSync;
        public string resolution;
        public string cameraSensitivity;
        public string applyChanges;

        [Header("Dropdown Buttons")]
        public string veryLow;
        public string low;
        public string medium;
        public string high;
        public string veryHigh;
        public string ultra;
        
        [Header("Controls Menu Text")]
        public string moveForward;
        public string moveBackward;
        public string strafeLeft;
        public string strafeRight;
        public string jump;
        public string questMenu;
        public string hideMap;
        public string astrolabe;
        public string turnRight;
        public string turnLeft;
        public string sprint;
        public string openMap;
        public string interact;
        public string skip;
        public string flipBone;
        public string openBoneUI;
        public string stopClimbing;
        public string endGame;
        public string resetToDefaults;
        
        [Header("Audio Menu Text")]
        public string master;
        public string sfx;
        public string dialogue;
        public string music;
        public string subtitles;
        
        [Header("Credits Menu Text Asset")]
        public TextAsset creditsText;
        
        [Header("Help Menu Text")]
        public string backToMainMenu;
        public string backToHelpMenu;
        
        //What am I supposed to do?
        public string question1;
        //How do I fast travel and teleport?
        public string question2;
        //How do I get rid of these flies?
        public string question3;
        //Help, my astrolabe won't work!
        public string question4;
        
        public TextAsset explanation1Text;
        public TextAsset explanation2Text;
        public TextAsset explanation3Text;
        public TextAsset explanation4Text;
        
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
        public TextAsset[] subtitlesArr = new[] { BP1, BP2, BP3, BP5_2, BP6_1, BP6_2, BP6_3, BP6_4, BP6_5, BP7_1, BP7_2, BP7_3, BP8, BP10, BP11, BP12, FC1, J1, J2, J3, J4, J5, JL2, J2_W, JL3, JL3_U, MC1, MC2_1, MC2_2, MC2_3, MC2_4, MC2_5, MC2_7, MC2_8, MC2_9, PQ1, PQ1_3,  PQ3, PQ4_1, PQ4_2, PQ4_3, PQ5_1 ,PQ5_2, PQ5_3, PQ5_4, PQ5_5, PQ5_6, PQ6_1, PQ6_2, PQ6_3, PQ7, PQ8, PQ9, PQ10_1, PQ10_2, PQ10_3 ,PQ10_4, PQ10_5, PQ10_6, PQ10_7, PQ10_8, PQ10_9, PQ10_10, RC0, RC0_1, RC1, RC2, RC3, RC4, RC5, RC6_1, RC6_2, RC7, RC8, RC8_1, SC1, SC2, SC3, UP1};

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
    }
}