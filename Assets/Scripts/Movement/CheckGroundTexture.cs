using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundTexture : MonoBehaviour
{

    CharacterController controller;
    Terrain terrain;
    int xPos;
    int zPos;
    public float[] textureVals;
    
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
        else
        {
            //Debug.Log("Not standing over terrain");
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

        textureVals[0] = alphaMap[0, 0, 0];
        textureVals[1] = alphaMap[0, 0, 1];
        textureVals[2] = alphaMap[0, 0, 2];
        textureVals[3] = alphaMap[0, 0, 3];
    }

    public float[] GetValues()
    {
        return textureVals;
    }

    bool SetTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, controller.height))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.black);
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
