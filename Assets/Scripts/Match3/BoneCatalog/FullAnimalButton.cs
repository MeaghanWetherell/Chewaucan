using Match3.DataClasses;
using UnityEngine;

namespace Match3.BoneCatalog
{
    public class FullAnimalButton : MonoBehaviour
    {
        private bool isOpen = false;

        public void OnClick()
        {
            if (!isOpen)
            {
                MeshDataObj target = FindFullMesh(BoneSceneManager.boneSceneManager.curObj);
                SetupBone.boneRenderer.material = target.material;
                SetupBone.boneFilter.mesh = target.mesh;
            }
            else
            {
                SetupBone.boneRenderer.material = BoneSceneManager.boneSceneManager.curObj.material;
                SetupBone.boneFilter.mesh = BoneSceneManager.boneSceneManager.curObj.mesh;
            }
            isOpen = !isOpen;
        }

        private MeshDataObj FindFullMesh(MeshDataObj input)
        {
            MeshDataList fullModels = Resources.Load<MeshDataList>("Meshes/FullAnimalMeshes/FullAnimalMeshes");
            foreach (MeshDataObj obj in fullModels.meshes)
            {
                if (obj.animal == input.animal)
                    return obj;
            }
            return null;
        }
    }
}
