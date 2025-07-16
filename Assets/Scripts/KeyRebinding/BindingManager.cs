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
                Destroy(gameObject);
                return;
            }
            foreach (InputActionMap map in asset.actionMaps)
            {
                maps.Add(map);
            }
            setBinds();
            bindingManager = this;
            DontDestroyOnLoad(this.gameObject);
            SaveHandler.saveHandler.subSettingToSave(Save);
            SaveHandler.saveHandler.subSettingToLoad(Load);
        }

        private void setBinds()
        {
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

        private void Load(string path)
        {
            try
            {
                binds = JsonSerializer.Deserialize<Dictionary<String, String>>(
                    File.ReadAllText(path+"/" + saveFileName + ".json"));
            } catch(IOException){}
            setBinds();
        }

        private void Save(string path)
        {
            string bindJson = JsonSerializer.Serialize(binds);
            File.WriteAllText(path+"/" + saveFileName + ".json", bindJson);
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
            //if a bind for this action has already been saved in the manager, change it in the manager to the current binding
            if (binds.ContainsKey(bind.actionMap + bind.name + index))
            {
                binds[bind.actionMap + bind.name + index] = bind.bindings[index].overridePath;
            }
            //otherwise, save it to the manager
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
                            //if this map+action+index combination is the one we rebound, do nothing
                            if(action == bind && i == index)
                                continue;
                            //if it has been overridden to the same key as the new binding, overwrite it to none
                            if (!string.IsNullOrEmpty(action.bindings[i].overridePath))
                            {
                                if (action.bindings[i].overridePath.Equals(bind.bindings[index].overridePath))
                                {
                                    action.ApplyBindingOverride(i, "None");
                                    binds[action.actionMap + action.name + i] = action.bindings[i].overridePath;
                                }
                            }
                            //if it has not been overridden and its default key is the same as the new binding, overwrite it to none
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
