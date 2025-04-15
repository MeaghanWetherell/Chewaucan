using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportButton : MonoBehaviour
{
    private TeleportWaypoint teleportTo;
    //[SerializeField] Vector3 defaultTeleportTo = new Vector3(129f, 4f, 108f);

    // Start is called before the first frame update
    void Start()
    {
        teleportTo = null;
    }

    public void Teleport()
    {
        Debug.Log("teleport");
        if (teleportTo == null)
            return;
        Debug.Log("teleport2");
        PlayerPositionManager.playerPositionManager.setPlayerPosition(teleportTo.teleportToPosition, 0);
        if(teleportTo.mapType == TeleportWaypoint.MapType.modern)
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
        else
        {
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("PleistoceneMap");
        }
    }

    public void setTeleportTo(TeleportWaypoint newWP)
    {
        teleportTo = newWP;
    }
}
