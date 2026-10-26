using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;

namespace Language
{
    [CreateAssetMenu(menuName = "Language/LanguageObject")]
    //scriptable object to store all text data for each language.
    public class LanguageObject : ScriptableObject
    {
        [Tooltip("Language name")] public string langName;

        [Header("Main Menu Buttons")] public static string continueBtn;
        public static string continueWith;
        public static string load;
        public static string newGame;
        public static string loadGame;
        public static string options;
        public static string credits;
        public static string help;
        public static string quit;

        public string[] mainMenuButtonArray =
            { continueBtn, continueWith, load, newGame, loadGame, options, credits, help, quit };

        [Header("Options Menu Buttons")] public static string graphics;
        public static string controls;
        public static string audio;
        public static string language;
        public static string back;
        public string[] optionsMenuButtonArray = { graphics, controls, audio, language, back };

        [Header("Graphics Menu Text")] public static string fullScreen;
        public static string quality;
        public static string vSync;
        public static string resolution;
        public static string cameraSensitivity;
        public static string applyChanges;
        public string[] graphicsMenuText = { fullScreen, quality, vSync, resolution, cameraSensitivity, applyChanges };

        [Header("Dropdown Buttons")] public static string veryLow;
        public static string low;
        public static string medium;
        public static string high;
        public static string veryHigh;
        public static string ultra;
        public string[] dropdownButtons = { veryLow, low, medium, high, veryHigh, ultra };

        [Header("Dropdown Languages")] public static string english;
        public static string french;
        public string[] dropdownLanguages = { english, french };

        [Header("Controls Menu Text")] public static string moveForward;
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
            turnRight, turnLeft, sprint, openMap, interact, skip, flipBone, openBoneUI, stopClimbing, endGame,
            resetToDefaults
        };

        [Header("Audio Menu Text")] public static string master;
        public static string sfx;
        public static string dialogue;
        public static string music;
        public static string subtitles;
        public string[] audioMenuText = { master, sfx, dialogue, music, subtitles };

        [Header("Help Menu Text")] public static string backToMainMenu;
        public static string backToHelpMenu;

        //What am I supposed to do?
        public static string question1;

        //How do I fast travel and teleport?
        public static string question2;

        //How do I get rid of these flies?
        public static string question3;

        //Help, my astrolabe won't work!
        public static string question4;

        public string[] helpMenuText = { question1, question2, question3, question4, backToMainMenu, backToHelpMenu };

        [Header("Match 3 Level Select")]
        public string level; //Level
        public static string levels; //Levels
        public static string boneCatalogue; //The Bone Catalogue
        public string[] levelSelectText = {levels, boneCatalogue };
        
        [Header("Match 3")]
        public string lose; //You Lose!
        public string retry; //Retry
        public string score; //Score
        public string playAgain; //Play Again!
        public string win; //You Win!
        public string nextLevel; //Next Level
        public string noMatches; //No matches remaining!
        public string scoreReached; //You reached the required score!
        public string timeUp; //Time up!
        public string time; //Time: 
        public string timeRemaining; //Time Remaining: 
        public string outOf; //out of
        public string highScore; //High Score
        public string highScores; //High Scores
        public string endless; //Endless
        public string objective; //Objective:
        public string boneView; //Bone View

        [Header("Match 3 Rules")]
        public string swapBones; //Swap bones to identify them by making #threeOrMore matches in a row
        public string threeOrMore; //3 or more
        public string matchBones; //Match bones by #type
        public string dragBones; //#rightClickDrag to move bone
        public string viewBones; //To view, #rightClick
        public string anatomy;
        public string species;
        public string anatomySpecies; //anatomy and species
        public string rightClick;
        public string rightClickDrag; //Right click and drag

        [Header("Match 3 Objectives")]
        public string endlessObjective; //Play endlessly for fun!
        public string femursObjective; //Match femurs to identify your fossil.
        public string vertObjective; //Match vertebrae to find clues about the past.
        public string bonesObjective; //Match bones within a species to uncover clues. 
        

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


        [Header("Map Text")] public string modernMap;

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

        public string[] waypoints =
        {
            smallLake, bonePile, strangePlateaus, caveSite, thePast, ancientLake, pikaTerritory, mammothFootprints,
            tuiChub
        };
    
        [Header("Bone Names")]
        public static string antFemur; //Antelope Femur
        public static string antHum; //Antelope Humerus
        public static string antRad; //Antelope Radius
        public static string antToe; //Antelope Toe
        public static string antUlna; //Antelope Ulna
        public static string antVert; //Antelope Vertebrae
        public static string cowCal; //Cow Calcaneum
        public static string cowFemur; //Cow Femur
        public static string cowHum; //Cow Humerus
        public static string cowTalus; //Cow Talus
        public static string cowTibia; //Cow Tibia
        public static string horseHum; //Horse Humerus
        public static string horseFemur; //Horse Femur
        public static string jackrabbitFemur; //Jackrabbit Femur
        public static string jackrabbitHum; //Jackrabbit Humerus
        public static string jackrabbitSkull; //Jackrabbit Skull
        public static string jackrabbitVert; //Jackrabbit Vertebrae
        public static string mammothVert; //Mammoth Vertebrae
        public static string marmotFemur; //Marmot Femur
        public static string marmotHum; //Marmot Humerus
        public static string marmotSkull; //Marmot Skull
        public static string mastodonFemur; //Mastodon Femur
        public static string pikaFemur; //Pika Femur
        public static string pikaHum; //Pika Humerus
        public static string pikaSkull; //Pika Skull
        public static string salmonidVert; //Salmonid Vertebrae
        public static string slothFemur; //Giant Ground Sloth Femur
        public static string chubVert; //Tui Chub Vertebrae

        public string[] boneNamesArr =
        {
            antFemur, antHum, antRad, antToe, antUlna, antVert, cowCal, 
            cowFemur, cowHum, cowTalus, cowTibia, horseHum, horseFemur, jackrabbitFemur, jackrabbitHum, 
            jackrabbitSkull, jackrabbitVert, mammothVert, marmotFemur, marmotHum, marmotSkull,
            mastodonFemur, pikaFemur, pikaHum, pikaSkull, salmonidVert, slothFemur, chubVert
        };


        public TextAsset GetSubtitles(string narrName)
        {
            return Resources.Load<TextAsset>("TextFiles/"+LanguageManager.GetLanguageObj().name+"/Subtitles/"+narrName);
        }

        public string GetSaveHeaderText(int slot, string saveName)
        {
            enterSaveName = enterSaveName.Replace("#slot", slot.ToString());
            enterSaveName = enterSaveName.Replace("#name", saveName);
            return enterSaveName;
        }

        public string GetFileNameInvalidReserved(string inText)
        {
            fileNameInvalidReserved = fileNameInvalidReserved.Replace("#name", inText);
            return fileNameInvalidReserved;
        }

        public string GetMapControls(string inText)
        {
            mapControls1 = mapControls1.Replace("#openmap", inText);
            return mapControls1 + "\n" + mapControls2;
        }

        //0 = anatomy, 1 = species, 2 = both
        public string GetMatch3Rules(int type)
        {
            string rules = "";
            rules += swapBones.Replace("#threeOrMore", "\n<style=\"M3\">" + threeOrMore + "</style>\n");
            rules += "\n";
            string typeString = "";
            switch (type)
            {
                case 0:
                    typeString = anatomy;
                    break;
                case 1:
                    typeString = species;
                    break;
                case 2:
                    typeString = anatomySpecies;
                    break;
            }
            rules += matchBones.Replace("#type", "\n<style=\"M3\">" + typeString + "</style>\n");
            rules += "\n";
            rules += dragBones.Replace("#rightClickDrag", "\n<style=\"M3\">" + rightClickDrag + "</style>\n");
            rules += "\n";
            rules += viewBones.Replace("#rightClick", "\n<style=\"M3\">" + rightClick + "</style>\n");
            return rules;
        }

        public string GetBoneName(string bone)
        {
            for (int i = 0; i < boneNamesArr.Length; i++)
            {
                if (bone == LanguageManager.GetEnglishObj().boneNamesArr[i])
                {
                    return boneNamesArr[i];
                }
            }

            return null;

        }
    }
}