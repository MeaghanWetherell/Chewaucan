using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MeshDataList : ScriptableObject
{
    public List<Mesh> meshes;

    public List<Material> materials;
}
