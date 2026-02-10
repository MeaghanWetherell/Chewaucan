using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DateRock : MonoBehaviour
{
    public float myPoints;

    public AudioSource mySE;

    public CourseManager manager;

    public float scaleMinX;

    public float scaleMaxX;

    public float scaleMinY;

    public float scaleMaxY;

    public float scaleMinZ;

    public float scaleMaxZ;

    [NonSerialized]public string dateTextColor = "white";

    [NonSerialized]public string date;

    [Tooltip("Overrides the date setting from the manager.")]public int overrideDateMin;

    [Tooltip("Overrides the date setting from the manager.")]public int overrideDateMax;

    private static GameObject dateText;

    private static GameObject HUD;

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

    private void FillStatics()
    {
        if (dateText == null)
            dateText = Resources.Load<GameObject>("TextWithGravity");
        if (HUD == null)
            HUD = GameObject.Find("HUD");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && manager != null && manager.active)
        {
            mySE.Play();
            StartCoroutine(DisableAfterTime(0.5f));
            GetComponent<BoxCollider>().enabled = false;
            FillStatics();
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
            manager.AddPoints(myPoints);
        }
    }

    private void ReenableOnFinish()
    {
        transform.parent.gameObject.SetActive(true);
        manager.Stopped.RemoveListener(ReenableOnFinish);
        GetComponent<BoxCollider>().enabled = true;
    }

    private IEnumerator DisableAfterTime(float secs)
    {
        yield return new WaitForSeconds(secs);
        manager.Stopped.AddListener(ReenableOnFinish);
        transform.parent.gameObject.SetActive(false);
    }
}
