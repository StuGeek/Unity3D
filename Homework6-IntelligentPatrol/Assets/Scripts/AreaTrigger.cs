using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour {
    public int areaId;  // 区域序号
    MainController mainController;
    private void Start() {
        mainController = Director.GetInstance().mainController as MainController;
    }

    void OnTriggerEnter(Collider collider) {
        // 记录玩家进入区域的序号
        if (collider.gameObject.tag == "Player") {
            mainController.SetPlayerAreaId(areaId);
        }
    }
}
