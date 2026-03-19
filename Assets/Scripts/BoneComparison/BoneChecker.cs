using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using LoadGUIFolder;
using QuestSystem; //Required to run the questmanager function
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BoneChecker : MonoBehaviour
{
    //stores whether the current view bone is correct
    [NonSerialized]public bool isCorrect = false;

    [Tooltip("The comparison bone from the Bone Pile Quest")]
    public GameObject viewer; 
    
    [Tooltip("the mastodon bone")]
    public GameObject mBoneViewer; 

    [Tooltip("The amount of time between audio triggers telling the player whether their bone looks correct or not")]
    public float timeBetweenBarks;
    
    [Tooltip("Text telling the player if they have found the correct bone or not")]
    public TextMeshProUGUI resultText;

    [Tooltip("List of barks telling the player their bone is not correct")]
    public List<Narration.Narration> incorrectBarks;

    [Tooltip("List of barks telling the player their bone is correct")]
    public List<Narration.Narration> correctBarks;

    [Tooltip("Text telling the player the name of the bone they are comparing to theirs")]
    public TextMeshProUGUI boneNameTMP;

    // store the starting bone rotations
    private Quaternion initialRotation; 

    private Quaternion initialMBoneRot;

    //used to determine whether barks should play
    private bool waitToStartNarr = false;

    //stores whether the player has rotated an incorrect bone sufficiently to be told it is incorrect
    private bool suffRot = false;

    //the name of the current comparison bone
    private string boneTitle;

    //reference to the bonepile quest
    private QuestNode bpile;

    private void Start()
    {
        //create the comparison bone
        Instantiate(BoneInteractable.currBone.answerBone, viewer.transform);
        //set bone details
        viewer.GetComponent<BoneChecker>().isCorrect = BoneInteractable.currBone.isCorrect;
        boneTitle = BoneInteractable.currBone.boneName;
        boneNameTMP.text = "<color=\"white\">" + BoneInteractable.currBone.boneName + "</color>";
        //set rotation and scale appropriately
        viewer.GetComponent<BoneChecker>().SetBoneRotation(viewer.transform.localEulerAngles);
        viewer.GetComponent<BoneChecker>().SetBoneScale(viewer.transform.localScale);
        // Save the initial rotation
        initialRotation = viewer.transform.rotation;
        initialMBoneRot = mBoneViewer.transform.rotation;
        Quaternion offsetRotation = initialRotation * Quaternion.Euler(0f, 90f, 0f); //rotate it 90 degrees
        viewer.transform.rotation = offsetRotation;
        initialRotation = offsetRotation;
        //when the player closes the scene, update the quest
        LoadGUIManager.loadGUIManager.SubtoUnload(BoneInteractable.currBone.UpdateQuest);
        bpile = QuestManager.questManager.GETNode("bonepile");
    }

    //play a bark after a passed amount of time
    private IEnumerator PlayAfterTime(Narration.Narration narr, float time)
    {
        AudioListener.pause = false;
        waitToStartNarr = true;
        yield return new WaitForSeconds(time);
        narr.Begin();
        yield return new WaitForSeconds(Mathf.Max(0, timeBetweenBarks - time));
        waitToStartNarr = false;
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
        float playerRotationAmountMBone = Quaternion.Angle(initialMBoneRot, mBoneViewer.transform.rotation);

        //If it is correct
        if (isCorrect) {
            //player rotated it at all
            if (playerRotationAmount >1f || playerRotationAmountMBone > 1f) 
            {
                //and the rotation is off by 5 degrees or less
                if (yDiff < 5f && FlipMatches)
                {
                    resultText.text = "That's it! Your bone is the distal end of a femur.";
                    bpile.AddCount(0);
                    QuestManager.questManager.GETNode("MainQuest").UnlockUpdate(0);
                    StopAllCoroutines();
                    this.enabled = false;

                } else
                {
                    if (!SoundManager.soundManager.narrator.isPlaying && !waitToStartNarr)
                    {
                        StartCoroutine(PlayAfterTime(correctBarks[Random.Range(0, correctBarks.Count)], 2));
                    }
                    resultText.text = "This looks like the right bone, but not quite the right position.";
                }
                   
            } else    
            {
                // player hasn't rotated it at all
                resultText.text = "Rotate the bone to see if it matches yours.";
            }

        } else {
            //incorrect bone, but the player has rotated it sufficiently
            if (playerRotationAmount > 120f || suffRot || playerRotationAmountMBone > 120f)
            {
                resultText.text = "Hmm, this doesn't seem to be the right bone. ";
                if (!SoundManager.soundManager.narrator.isPlaying && !waitToStartNarr)
                {
                    StartCoroutine(PlayAfterTime(incorrectBarks[Random.Range(0, incorrectBarks.Count)], 0));
                }
                if (BoneInteractable.currBone != null)
                {
                    BoneInteractable.currBone.sendUpdate = true;
                }
                suffRot = true;
            }
            else
            {
                resultText.text = "Rotate the bone to see if it matches yours.";
            }
        }
        //let the player know they've already seen this bone
        if (BoneInteractable.currBone != null && bpile.isUpdateUnlocked(BoneInteractable.currBone.boneName))
        {
            resultText.text = "You've already checked, your bone is not a " + BoneInteractable.currBone.boneName+"!";
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
