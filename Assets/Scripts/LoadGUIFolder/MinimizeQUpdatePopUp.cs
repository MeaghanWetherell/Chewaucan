using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Misc;
using UnityEngine;
using UnityEngine.UI;

public class MinimizeQUpdatePopUp : PopUpOnClick
{
    public RectTransform mainImgRectTransform;

    public float timeToMinimize;

    private bool clicked = false;

    public override void OnClick()
    {
        if (!clicked)
        {
            StartCoroutine(Minimize());
            clicked = true;
            LoadGUIManager.loadGUIManager.RegisterPopUpClose(index);
        }
    }

    private IEnumerator Minimize()
    {
        RectTransform target = HUDManager.hudManager.questJournalIcon;
        if (target != null && target.gameObject.activeSelf && !LoadGUIManager.loadGUIManager.isGUIOpen())
        {
            List<Vector2> scaleFactors = new List<Vector2>();
            List<Vector2> originalPositions = new List<Vector2>();
            foreach (RectTransform child in mainImgRectTransform)
            {
                scaleFactors.Add(new Vector2(child.rect.size.x/mainImgRectTransform.rect.size.x, child.rect.size.y/mainImgRectTransform.rect.size.y));
                originalPositions.Add(child.anchoredPosition);
            }

            Vector2 originalPos = mainImgRectTransform.anchoredPosition;
            Vector2 originalSize = mainImgRectTransform.rect.size;
            Vector2 targetPos = target.anchoredPosition;
            //Debug.Log(targetPos);
            Vector2 targetSize = target.rect.size;
            
            float time = 0;
            while (time < timeToMinimize)
            {
                time += Time.deltaTime;
                float lv = time / timeToMinimize;
                Vector2 lerpPos = Vector2.Lerp(originalPos, targetPos, lv);
                mainImgRectTransform.anchoredPosition = lerpPos;
                Vector2 lv2 = Vector2.Lerp(originalSize, targetSize, lv);
                mainImgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lv2.x);
                mainImgRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lv2.y);
                Vector2 comp = new Vector2(mainImgRectTransform.rect.size.x / originalSize.x,
                    mainImgRectTransform.rect.size.y / originalSize.y);
                for (int i = 0; i < mainImgRectTransform.childCount; i++)
                {
                    RectTransform child = mainImgRectTransform.GetChild(i) as RectTransform;
                    child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,mainImgRectTransform.rect.size.x*scaleFactors[i].x);
                    child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mainImgRectTransform.rect.size.y*scaleFactors[i].y);
                    child.anchoredPosition =
                        new Vector2(comp.x * originalPositions[i].x, comp.y * originalPositions[i].y);
                }
                yield return new WaitForSeconds(0);
            }
        }
        Destroy(gameObject);
    }
}
