using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace KeyRebinding
{
    public class BindingManager : MonoBehaviour
    {
        public static BindingManager bindingManager;

        public InputActionAsset asset;

        [NonSerialized]public List<InputActionMap> maps = new List<InputActionMap>();

        [NonSerialized]public UnityEvent bindChange = new UnityEvent();

        [Tooltip("File name to save to (NOT A FULL PATH, no file extension)")]public String saveFileName;

        private Dictionary<String, String> binds = new Dictionary<string, string>();
        private void Awake()
        {
            if (bindingManager != null)
            {
                Debug.LogError("Loaded persistent object "+gameObject.name+" twice!");
                Destroy(bindingManager.gameObject);
            }
            bindingManager = this;
            DontDestroyOnLoad(this.gameObject);
            foreach (InputActionMap map in asset.actionMaps)
            {
                maps.Add(map);
            }
            try
            {
                binds = JsonSerializer.Deserialize<Dictionary<String, String>>(
                    File.ReadAllText("Saves/" + saveFileName + ".json"));
            } catch(IOException){}
            foreach (InputActionMap map in maps)
            {
                foreach (InputAction action in map)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        if (binds.ContainsKey(action.actionMap + action.name + i))
                        {
                            action.ApplyBindingOverride(i, binds[action.actionMap + action.name + i]);
                        }
                    }
                }
            }
        }

        private void OnDisable()
        {
            string bindJson = JsonSerializer.Serialize(binds);
            Directory.CreateDirectory("Saves");
            File.WriteAllText("Saves/" + saveFileName + ".json", bindJson);
        }

        public void ResetBinds()
        {
            foreach (InputActionMap map in maps)
            {
                foreach (InputAction action in map)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(action.bindings[i].overridePath))
                        {
                            action.ApplyBindingOverride(i, action.bindings[i].path);
                            binds[action.actionMap + action.name + i] = action.bindings[i].path;
                        }
                    }
                }
            }
            bindChange.Invoke();
        }

        public void SetBind(InputAction bind, int index)
        {
            if (binds.ContainsKey(bind.actionMap + bind.name + index))
            {
                binds[bind.actionMap + bind.name + index] = bind.bindings[index].overridePath;
            }
            else
            {
                binds.Add(bind.actionMap+bind.name+index, bind.bindings[index].overridePath);
            }

            foreach (InputActionMap map in maps)
            {
                if (map == bind.actionMap)
                {
                    foreach (InputAction action in map)
                    {
                        for (int i = 0; i < action.bindings.Count; i++)
                        {
                            if(action == bind && i == index)
                                continue;
                            if (!string.IsNullOrEmpty(action.bindings[i].overridePath))
                            {
                                if (action.bindings[i].overridePath.Equals(bind.bindings[index].overridePath))
                                {
                                    action.ApplyBindingOverride(i, "None");
                                    binds[action.actionMap + action.name + i] = action.bindings[i].overridePath;
                                }
                            }
                            else
                            {
                                if (action.bindings[i].path.Equals(bind.bindings[index].overridePath) &&
                                    !string.IsNullOrEmpty(action.bindings[i].path))
                                {
                                    action.ApplyBindingOverride(i, "None");
                                    if(!binds.ContainsKey(action.actionMap+action.name+i))
                                        binds.Add(action.actionMap+action.name+i, action.bindings[i].overridePath);
                                }
                            }
                        }
                        
                    }
                    break;
                }
            }
            bindChange.Invoke();
        }
    }
}