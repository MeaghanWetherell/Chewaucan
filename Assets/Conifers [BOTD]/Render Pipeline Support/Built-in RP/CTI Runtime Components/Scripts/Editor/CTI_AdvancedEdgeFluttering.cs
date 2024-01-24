using UnityEngine;
using System.Collections;
using UnityEditor;

public class CtiAdvancedEdgeFluttering: MaterialPropertyDrawer {

	override public void OnGUI (Rect position, MaterialProperty prop, string label, MaterialEditor editor) {
		Material material = editor.target as Material;
		if (material.GetFloat("_EnableAdvancedEdgeBending") != 0 ) {
			float halfHeight = (position.height * 0.5f - 4);
			Rect pos1 = new Rect(position.position.x, position.position.y, position.width, halfHeight);
			Rect pos2 = new Rect(position.position.x, position.position.y + halfHeight + 2, position.width, halfHeight);

			Vector4 vec4Value = prop.vectorValue;
			vec4Value.x = EditorGUI.Slider(pos1, "    Strength", vec4Value.x, 0.0f, 5.0f);
			vec4Value.y = EditorGUI.Slider(pos2, "    Speed", vec4Value.y, 0.0f, 20.0f);
			prop.vectorValue = vec4Value;
		}
	}

	public override float GetPropertyHeight (MaterialProperty prop, string label, MaterialEditor editor) {
		Material material = editor.target as Material;
		if (material.GetFloat("_EnableAdvancedEdgeBending") != 0 ) {
			return base.GetPropertyHeight (prop, label, editor) * 2.0f + 6;
		}
		else {
			return 0.0f;
		}
	}
}
