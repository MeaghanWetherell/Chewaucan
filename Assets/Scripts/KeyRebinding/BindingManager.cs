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
        //singleton
        public static BindingManager bindingManager;

        [Tooltip("Main input action asset for the project")]
        public InputActionAsset asset;

        //all the maps in our InputActionAsset
        [NonSerialized]public List<InputActionMap> maps = new List<InputActionMap>();

        //invoked when we change a binding
        [NonSerialized]public UnityEvent bindChange = new UnityEvent();

        [Tooltip("File name to save to (NOT A FULL PATH, no file extension)")]public String saveFileName;

        //store binding overrides. keys are in the format action map name+action name+bid index concatenated with no spaces
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
            bindingManager = this;
            DontDestroyOnLoad(this.gameObject);
            SaveHandler.saveHandler.subSettingToSave(Save);
            SaveHandler.saveHandler.subSettingToLoad(Load);
        }

        //for each binding override in our dictionary, apply that override
        private void setBinds()
        {
            foreach (InputActionMap map in maps)
            {
                foreach (InputAction action in map)
                {
                    for (int i = 0; i < action.bindings.Count; i++)
                    {
                        if (binds.ContainsKey(action.actionMap + action.name + i))
                        {
                            action.ApplyBindingOverride(i, binds[action.actionMap + action.name + i]);
                        }
                    }
                }
            }
        }

        //load existing binding overrides from file
        private void Load(string path)
        {
            try
            {
                binds = JsonSerializer.Deserialize<Dictionary<String, String>>(
                    File.ReadAllText(path+"/" + saveFileName + ".json"));
            } catch(IOException){}
            setBinds();
        }

        //save overrides dictionary to file
        private void Save(string path)
        {
            string bindJson = JsonSerializer.Serialize(binds);
            File.WriteAllText(path+"/" + saveFileName + ".json", bindJson);
        }

        //get the display string for a particular binding
        public string GetBind(string controlName, int controlIndex = 0)
        {
            foreach (InputActionMap map in maps)
            {
                foreach (InputAction action in map)
                {
                    if (action.name.Equals(controlName))
                        return action.bindings[controlIndex].ToDisplayString();
                }
            }
            return null;
        }

        //reset all the binding overrides to defaults
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
            string overridePath = bind.bindings[index].overridePath;
            //if a bind for this action has already been saved in the manager, change it in the manager to the current binding
            if (binds.ContainsKey(bind.actionMap + bind.name + index))
            {
                binds[bind.actionMap + bind.name + index] = overridePath;
            }
            //otherwise, save it to the manager
            else
            {
                binds.Add(bind.actionMap+bind.name+index, overridePath);
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
