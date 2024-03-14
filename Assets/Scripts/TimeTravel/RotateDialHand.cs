using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * idea of what to do instead
 * float rotation amount = 360
 *      how much counterclockwise the dials start in
 *      in the future, astrolabe will only be unlockable after completing at least 1 quest
 * increase progress by interval according to number of quests
 * only able to move right, no reason to revert progress directly
 *      that would only be done by loading a previous save
 */

public class RotateDialHand : MonoBehaviour
{
 
    [SerializeField] private float minRotation = -360f;

    public float archeologyRotationDefault = 335f;
    public float geologyRotationDefault = 298f;
    public float biologyRotationDefault = 228f;

    [SerializeField] private List<RectTransform> dialHandTransforms = new List<RectTransform>();

    [SerializeField] private SaveDialProgressData.Dial currentDial;

    private void Start()
    {
        Debug.Log("SETTING ROTATIONS");
        SetHandsToRotations();
    }

    private void SetHandsToRotations()
    {
        DialProgress dp = SaveDialProgressData.LoadDialProgress();
        float[] defaults = { archeologyRotationDefault, geologyRotationDefault, biologyRotationDefault };
        if (dp != null)
        {
            int[] progresses = { dp.A_progress, dp.G_progress, dp.B_progress };
            for (int i = 0; i < dialHandTransforms.Count; i++)
            {
                SaveDialProgressData.Dial dial = (SaveDialProgressData.Dial)i;
                Debug.Log(dial.ToString());

                RectTransform hand = dialHandTransforms[i];

                float interval = CalculateRotInterval(dial);
                float rotationAmount = interval * progresses[i];
                float finalRotation = defaults[i] - rotationAmount;

                Vector3 dialRot = SetDialRotation(hand, finalRotation);
                hand.rotation = Quaternion.Euler(dialRot);
            }
        }
        else
        {
            for (int i = 0; i < dialHandTransforms.Count; i++)
            {
                RectTransform hand = dialHandTransforms[i];
                Debug.Log("DEFAULT");
                Vector3 dialRot = SetDialRotation(hand, defaults[i]);
                hand.rotation = Quaternion.Euler(dialRot);
            }
        }
    }

    private Vector3 SetDialRotation(RectTransform r, float z)
    {
        Vector3 q = Quaternion.Euler(r.rotation.x, r.rotation.y, z).eulerAngles;

        return q;
    }
    
    private float CalculateRotInterval(SaveDialProgressData.Dial dial)
    {
        int questNum = 0;
        if (dial == SaveDialProgressData.Dial.ARCHEOLOGY)
        {
            questNum = SaveDialProgressData.archeologyQuestNum;
        }
        else if (dial == SaveDialProgressData.Dial.GEOLOGY)
        {
            questNum = SaveDialProgressData.geologyQuestNum;
        }
        else if (dial == SaveDialProgressData.Dial.BIOLOGY)
        {
            questNum = SaveDialProgressData.biologyQuestNum;
        }

        float interval = Mathf.Abs(minRotation/questNum);
        return interval;
    }

}
