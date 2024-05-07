using UnityEngine;

/*
 * This method of finding the terrain texture was made with the help of this tutorial:
 * https://johnleonardfrench.com/terrain-footsteps-in-unity-how-to-detect-different-textures/
 * 
 * As well as assitance from this reddit thread:
 * https://www.reddit.com/r/Unity3D/comments/yf02lz/footstep_system_detect_the_layer_name_on_the/
 */

/*
 * You can find the relevant terrain palette in the folder Assests/GroundTextures/TerrainPalette
 */

public class CheckGroundTexture : MonoBehaviour
{

    CharacterController _controller;
    Terrain _terrain;
    int _xPos;
    int _zPos;
    float[] textureVals;

    [Tooltip("The number of textures in the terrain layer palette asset")]
    
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
    }

    /*
     * Finds the alpha (transparency) values of the terrain layers on the current terrain
     * and puts them into a single float array.
     */
    void FindTextureValue()
    {
        float[,,] alphaMap = _terrain.terrainData.GetAlphamaps(_xPos, _zPos, 1, 1);

        float[] mapLayers = new float[alphaMap.GetUpperBound(2) + 1];

        for (int n = 0; n < mapLayers.Length; n++)
        {
            mapLayers[n] = alphaMap[0, 0, n];
        }

        textureVals = mapLayers;
    }

    /*
     * finds the layer name of the layer with the greatest alpha value on the terrain,
     * or returns default "rock" if something cannot be found
     */
    public string GetCurrentLayerName()
    {
        float highest = 0f;
        int maxIndex = 0;
        for (var i = 0; i < textureVals.Length; i++)
        {
            if (textureVals[i] > highest)
            {
                maxIndex = i;
                highest = textureVals[i];
            }
        }

        if (SetTerrain())
        {
            return _terrain.terrainData.terrainLayers[maxIndex].name;
        }
        return "rock";
    }

    public float[] GetValues()
    {
        return textureVals;
    }

    // find the terrain the player is currently standing on
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
