using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem; //Required to run the questmanager function
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoneChecker : MonoBehaviour
{
    [NonSerialized]public bool isCorrect = false;

    public GameObject viewer; //The comparison bone from the Bone Pile Quest
    
    public GameObject mBoneViewer; //the mastodon bone
    
    public TextMeshProUGUI text;

    private Quaternion initialRotation; // store the starting rotation

    private void Start()
    {
        viewer.GetComponent<BoneChecker>().SetBoneRotation(viewer.transform.localEulerAngles);
        viewer.GetComponent<BoneChecker>().SetBoneScale(viewer.transform.localScale);
        // Save the initial rotation
        initialRotation = viewer.transform.rotation;
        Quaternion offsetRotation = initialRotation * Quaternion.Euler(0f, 90f, 0f); //rotate it 90 degrees
        viewer.transform.rotation = offsetRotation;
        initialRotation = offsetRotation;
    }

    // Update is called once per frame
    void Update()
    {

        // Calculate Y rotation difference
        float yDiff = GetYawDifference(viewer.transform.rotation, mBoneViewer.transform.rotation);

        // Check if flip on x or z is matching
        bool FlipMatches = IsFlipCorrect(viewer.transform.rotation, mBoneViewer.transform.rotation);

        // Track how much the player has rotated relative to starting point, so we know they've played a bit
        float playerRotationAmount = Quaternion.Angle(initialRotation, viewer.transform.rotation);


        //If it is correct
        if (isCorrect) {

            //player rotated it at all
            if (playerRotationAmount >1f) 
            {
                //and the rotation is off by 5 degrees or less
                if (yDiff < 5f && FlipMatches)
                {
                    text.text = "That's it! Your bone is the distal end of a femur.";
                    QuestManager.questManager.GETNode("bonepile").AddCount(0);
                    this.enabled = false;

                } else
                {
                    text.text = "This looks like the right bone, but not quite the right position.";
                }
                   
            } else    
            {
                // player hasn't rotated it at all
                text.text = "Rotate the bone to see if it matches yours.";
            }

        } else {
          

            //incorrect bone, but the player has rotated it sufficiently
            if (playerRotationAmount > 120f)
            {
                text.text = "Hmm, this doesn't seem to be the right bone. ";
                this.enabled = false;
            }
            else
            {
                text.text = "Rotate the bone to see if it matches yours.";
            }


        }

    }

    public void SetBoneScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
    public void SetBoneRotation(Vector3 newRotation)
    {
        transform.localEulerAngles = newRotation;
    }

    // Get shortest angular difference in Y (yaw)
    private float GetYawDifference(Quaternion a, Quaternion b)
    {
        Vector3 eulerA = a.eulerAngles;
        Vector3 eulerB = b.eulerAngles;

        float yDiff = Mathf.Abs(Mathf.DeltaAngle(eulerA.y, eulerB.y));
        return yDiff;
    }

    //Checks to see if the two are flipped in the same direction
    private bool IsFlipCorrect(Quaternion playerRotation, Quaternion targetRotation)
    {
        // Get "up" vectors for both bones in world space
        Vector3 playerUp = playerRotation * Vector3.up;
        Vector3 targetUp = targetRotation * Vector3.up;

        // Dot product measures alignment
        float dot = Vector3.Dot(playerUp, targetUp);

        // dot = +1 means perfectly aligned
        // dot = -1 means perfectly upside-down
        return dot > 0f; // True if not upside-down relative to target
    }
}
