using UnityEngine;

namespace Match3
{
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
