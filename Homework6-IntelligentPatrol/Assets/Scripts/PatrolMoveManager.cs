using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMoveManager : SSActionManager {
    // 按照矩形移动
    public void MoveRect(GameObject patrol) {
        PatrolNormalAction moveRect = PatrolNormalAction.GetSSAction(patrol.transform.position);
        this.RunAction(patrol, moveRect, this);
    }
}
