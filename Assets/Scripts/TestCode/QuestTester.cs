using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;
using UnityEngine.Events;

public class QuestTester : MonoBehaviour
{
    public List<QuestObj> testObjects;
    
    private UnityEvent<float> w = new wPressed();
    
    private UnityEvent<float> b = new bPressed();
    // Start is called before the first frame update
    void Start()
    {
        QuestNode q1 = new QuestNode(testObjects[0]);
        w.AddListener(q1.onAction);
        QuestNode q2 = new QuestNode(testObjects[1]);
        b.AddListener(q2.onAction);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            w.Invoke(1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            b.Invoke(1);
        }
    }
    
    private class wPressed : UnityEvent<float> {}
    
    private class bPressed : UnityEvent<float> {}
}
