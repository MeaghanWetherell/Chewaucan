using TMPro;
using UnityEngine;

namespace Match3
{
    //loads the description of a bone into the catalog scene's description box.
    //loads lorem ipsum by default if there is no description
    public class BoneTextSetter : MonoBehaviour
    {
        public TextMeshProUGUI boneName;

        public TextMeshProUGUI mainText;

        private static string lorem;

        private void Awake()
        {
            boneName.text = BoneSceneManager.boneSceneManager.curObj.boneName;
            if (BoneSceneManager.boneSceneManager.curObj.animalDesc == null)
            {
                if (lorem == null)
                {
                    TextAsset text = Resources.Load("meshes/descriptions/LoremIpsum") as TextAsset;
                    lorem = text.ToString();
                }
                mainText.text = lorem;
            }
            else
            {
                mainText.text = BoneSceneManager.boneSceneManager.curObj.animalDesc.ToString();
            }
            if (BoneSceneManager.boneSceneManager.curObj.GetMatchCount() > 0)
            {
                mainText.text += "\n# Matched in Endless: " + BoneSceneManager.boneSceneManager.curObj.GetMatchCount();
            }
        }
    }
}
