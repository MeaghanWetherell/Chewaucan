using System.Collections;
using System.Collections.Generic;
using KeyRebinding;
using UnityEngine;

public class ResetBinds : MonoBehaviour
{
    public void OnClick()
    {
        BindingManager.bindingManager.ResetBinds();
    }
}
