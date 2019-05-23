using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorExtensionTest : MonoBehaviour {

	public const int RangeTest1 = 0;
	public const int RangeTest2 = 1000;

	[SerializeField, Range(RangeTest1, RangeTest2)] private int HP;

	public int Hp {
		get { return HP; }
		set { HP = Mathf.Clamp(value, RangeTest1, RangeTest2);}
	}
}
