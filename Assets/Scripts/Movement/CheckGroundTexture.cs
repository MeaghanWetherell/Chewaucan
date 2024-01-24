using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckGroundTexture : MonoBehaviour
{

    CharacterController _controller;
    Terrain _terrain;
    int _xPos;
    int _zPos;
    public float[] textureVals;

    [Tooltip("The number of textures in the terrain layer palette asset")]
    public int numOfTextures = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void GetGroundTexture()
    {
        bool hasTerrain = SetTerrain();
        if (hasTerrain)
        {
            FindTexturePosition(_controller.gameObject.transform.position);
            FindTextureValue();
        }
    }

    void FindTexturePosition(Vector3 playerPos)
    {
        Vector3 terrainPos = playerPos - _terrain.transform.position;
        Vector3 mapPos = new Vector3(terrainPos.x / _terrain.terrainData.size.x, 0, terrainPos.z / _terrain.terrainData.size.z);
        float xCoord = mapPos.x * _terrain.terrainData.alphamapWidth;
        float zCoord = mapPos.z * _terrain.terrainData.alphamapHeight;
        _xPos = (int)xCoord;
        _zPos = (int)zCoord;
        FindTextureValue();
    }

    void FindTextureValue()
    {
        float[,,] alphaMap = _terrain.terrainData.GetAlphamaps(_xPos, _zPos, 1, 1);

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

            _terrain = objectHit.GetComponent<Terrain>();
            if (_terrain != null)
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
