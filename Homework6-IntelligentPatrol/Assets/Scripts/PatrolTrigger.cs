using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider collider) {
        // 如果玩家进入巡逻兵感知范围
        if (collider.gameObject.tag == "Player") {
            // 设置巡逻兵正在追踪玩家，设置追踪的玩家对象
            this.gameObject.transform.parent.GetComponent<PatrolData>().isTrackPlayer = true;
            this.gameObject.transform.parent.GetComponent<PatrolData>().trackPlayer = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider) {
        // 如果玩家退出巡逻兵感知范围
        if (collider.gameObject.tag == "Player") {
            // 设置巡逻兵不在追踪玩家，设置追踪的玩家对象为空
            this.gameObject.transform.parent.GetComponent<PatrolData>().isTrackPlayer = false;
            this.gameObject.transform.parent.GetComponent<PatrolData>().trackPlayer = null;
        }
    }
}
