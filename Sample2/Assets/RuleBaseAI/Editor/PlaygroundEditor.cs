using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Playground))]
[CanEditMultipleObjects] // 複数オブジェクトの同時編集に対応させるオプション
public class PlayGroundEditor : Editor {
	// EnumFlagsFieldで使用
	enum ExampleFlagsEnum
		{
			None = 0, // Custom name for "Nothing" option
			A = 1 << 0,
			B = 1 << 1,
			AB = A | B, // Combination of two flags
			C = 1 << 2,
			All = ~0, // Custom name for "Everything" option
		}

	// EnumPopupで使用
	enum OPTIONS
	{
		CUBE = 0,
		SPHERE = 1,
		PLANE = 2
	}

	Bounds b;
	BoundsInt bi;
	Color co;
	AnimationCurve curve;
	GUIContent guicontent;
	ExampleFlagsEnum　m_flag;
	OPTIONS opt;

	public void OnEnable() {
		// Bounds:座標とサイズの二つの三次元要素で構成される
		b = new Bounds(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
		// BoundsInt:
		// Positionの座標3つとSizeの値3つで構成される
		bi = new BoundsInt( -3, -3, 0, 6, 6, 1);
		// Color
		co = new Color();
		// AnimationCurve:
		// 曲線を表示する
		curve = AnimationCurve.Linear( 0.0f,0.0f,60.0f,1.0f );
		// DropDownButton:
		// クリックした時の動作をif文を組むことで設定できる？
		// Windowの設定が面倒なので後日試す
		guicontent = new GUIContent("click me!", "click meeeeeee!");
	}

	public override void OnInspectorGUI() {
		
		// 戻り値を指定しないで使用することもできるがその場合は要素とシリアル化オブジェクトを
		// serializedObject.FindProperty("プロパティ")で紐付ける必要がある
		b = EditorGUILayout.BoundsField("BoundsField", b);
		bi = EditorGUILayout.BoundsIntField("BoundsInt", bi);
		co = EditorGUILayout.ColorField("Color", co);
		curve = EditorGUILayout.CurveField("AnimationCurve", curve);
		EditorGUILayout.DropdownButton(guicontent, new FocusType());
		m_flag = (ExampleFlagsEnum)EditorGUILayout.EnumFlagsField(m_flag);
        opt = (OPTIONS)EditorGUILayout.EnumPopup("Primitive to create:", opt);
	}

}
