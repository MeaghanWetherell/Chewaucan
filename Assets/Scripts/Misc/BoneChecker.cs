using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoneChecker : MonoBehaviour
{
    [NonSerialized]public bool isCorrect = false;

    public GameObject viewer;
    
    public GameObject mBoneViewer;
    
    public TextMeshProUGUI text;

    private void Start()
    {
        viewer.transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCorrect && (viewer.transform.rotation.eulerAngles - mBoneViewer.transform.rotation.eulerAngles).magnitude < 10f)
        {
            text.text = "It's a little off, but this looks about right.";
            QuestManager.questManager.GETNode("bonepile").AddCount(0);
            this.enabled = false;
        }
    }
}
