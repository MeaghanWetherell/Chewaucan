using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using ScriptTags;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DateRock : MonoBehaviour
{
    [Tooltip("Amount of dating 'points' this rock provides. Will be reversed automatically if it is at the wrong height for the current level")]
    public float myPoints;

    [Tooltip("Ref to the audiosource that should play when stepped on")]
    public AudioSource mySE;

    //ref to the active course manager, set automatically by course manager
    [NonSerialized]public CourseManager manager;

    //dunno why this is duplicated from the scale randomizer instead of just using that component on the date rocks, I'm going to assume there's a reason
    [Tooltip("Min x scale for random scale")]public float scaleMinX;

    [Tooltip("Max x scale for random scale")]public float scaleMaxX;

    [Tooltip("Min y scale for random scale")]public float scaleMinY;

    [Tooltip("Max y scale for random scale")]public float scaleMaxY;

    [Tooltip("Min z scale for random scale")]public float scaleMinZ;

    [Tooltip("Max z scale for random scale")]public float scaleMaxZ;

    //color of the text that displays on dating a rock
    [NonSerialized]public string dateTextColor = "white";

    //the date that corresponds to this rock, set automatically on level start
    [NonSerialized]public string date;

    [Tooltip("Overrides the date setting from the manager.")]public int overrideDateMin;

    [Tooltip("Overrides the date setting from the manager.")]public int overrideDateMax;

    [Tooltip("Degree to which to quiet the background music while the sound effect plays")]public float BGMAttenuation;

    //prefab for the text that appears when stepping on the rock
    private static GameObject dateText;

    //ref to the hud
    private static GameObject HUD;

    //sets the objects scale randomly and sets static variables if unitialized
    private void Awake()
    {
        float scaleX = Random.Range(scaleMinX, scaleMaxX);
        float scaleY = Random.Range(scaleMinY, scaleMaxY);
        float scaleZ = Random.Range(scaleMinZ, scaleMaxZ);
        Vector3 scale = transform.parent.localScale;
        Vector3 newScale = new Vector3(scale.x * scaleX, scale.y * scaleY, scale.z * scaleZ);
        transform.parent.localScale = newScale;
        FillStatics();
    }

    //if this scripts static variables are unitialized, initialize them
    private void FillStatics()
    {
        if (dateText == null)
            dateText = Resources.Load<GameObject>("TextWithGravity");
        if (HUD == null)
            HUD = GameObject.Find("HUD");
    }

    //when the player steps on the date rock
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && manager != null && manager.active)
        {
            //play the step sound
            mySE.Play();
            if (!SoundManager.soundManager.IsMuted(2))
            {
                StartCoroutine(SoundManager.soundManager.QuietBGMUntilDone(mySE, BGMAttenuation));
            }
            //prepare to disable
            StartCoroutine(DisableAfterTime(0.5f));
            GetComponent<BoxCollider>().enabled = false;
            //ensure statics are filled
            FillStatics();
            //display the date to the player
            GameObject text = Instantiate(dateText, HUD.transform);
            Gravity grav = text.GetComponent<Gravity>();
            grav.accel = grav.accel * 2 / 3;
            RectTransform rect = text.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 80);
            rect.anchoredPosition = new Vector3(0, 88, 0);
            string dateTextA = "<color=";
            if (dateTextColor[0].Equals('#'))
            {
                dateTextA += dateTextColor;
            }
            else
            {
                dateTextA += "\"" + dateTextColor + "\"";
            }
            dateTextA += ">" + date;
            if (myPoints < 0)
            {
                dateTextA += "! Bad date!";
                
            }
            else if (transform.position.y >= manager.yMax ||
                transform.position.y <= manager.yMin)
            {
                myPoints *= -1;
                rect.sizeDelta = new Vector2(400, 160);
                dateTextA += "! Bad date! This rock is from the wrong level!";
            }
            else
            {
                dateTextA += " Years Ago!";
            }
            dateTextA += "</color>";
            text.GetComponent<TextMeshProUGUI>().text = dateTextA;
            //add points
            manager.AddPoints(myPoints);
        }
    }

    //recreates the rock when the game finishes
    private void ReenableOnFinish()
    {
        transform.parent.gameObject.SetActive(true);
        manager.Stopped.RemoveListener(ReenableOnFinish);
        GetComponent<BoxCollider>().enabled = true;
    }

    //disables the rock after the passed time
    private IEnumerator DisableAfterTime(float secs)
    {
        yield return new WaitForSeconds(secs);
        manager.Stopped.AddListener(ReenableOnFinish);
        transform.parent.gameObject.SetActive(false);
    }
}
