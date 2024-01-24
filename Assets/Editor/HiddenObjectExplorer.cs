using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class HiddenObjectExplorer : EditorWindow
{
	[MenuItem("Tools/HiddenObjectExplorer")]
    static void Init()
    {
		GetWindow<HiddenObjectExplorer>();
    }
    List<GameObject> _mObjects = new List<GameObject>();
	Vector2 _scrollPos = Vector2.zero;
	
	void OnEnable()
	{
		FindObjects();
	}
	
	void FindObjects()
	{
		var objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		_mObjects.Clear();
		foreach(var o in objs)
		{
			var go = o.transform.root.gameObject;
			if (!_mObjects.Contains(go))
				_mObjects.Add(go);
		}
	}
	void FindObjectsAll()
	{
		var objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		_mObjects.Clear();
		_mObjects.AddRange(objs);
	}
	
	HideFlags HideFlagsButton(string aTitle, HideFlags aFlags, HideFlags aValue)
	{
		if(GUILayout.Toggle((aFlags & aValue) > 0, aTitle, "Button"))
			aFlags |= aValue;
		else
			aFlags &= ~aValue;
		return aFlags;
	}
	
    void OnGUI()
    {
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("find top level"))
		{
			FindObjects();
		}
		if (GUILayout.Button("find ALL object"))
		{
			FindObjectsAll();
		}
		GUILayout.EndHorizontal();
		_scrollPos = GUILayout.BeginScrollView(_scrollPos);
		for(int i = 0; i < _mObjects.Count; i++)
		{
			GameObject o = _mObjects[i];
			if (o == null)
				continue;
			GUILayout.BeginHorizontal();
			EditorGUILayout.ObjectField(o.name, o,typeof(GameObject),true);
			HideFlags flags = o.hideFlags;
			flags = HideFlagsButton("HideInHierarchy",flags, HideFlags.HideInHierarchy);
			flags = HideFlagsButton("HideInInspector",flags, HideFlags.HideInInspector);
			flags = HideFlagsButton("DontSave",flags, HideFlags.DontSave);
			flags = HideFlagsButton("NotEditable",flags, HideFlags.NotEditable);
			o.hideFlags = flags;
			GUILayout.Label(""+((int)flags),GUILayout.Width(20));
			GUILayout.Space(20);
			if (GUILayout.Button("DELETE"))
			{
				DestroyImmediate(o);
				FindObjects();
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(20);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
    }
}
