using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCPlayDiskAction : SSAction {
    float gravity; // 垂直速度
    float speed; // 水平速度
    Vector3 direction;  // 方向
    float time; // 时间

    public static CCPlayDiskAction GetSSAction(Vector3 direction, float speed) {
        CCPlayDiskAction action = ScriptableObject.CreateInstance<CCPlayDiskAction>();
        action.gravity = 9.8f;
        action.time = 0;
        action.speed = speed;
        action.direction = direction;
        return action;
    }

    public override void Start() {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void Update() {
        time += Time.deltaTime;
        transform.Translate(Vector3.down * gravity * time * Time.deltaTime);
        transform.Translate(direction * speed * Time.deltaTime);
        // 飞碟到达底部动作结束，回调
        if (this.transform.position.y < -5) {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }
    }
}
