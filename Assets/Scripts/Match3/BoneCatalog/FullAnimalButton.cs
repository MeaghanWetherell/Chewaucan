using Match3.DataClasses;
using UnityEngine;

namespace Match3.BoneCatalog
{
    public class FullAnimalButton : MonoBehaviour
    {
        private bool isOpen = false;

        private static MeshDataObj lastBone;

        public void OnClick()
        {
            if (!isOpen)
            {
                lastBone = BoneSceneManager.boneSceneManager.curObj;
                MeshDataObj target = FindFullMesh(BoneSceneManager.boneSceneManager.curObj);
                BoneSceneManager.boneSceneManager.LoadBoneScene(target);
            }
            else
            {
                BoneSceneManager.boneSceneManager.LoadBoneScene(lastBone);
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
