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
                if(lorem == null)
                    lorem = Resources.Load<DescObj>("LoremIpsum").description;
                mainText.text = lorem;
            }
            else
            {
                mainText.text = BoneSceneManager.boneSceneManager.curObj.animalDesc.description;
            }
        }
    }
}
