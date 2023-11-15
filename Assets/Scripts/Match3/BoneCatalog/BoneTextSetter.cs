using System;
using TMPro;
using UnityEngine;

namespace Match3
{
    public class BoneTextSetter : MonoBehaviour
    {
        public TextMeshProUGUI boneName;

        public TextMeshProUGUI mainText;

        private void Awake()
        {
            boneName.text = BoneSceneManager.boneSceneManager.curObj.boneName;
            if (BoneSceneManager.boneSceneManager.curObj.animalDesc == null)
            {
                mainText.text = Resources.Load<DescObj>("LoremIpsum").description;
            }
            else
            {
                mainText.text = BoneSceneManager.boneSceneManager.curObj.animalDesc.description;
            }
        }
    }
}
