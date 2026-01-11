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

    [Tooltip("Course manger will set this automatically")]public string dateTextColor;

    [Tooltip("Course manger will set this automatically")]public string date;

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
        if (other.GetComponent<Player>() != null)
        {
            manager.AddPoints(myPoints);
            mySE.Play();
            StartCoroutine(DestroyAfterTime(0.5f));
            GetComponent<BoxCollider>().enabled = false;
            FillStatics();
            GameObject text = Instantiate(dateText, HUD.transform);
            RectTransform rect = text.GetComponent<RectTransform>();
            //rect.sizeDelta = new Vector2(150, 40);
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
            else
            {
                dateTextA += " Years Ago!";
            }
            dateTextA += "</color>";
            text.GetComponent<TextMeshProUGUI>().text = dateTextA;
        }
    }

    private IEnumerator DestroyAfterTime(float secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(transform.parent.gameObject);
    }
}
