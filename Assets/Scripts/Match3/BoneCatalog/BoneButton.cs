using Match3.DataClasses;
using UnityEngine;

namespace Match3
{
    //loads the bone viewer on click
    public class BoneButton : MonoBehaviour
    {
        public MeshDataObj data;

        public void OnClick()
        {
            if (data == null)
                return;
            BoneSceneManager.boneSceneManager.LoadBoneScene(data);
        }
    }
}
