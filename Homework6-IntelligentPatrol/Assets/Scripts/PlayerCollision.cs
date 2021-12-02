using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    void OnCollisionEnter(Collision collider) {
        // 当和巡逻兵相撞的是玩家，那么调整玩家状态，触发玩家被抓事件，如果碰撞是墙，那么改换方向移动
        if (collider.gameObject.tag == "Player") {
            collider.gameObject.GetComponent<Animator>().SetTrigger("death");
            this.GetComponent<Animator>().SetTrigger("attack");
            Singleton<GameEventManager>.Instance.PlayerGetCaught();
        }
    }
}
