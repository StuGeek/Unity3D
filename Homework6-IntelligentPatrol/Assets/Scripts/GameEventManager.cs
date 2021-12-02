using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {
    // 增加分数
    public delegate void AddScoreEvent();
    public static event AddScoreEvent AddScoreAction;
    // 减少宝箱
    public delegate void DecreaseTreasureEvent();
    public static event DecreaseTreasureEvent DecreaseTreasureAction;
    // 游戏结束
    public delegate void GameoverEvent();
    public static event GameoverEvent GameoverAction;

    // 玩家逃脱
    public void PlayerGetAway() {
        if (AddScoreAction != null) {
            AddScoreAction();
        }
    }

    // 减少宝箱数量
    public void DecreaseTreasureNum() {
        if (DecreaseTreasureAction != null) {
            DecreaseTreasureAction();
        }
    }

    // 玩家被抓到了
    public void PlayerGetCaught() {
        if (GameoverAction != null) {
            GameoverAction();
        }
    }
}
