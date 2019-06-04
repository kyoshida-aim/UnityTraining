using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ControlTrackTest : MonoBehaviour, ITimeControl {
	[SerializeField] Vector3 axis = Vector3.up;
	[SerializeField] float speed = 1;
    public void SetTime(double time)
    {
        // 時間から値が一意に決まる式の形にする
        transform.localEulerAngles = Quaternion.AngleAxis((float) time * speed, axis).eulerAngles;
    }
	// クリップ開始時に呼ばれる
	public void OnControlTimeStart ()
	{
	}

	// クリップから抜ける時に呼ばれる
	public void OnControlTimeStop ()
	{
	}
}
