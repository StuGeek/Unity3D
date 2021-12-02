using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour {
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
            Destroy(ac);
        }
        // 清空等待删除队列
        waitingDelete.Clear();
    }

    // 初始化动作并加入到等待加入队列
    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback manager) {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}
