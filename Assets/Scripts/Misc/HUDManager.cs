using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager hudManager;
        
        public List<GameObject> pins;

        public TextMeshProUGUI messageText;

        public GameObject subtitleBG;

        public GameObject skipBG;
        public RectTransform minimap;

        public RectTransform astrolabeUI;

        public RectTransform questJournalIcon;

        public RectTransform mastoBoneRect;

        public GameObject hudOverlayPrefab;

        public GameObject pinField;

        private void Awake()
        {
            hudManager = this;
            //minimap.anchoredPosition = new Vector2(150, -150);
            //mastoBoneRect.anchoredPosition = new Vector2(-694, -48);
            StartCoroutine(WaitForQuestLoad());
        }

        private void Start()
        {
            PauseCallback.pauseManager.SubscribeToPause(DeActivate);
            PauseCallback.pauseManager.SubscribeToResume(Activate);
        }

        private void OnDestroy()
        {
            PauseCallback.pauseManager.UnsubToPause(DeActivate);
            PauseCallback.pauseManager.UnsubToResume(Activate);
        }

        private void OnEnable()
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(false);
        }

        public void DisplayMessageToHUD(String message)
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(true);
            messageText.text = message;
        }

        public void CreateFadingOverlay(Sprite image, float timeOnScreen)
        {
            GameObject overlay = Instantiate(hudOverlayPrefab, transform);
            overlay.transform.SetAsFirstSibling();
            Image sp = overlay.GetComponent<Image>();
            sp.sprite = image;
            StartCoroutine(FadeOverlay(sp, timeOnScreen));
        }

        private IEnumerator FadeOverlay(Image img, float fadeTime)
        {
            float curTime = 0;
            float alpha = SinInterpVal(curTime, fadeTime);
            alpha = Mathf.Clamp(alpha, 0, 1);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            while (curTime < fadeTime)
            {
                if (!PauseCallback.pauseManager.isPaused)
                {
                    curTime += Time.deltaTime;
                    alpha = SinInterpVal(curTime, fadeTime);
                    alpha = Mathf.Clamp(alpha, 0, 1);
                    img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
                }
                yield return new WaitForSeconds(0);
            }
            while (alpha > 0)
            {
                if (!PauseCallback.pauseManager.isPaused)
                {
                    curTime += Time.deltaTime;
                    alpha = SinInterpVal(curTime, fadeTime);
                    alpha = Mathf.Clamp(alpha, 0, 1);
                    img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
                }
                yield return new WaitForSeconds(0);
            }
            Destroy(img.gameObject);
        }

        private float SinInterpVal(float curTime, float endTime)
        {
            float val = 2f / 3;
            val *= Mathf.PI;
            val *= curTime;
            val+= Mathf.PI/2;
            val = Mathf.Sin(val);
            val *= 0.4f;
            float val2 = -0.025f*curTime;
            val2 = Mathf.Pow(System.MathF.E, val2);
            val *= val2;
            val += 0.4f;
            val2 = 0.5f / (endTime*1.5f);
            val2 *= curTime;
            val -= val2;
            return val;
        }

        public void DisplayMessageToHUDForTime(String message, float time)
        {
            DisplayMessageToHUD(message);
            StartCoroutine(CloseMessageAfterTime(time));
        }

        private IEnumerator CloseMessageAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            CloseMessage();
        }

        public void CloseMessage()
        {
            messageText.gameObject.transform.parent.gameObject.SetActive(false);
        }

        //if the quest manager hasn't been initialized yet, wait a frame to set up quest pins
        private IEnumerator WaitForQuestLoad()
        {
            while (QuestManager.questManager == null)
            {
                yield return new WaitForSeconds(0);
            }
            CheckAstrolabeOn();
            ResetPins();
            if (!QuestManager.questManager.hasQuests())
            {
                questJournalIcon.gameObject.SetActive(false);
                QuestManager.questManager.onQuestCreated.AddListener(ShowQJIcon);
            }
        }

        private void CheckAstrolabeOn()
        {
            QuestNode unlockQuest = QuestManager.questManager.GETNode("bonepile");
            if (unlockQuest == null || !unlockQuest.isComplete)
            {
                astrolabeUI.gameObject.SetActive(false);
            }
        }

        private void ShowQJIcon(QuestNode n)
        {
            questJournalIcon.gameObject.SetActive(true);
            QuestManager.questManager.onQuestCreated.RemoveListener(ShowQJIcon);
        }

        public void ResetPins()
        {
            QuestNode[] pinNodes = QuestManager.questManager.GETPinNodes();
            pinNodes.InsertionSort();
            if (pinNodes[0] == null)
            {
                pinField.SetActive(false);
                return;
            }
            pinField.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                QuestNode cur = pinNodes[i];
                Transform pin = pins[i].transform;
                if (cur == null)
                {
                    pin.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    pin.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    pin.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                }
                else
                {
                    pin.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cur.name;
                    //removed so objectives don't get compressed
                    //pin.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = cur.shortDescription;
                    TextMeshProUGUI text = pin.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
                    text.text = "";
                    for (int j = 0; j < cur.objectives.Count; j++)
                    {
                        text.text += +cur.counts[j] + "/" + cur.requiredCounts[j] + " " + cur.objectives[j]+"\n";
                    }
                }
            }
        }

        private void Activate()
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            StartCoroutine(WaitForQuestLoad());
        }

        private void DeActivate()
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}
