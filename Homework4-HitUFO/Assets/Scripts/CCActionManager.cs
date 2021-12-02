using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback, IActionManager {
    CCPlayDiskAction PlayDiskAction; // 飞碟空中动作

    public void PlayDisk(GameObject disk, float speed, Vector3 direction) {
        PlayDiskAction = CCPlayDiskAction.GetSSAction(direction, speed);
        RunAction(disk, PlayDiskAction, this);
    }

    // 回调函数
    public void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competed,
    int intParam = 0,
    string strParam = null,
    Object objectParam = null)
    {
        // 结束飞行后回收飞碟
        Singleton<RoundController>.Instance.FreeFactoryDisk(source.gameObject);
    }
}
