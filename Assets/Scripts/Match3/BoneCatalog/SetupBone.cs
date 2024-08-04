using UnityEngine;

namespace Match3
{
    //When the bone catalog scene is loaded, sets up the bone model from data in the bone scene manager
    public class SetupBone : MonoBehaviour
    {
        public GameObject bonePrefab;
        
        private void Awake()
        {
            bonePrefab = BoneSceneManager.boneSceneManager.curObj.meshPrefab;
            Instantiate(bonePrefab, transform);
        }
    }
}
