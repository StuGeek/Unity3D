using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolData : MonoBehaviour {
    public int patrolAreaId;  // 巡逻兵所在区域序号
    public bool isTrackPlayer = false;  // 是否跟随玩家
    public GameObject trackPlayer;  // 追踪的玩家对象
    public Vector3 startPos;  // 巡逻兵初始位置     
}
