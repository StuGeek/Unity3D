using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTrackAction : SSAction {
    private GameObject trackPlayer;  // 追踪的玩家
    private PatrolData patrol;  // 巡逻兵
    private MainController mainController;

    public override void Start() {
        patrol = this.gameobject.GetComponent<PatrolData>();
        mainController = Director.GetInstance().mainController;
    }

    public override void Update() {
        // 朝玩家方向走
        transform.position = Vector3.MoveTowards(this.transform.position, trackPlayer.transform.position, 2 * Time.deltaTime);
        this.transform.LookAt(trackPlayer.transform.position);

        // 如果玩家和巡逻兵不在同一个区域，或巡逻兵没有感知到玩家
        if (mainController.GetPlayerAreaId() != patrol.patrolAreaId || !patrol.isTrackPlayer) {
            // 发送追踪行为结束消息
            this.destroy = true;
            this.callback.SSActionEvent(this, SSActionEventType.Competed, 1, null, this.gameobject);
        }
    }

    // 获取动作
    public static PatrolTrackAction GetSSAction(GameObject player) {
        PatrolTrackAction action = CreateInstance<PatrolTrackAction>();
        action.trackPlayer = player;
        return action;
    }
}
