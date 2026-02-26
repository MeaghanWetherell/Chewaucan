using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateDialHand : MonoBehaviour
{
 
    [SerializeField] private float maxRotation = -410f;

    private float archeologyRotationDefault = -60f;
    private float geologyRotationDefault = -60f;
    private float biologyRotationDefault = -60f;

    //public List<GameObject> textObjs;

    public Transform hand;

    public float rotationPerSecond;
        
    //[SerializeField] private List<Transform> dialHandTransforms = new List<Transform>();

    private void Start()
    {
        //Debug.Log("SETTING ROTATIONS");
        SetHandsToRotations();
    }

    private IEnumerator RotateOverTime(float rotationAmount)
    {
        float totalRot = 0;
        float rps = rotationPerSecond;
        if (rotationAmount < 0)
            rps *= -1;
        while (totalRot < Mathf.Abs(rotationAmount))
        {
            if (totalRot + Mathf.Abs(Time.deltaTime*rps) > Mathf.Abs(rotationAmount))
            {
                if(rotationAmount > 0)
                    hand.Rotate(new Vector3(0,0,1), rotationAmount-totalRot);
                else
                {
                    hand.Rotate(new Vector3(0,0,-1), Mathf.Abs(rotationAmount)-totalRot);
                }
            }
            else
            {
                hand.Rotate(new Vector3(0,0,1), rps*Time.deltaTime);
            }
            totalRot += rotationPerSecond*Time.deltaTime;
            yield return null;
        }
        
    }

    // loads quest save data and sets the astrolabe dial hands to appropriate rotations
    // based on how many have been completed
    private void SetHandsToRotations()
    {
        DialProgress dp = SaveDialProgressData.LoadDialProgress(); //get quest save data
        float[] defaults = { archeologyRotationDefault, biologyRotationDefault, geologyRotationDefault };

        if (dp != null)
        {
            int[] progresses = { dp.A_progress, dp.B_progress, dp.G_progress };
            int prog = progresses[0] + progresses[1] + progresses[2];
            float interval = CalculateRotInterval(SaveDialProgressData.Dial.NONE);
            float rotationAmount = interval * prog;
            StartCoroutine(RotateOverTime(rotationAmount));
            /*
            for (int i = 0; i < dialHandTransforms.Count; i++)
            {
                SaveDialProgressData.Dial dial = (SaveDialProgressData.Dial)i;
                //Debug.Log(dial.ToString());

                Transform hand = dialHandTransforms[i];

                float interval = CalculateRotInterval(dial);

                // total rotation = interval * (number of quests completed)
                float rotationAmount = interval * progresses[i];

                if(progresses[i] > 0)
                    textObjs[i].SetActive(true);

                hand.Rotate(new Vector3(0,0,1), rotationAmount);
            } */
        }
    }
    
    /* Calculates how much to rotate the dial hand based on the number of quests
     * in a category and the min rotation.
     */
    private float CalculateRotInterval(SaveDialProgressData.Dial dial)
    {
        int questNum = 0;
        float add = 0;
        if (dial == SaveDialProgressData.Dial.ARCHEOLOGY)
        {
            questNum = SaveDialProgressData.archeologyQuestNum;
            add = archeologyRotationDefault;
        }
        else if (dial == SaveDialProgressData.Dial.GEOLOGY)
        {
            questNum = SaveDialProgressData.geologyQuestNum;
            add = geologyRotationDefault;
        }
        else if (dial == SaveDialProgressData.Dial.BIOLOGY)
        {
            questNum = SaveDialProgressData.biologyQuestNum;
            add = biologyRotationDefault;
        }
        else
        {
            questNum = SaveDialProgressData.biologyQuestNum+SaveDialProgressData.geologyQuestNum+SaveDialProgressData.archeologyQuestNum;
            add = archeologyRotationDefault;
        }

        if (questNum == 0) return 0;
        
        float interval = (maxRotation-add)/questNum;
        return interval;
    }

}
