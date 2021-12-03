using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCollision : MonoBehaviour {
    void OnTriggerEnter(Collider collider) {
        // 如果是玩家碰到还在显示中的宝箱
        if (collider.gameObject.tag == "Player" && this.gameObject.activeSelf) {
            this.gameObject.SetActive(false);
            // 减少宝箱数量
            Singleton<GameEventManager>.Instance.DecreaseTreasureNum();
        }
    }
}
