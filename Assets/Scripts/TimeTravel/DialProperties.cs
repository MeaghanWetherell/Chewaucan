using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// helper class to be used for saving the dial progress to json file
public class DialProgress
{
    public float A_progress;
    public float A_rotation;
    public float G_progress;
    public float G_rotation;
    public float B_progress;
    public float B_rotation;
}

public class DialProperties : MonoBehaviour
{
    public enum Dial
    {
        ARCHEOLOGY,
        GEOLOGY,
        BIOLOGY
    }

    public static DialProperties instance { get; private set; }

    //the rotation values can be weird in the editor itself, so this keeps track
    // of how far the hand is rotated instead

    private float ArcheoProgress;
    private float GeologyProgress;
    private float BiologyProgress;

    // the rotation offsets of each dial hand, set them so the hands don't overlap
    public float archeoOffset;
    public float geologyOffset;
    public float biologyOffset;

    string saveFilePath;

    [SerializeField] private RectTransform archeoTransform;
    [SerializeField] private RectTransform geologyTransform;
    [SerializeField] private RectTransform biologyTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            instance = null;
        }
        instance = this;

        instance.saveFilePath = Application.persistentDataPath + "/DialProgess.json";

        if (!File.Exists(saveFilePath))
        {
            instance.ArcheoProgress = 0f;
            instance.GeologyProgress = 0f;
            instance.BiologyProgress = 0f;
            Debug.Log("SAVE: "+saveFilePath);
            instance.SaveDialProgress();
        }
        else
        {
            instance.LoadDialProgress();
        }
    }

    public float getCurRotationAmount(Dial dial) {
        instance.LoadDialProgress();
        switch (dial)
        {
            case Dial.ARCHEOLOGY:
                return instance.ArcheoProgress;
            case Dial.GEOLOGY:
                return instance.GeologyProgress;
            case Dial.BIOLOGY:
                return instance.BiologyProgress;
            default:
                return -1000f;
        } 
    }

    public void changeCurRotationAmount(float n, Dial dial)
    {
        n = Mathf.Abs(n);
        if (dial == Dial.ARCHEOLOGY)
        {
            instance.ArcheoProgress += n;
        }
        else if (dial == Dial.GEOLOGY)
        {
            instance.GeologyProgress += n;
        }
        else if (dial == Dial.BIOLOGY)
        {
            instance.BiologyProgress += n;
        }

        instance.SaveDialProgress();
    }

    public void LoadDialProgress()
    {
        if (File.Exists(saveFilePath))
        {
            string loadDialProgress = File.ReadAllText(saveFilePath);
            DialProgress dp = JsonUtility.FromJson<DialProgress>(loadDialProgress);
            instance.ArcheoProgress = dp.A_progress;
            instance.archeoTransform.rotation = Quaternion.Euler(SetDialRotation(instance.archeoTransform, dp.A_rotation));
            instance.GeologyProgress = dp.G_progress;
            instance.geologyTransform.rotation = Quaternion.Euler(SetDialRotation(instance.geologyTransform, dp.G_rotation));
            instance.BiologyProgress = dp.B_progress;
            instance.biologyTransform.rotation = Quaternion.Euler(SetDialRotation(instance.biologyTransform, dp.B_rotation));
        }
    }

    private Vector3 SetDialRotation(RectTransform r, float z)
    {
        Vector3 q = Quaternion.Euler(r.rotation.x, r.rotation.y, z).eulerAngles;

        return q;
    }

    public void SaveDialProgress()
    {
        DialProgress dp = new DialProgress();
        dp.A_progress = instance.ArcheoProgress;
        dp.A_rotation = instance.archeoTransform.rotation.eulerAngles.z;
        dp.G_progress = instance.GeologyProgress;
        dp.G_rotation = instance.geologyTransform.rotation.eulerAngles.z;
        dp.B_progress = instance.BiologyProgress;
        dp.B_rotation = instance.biologyTransform.rotation.eulerAngles.z;
        string saveDialProgress = JsonUtility.ToJson(dp);
        File.WriteAllText(instance.saveFilePath, saveDialProgress);
    }

    public void DeleteProgress()
    {
        if (File.Exists(instance.saveFilePath))
        {
            File.Delete(instance.saveFilePath);
        }
    }
}
