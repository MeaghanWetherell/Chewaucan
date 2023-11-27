using UnityEngine;
using System.Collections;
using UnityEditor;

public class CtiDetailsEnum : MaterialPropertyDrawer {

	public enum DetailMode
	{
	    Disabled = 0,
	    Enabled = 1,
	    FadeBaseTextures = 2,
	    SkipBaseTextures = 3
	}

	DetailMode _status;

	override public void OnGUI (Rect position, MaterialProperty prop, string label, MaterialEditor editor)
	{
		
		Material material = editor.target as Material;

		_status = (DetailMode)((int)prop.floatValue);
		_status = (DetailMode)EditorGUI.EnumPopup(position, label, _status);
		prop.floatValue = (float)_status;

		if (prop.floatValue == 0.0f) {
			material.DisableKeyword("GEOM_TYPE_BRANCH");
			material.DisableKeyword("GEOM_TYPE_BRANCH_DETAIL");
			material.DisableKeyword("GEOM_TYPE_FROND");
		}
		else if (prop.floatValue == 1.0f) {
			material.EnableKeyword("GEOM_TYPE_BRANCH");
			material.DisableKeyword("GEOM_TYPE_BRANCH_DETAIL");
			material.DisableKeyword("GEOM_TYPE_FROND");
		}
		else if (prop.floatValue == 2.0f) {
			material.DisableKeyword("GEOM_TYPE_BRANCH");
			material.EnableKeyword("GEOM_TYPE_BRANCH_DETAIL");
			material.DisableKeyword("GEOM_TYPE_FROND");
		}
		else if (prop.floatValue == 3.0f) {
			material.DisableKeyword("GEOM_TYPE_BRANCH");
			material.DisableKeyword("GEOM_TYPE_BRANCH_DETAIL");
			material.EnableKeyword("GEOM_TYPE_FROND");
		}
	}
	public override float GetPropertyHeight (MaterialProperty prop, string label, MaterialEditor editor)
	{
		return base.GetPropertyHeight (prop, label, editor);
	}
}
