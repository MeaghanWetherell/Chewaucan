using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckGroundTexture : MonoBehaviour
{

    CharacterController controller;
    Terrain terrain;
    int xPos;
    int zPos;
    public float[] textureVals;

    [Tooltip("The number of textures in the terrain layer palette asset")]
    public int numOfTextures = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void GetGroundTexture()
    {
        bool hasTerrain = SetTerrain();
        if (hasTerrain)
        {
            FindTexturePosition(controller.gameObject.transform.position);
            FindTextureValue();
        }
    }

    void FindTexturePosition(Vector3 playerPos)
    {
        Vector3 terrainPos = playerPos - terrain.transform.position;
        Vector3 mapPos = new Vector3(terrainPos.x / terrain.terrainData.size.x, 0, terrainPos.z / terrain.terrainData.size.z);
        float xCoord = mapPos.x * terrain.terrainData.alphamapWidth;
        float zCoord = mapPos.z * terrain.terrainData.alphamapHeight;
        xPos = (int)xCoord;
        zPos = (int)zCoord;
        FindTextureValue();
    }

    void FindTextureValue()
    {
        float[,,] alphaMap = terrain.terrainData.GetAlphamaps(xPos, zPos, 1, 1);

        for (int i = 0; i < numOfTextures; i++)
        {
            textureVals[i] = alphaMap[0, 0, i];
        }
    }

    public float[] GetValues()
    {
        return textureVals;
    }

    bool SetTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            GameObject objectHit = hit.collider.gameObject;

            terrain = objectHit.GetComponent<Terrain>();
            if (terrain != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
