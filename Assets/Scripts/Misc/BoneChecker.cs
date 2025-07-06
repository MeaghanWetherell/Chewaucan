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

    public GameObject viewer; //The comparison bone from the Bone Pile Quest
    
    public GameObject mBoneViewer; //the mastodon bone
    
    public TextMeshProUGUI text;

    private Quaternion initialRotation; // store the starting rotation

    private void Start()
    {
        viewer.transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
        viewer.GetComponent<BoneChecker>().SetBoneScale(viewer.transform.localScale);
        // Save the initial rotation
        initialRotation = viewer.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        var rotationDifference = (viewer.transform.rotation.eulerAngles - mBoneViewer.transform.rotation.eulerAngles).magnitude;
        float playerRotationAmount = Quaternion.Angle(initialRotation, viewer.transform.rotation);

        //If it is correct
        if (isCorrect) {

            //player rotated it at all
            if (playerRotationAmount >1f) 
            {
                //and the rotation is right
                if (rotationDifference < 10f)
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
            if (playerRotationAmount > 30f)
            {
                text.text = "Hmm, this doesn't seem to be the right bone. ";
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

}
