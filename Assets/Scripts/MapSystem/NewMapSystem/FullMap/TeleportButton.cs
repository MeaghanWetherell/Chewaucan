using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportButton : MonoBehaviour
{
    private Vector3 teleportTo;
    [SerializeField] Vector3 defaultTeleportTo = new Vector3(129f, 4f, 108f);

    // Start is called before the first frame update
    void Start()
    {
        teleportTo = defaultTeleportTo;
    }

    public void Teleport()
    {
        PlayerPositionManager.playerPositionManager.setPlayerPosition(teleportTo);
        SceneManager.LoadScene("Modern Map");
    }

    public void setTeleportTo(Vector3 postition)
    {
        teleportTo = postition;
    }
}
