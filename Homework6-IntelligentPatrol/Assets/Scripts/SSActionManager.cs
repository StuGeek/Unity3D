using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour, ISSActionCallback {
    // 动作集
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    // 即将开始的动作的等待加入队列
    private List<SSAction> waitingAdd = new List<SSAction>();
    // 已完成的的动作的等待删除队列
    private List<int> waitingDelete = new List<int>();

    protected void Update() {
        // 载入即将开始的动作
        foreach (SSAction ac in waitingAdd) {
            actions[ac.GetInstanceID()] = ac;
        }
        // 清空等待加入队列
        waitingAdd.Clear();

        // 运行载入动作
        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction ac = kv.Value;
            if (ac.destroy) {
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable) {
                ac.Update();
            }
        }

        // 清空已完成的动作
        foreach (int key in waitingDelete) {
            SSAction ac = actions[key];
            actions.Remove(key);
            Object.Destroy(ac);
        }
        // 清空等待删除队列
        waitingDelete.Clear();
    }

    // 初始化动作并加入到等待加入队列
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    // 巡逻兵正常巡逻或追踪玩家行为结束后的回调方法
    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competed,
        int intParam = 0,
        string strParam = null,
        GameObject objectParam = null)
    {
        // 如果消息的回调参数为1，追踪行为结束，开始正常巡逻
        if(intParam == 1) {
            // 继续巡逻
            PatrolNormalAction move = PatrolNormalAction.GetSSAction(objectParam.gameObject.GetComponent<PatrolData>().startPos);
            this.RunAction(objectParam, move, this);
            // 玩家逃脱消息
            Singleton<GameEventManager>.Instance.PlayerGetAway();
        }
        // 如果消息的回调参数为0，正常巡逻结束，开始追踪玩家
        else {
            // 追踪玩家
            PatrolTrackAction trackAction = PatrolTrackAction.GetSSAction(objectParam.gameObject.GetComponent<PatrolData>().trackPlayer);
            this.RunAction(objectParam, trackAction, this);
        }
    }

    // 清除所有动作
    public void DestroyAll() {
        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction ac = kv.Value;
            ac.destroy = true;
        }
    }
}
