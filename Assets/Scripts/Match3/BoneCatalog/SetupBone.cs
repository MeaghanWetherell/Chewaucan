using System;
using UnityEngine;

namespace Match3
{
    public class SetupBone : MonoBehaviour
    {
        private void Awake()
        {
            this.GetComponent<MeshRenderer>().material = BoneSceneManager.boneSceneManager.curObj.material;
            this.GetComponent<MeshFilter>().mesh = BoneSceneManager.boneSceneManager.curObj.mesh;
        }
    }
}
