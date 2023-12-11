using UnityEngine;

namespace Match3
{
    //When the bone catalog scene is loaded, sets up the bone model from data in the bone scene manager
    public class SetupBone : MonoBehaviour
    {
        private void Awake()
        {
            this.GetComponent<MeshRenderer>().material = BoneSceneManager.boneSceneManager.curObj.material;
            this.GetComponent<MeshFilter>().mesh = BoneSceneManager.boneSceneManager.curObj.mesh;
        }
    }
}
