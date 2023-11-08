using System;
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu]
    public class MeshDataObj : ScriptableObject
    {
        public Mesh mesh;

        public Material material;

        public String type;
    }
}
