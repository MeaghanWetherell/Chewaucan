using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplyGraphics : MonoBehaviour
{
    public Toggle FSToggle, VSToggle;

    public List<V2IntWrapper> validResolutions;

    public TextMeshProUGUI resText;

    public GameObject specialYNPrefab;

    public Button resRight;

    public Button resLeft;

    public TMP_Dropdown qualityDropdown;

    private int curRes;

    private bool priorFS;

    private int priorVS;

    private int priorRes;

    private int priorQL;

    private void Start()
    {
        List<Vector2Int> addedReses = GSSaver.GetAddedResolutions();
        foreach(Vector2Int add in addedReses)
            validResolutions.Add(new V2IntWrapper(add));
        validResolutions.Sort();
        Init();
        if (validResolutions[curRes].x != Screen.width || validResolutions[curRes].y != Screen.height)
        {
            validResolutions.Add(new V2IntWrapper(new Vector2Int(Screen.width, Screen.height)));
            GSSaver.AddResolution(validResolutions[^1].v2);
            validResolutions.Sort();
            for (int i = 0; i < validResolutions.Count; i++)
            {
                if (validResolutions[i].x == Screen.width && validResolutions[i].y == Screen.height)
                {
                    ChangeRes(i);
                    break;
                }
            }
        }
    }

    private void Init()
    {
        FSToggle.isOn = Screen.fullScreen;
        priorFS = Screen.fullScreen;
        VSToggle.isOn = QualitySettings.vSyncCount == 1;
        priorVS = QualitySettings.vSyncCount;
        for (int i = 0; i < validResolutions.Count; i++)
        {
            if (validResolutions[i].x == Screen.width && validResolutions[i].y == Screen.height)
            {
                ChangeRes(i);
                break;
            }
        }
        priorRes = curRes;
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        priorQL = QualitySettings.GetQualityLevel();
    }

    public void ResRight()
    {
        ChangeRes(Mathf.Min(curRes + 1, validResolutions.Count-1));
        if (curRes == validResolutions.Count - 1)
            resRight.interactable = false;
        resLeft.interactable = true;
    }

    public void ResLeft()
    {
        ChangeRes(Mathf.Max(curRes - 1, 0));
        if (curRes == 0)
            resLeft.interactable = false;
        resRight.interactable = true;
    }

    private void ChangeRes(int res)
    {
        curRes = res;
        resText.text = validResolutions[curRes].x + "x" + validResolutions[curRes].y;
    }

    public void Revert(string n)
    {
        QualitySettings.vSyncCount = priorVS;
        Screen.SetResolution(validResolutions[priorRes].x, validResolutions[priorRes].y, priorFS);
        QualitySettings.SetQualityLevel(priorQL, SceneManager.GetActiveScene().name.Equals("MainMenuUI"));
        FSToggle.isOn = priorFS;
        VSToggle.isOn = priorVS == 1;
        ChangeRes(priorRes);
        qualityDropdown.value = priorQL;
    }

    public void Confirm(string n)
    {
        priorFS = FSToggle.isOn;
        if (VSToggle.isOn)
            priorVS = 1;
        else { priorVS = 0;}
        priorRes = curRes;
        priorQL = qualityDropdown.value;
    }
    
    
    public void ApplyGraphicsSettings()
    {
        if (VSToggle.isOn) QualitySettings.vSyncCount = 1;
        else { QualitySettings.vSyncCount = 0; }
        Screen.SetResolution(validResolutions[curRes].x, validResolutions[curRes].y, FSToggle.isOn);
        QualitySettings.SetQualityLevel(qualityDropdown.value, SceneManager.GetActiveScene().name.Equals("MainMenuUI"));
        LoadGUIManager.loadGUIManager.InstantiateYNPopUp(Instantiate(specialYNPrefab), "Confirm Changes", "", 
            new List<UnityAction<string>>{Confirm}, "Confirm","Decline",new List<UnityAction<string>>{Revert});
        //Debug.Log(SceneManager.GetActiveScene().name.Equals("MainMenuUI"));
    }
}

[Serializable]
public class V2IntWrapper : IComparable<V2IntWrapper>
{
    public Vector2Int v2;

    public int x => v2.x;

    public int y => v2.y;

    public V2IntWrapper(Vector2Int myVector)
    {
        v2 = myVector;
    }
    
    public int CompareTo(V2IntWrapper other)
    {
        if (other.v2.x > v2.x) return -1;
        if (other.v2.x < v2.x) return 1;
        if (other.v2.y > v2.y) return -1;
        if (other.v2.y < v2.y) return 1;
        return 0;
    }
}
