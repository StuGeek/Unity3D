using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNormalAction : SSAction {
    private Vector3 pos;  // 巡逻兵位置
    private float rectLength;  // 矩形长度
    public float speed = 1f;  // 移动速度
    private bool isDest;  // 是否每个方向的终点
    private int direction;  // 巡逻兵的移动方向
    private PatrolData patrol;  // 巡逻兵数据
    private MainController mainController;

    private const int EAST = 0;
    private const int NORTH = 1;
    private const int WEST = 2;
    private const int SOUTH = 3;
    
    // 获取动作
    public static PatrolNormalAction GetSSAction(Vector3 location) {
        PatrolNormalAction action = CreateInstance<PatrolNormalAction>();
        action.pos = new Vector3(location.x, 0, location.z);
        // 设定矩形边长
        action.rectLength = Random.Range(4, 8);
        return action;
    }

    public override void Start() {
        this.gameobject.GetComponent<Animator>().SetBool("run", true);
        patrol = this.gameobject.GetComponent<PatrolData>();
        mainController = Director.GetInstance().mainController;
        isDest = true;
        direction = EAST;
    }

    public override void Update() {
        // 巡逻兵按矩形移动
        MoveRect();
        // 如果巡逻兵和玩家在同一区域并在巡逻兵感知范围内，那么开始追踪
        if (mainController.GetPlayerAreaId() == patrol.patrolAreaId && patrol.isTrackPlayer) {
            this.destroy = true;
            this.callback.SSActionEvent(this, SSActionEventType.Competed, 0, null, this.gameobject);
        }
    }

    void MoveRect() {
        float newPosX = pos.x;
        float newPosZ = pos.z;
        if (isDest) {
            // 设定每个方向的新的移动终点
            switch (direction) {
                case EAST:
                    newPosX -= rectLength;
                    break;
                case NORTH:
                    newPosZ += rectLength;
                    break;
                case WEST:
                    newPosX += rectLength;
                    break;
                case SOUTH:
                    newPosZ -= rectLength;
                    break;
            }
            isDest = false;
        }
        Vector3 newPos = new Vector3(newPosX, 0, newPosZ);
        this.transform.LookAt(newPos);
        float distance = Vector3.Distance(transform.position, newPos);
        // 沿着方向直走
        if (distance > 1) {
            transform.position = Vector3.MoveTowards(this.transform.position, newPos, speed * Time.deltaTime);
        }
        // 在转弯处改变方向
        else {
            direction = (direction + 1) % 4;
            isDest = true;
        }
        // 设定新的位置
        pos.x = newPosX;
        pos.z = newPosZ;
    }
}
