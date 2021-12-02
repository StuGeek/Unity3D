using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysisPlayDiskAction : SSAction {
    float speed; // 水平速度
    Vector3 direction; // 飞行方向

    public static PhysisPlayDiskAction GetSSAction(Vector3 direction, float speed) {
        PhysisPlayDiskAction action = ScriptableObject.CreateInstance<PhysisPlayDiskAction>();
        action.speed = speed;
        action.direction = direction;
        return action;
    }

    public override void Start() {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        // 水平初速度
        gameObject.GetComponent<Rigidbody>().velocity = speed * direction;
    }

    public override void Update() {
        // 飞碟到达底部动作结束，回调
        if (this.transform.position.y < -5) {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }
    }
}
