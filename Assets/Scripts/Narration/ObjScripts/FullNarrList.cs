using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Narration/list")]
public class FullNarrList : ScriptableObject
{
    public List<Narration.Narration> allNarr;

    public void Clear()
    {
        foreach (Narration.Narration narr in allNarr)
        {
            narr.ResetOnComplete();
        }
    }
}
