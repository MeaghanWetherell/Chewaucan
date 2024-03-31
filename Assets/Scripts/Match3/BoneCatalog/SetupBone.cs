using UnityEngine;

namespace Match3
{
    //When the bone catalog scene is loaded, sets up the bone model from data in the bone scene manager
    public class SetupBone : MonoBehaviour
    {
        public static MeshRenderer boneRenderer;

        public static MeshFilter boneFilter;
        
        private void Awake()
        {
            boneRenderer = this.GetComponent<MeshRenderer>();
            boneRenderer.material = BoneSceneManager.boneSceneManager.curObj.material;
            boneFilter = this.GetComponent<MeshFilter>();
            boneFilter.mesh = BoneSceneManager.boneSceneManager.curObj.mesh;
        }
    }
}
