using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu]
    public class MeshDataList : ScriptableObject
    {
        public List<MeshDataObj> meshes;
    }
}
