using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {
    private MainController mainController;

    void Start() {
        mainController = Director.GetInstance().mainController as MainController;
    }

    void Update() {
        Move();
    }

    void OnGUI() {
        ShowScore();
        ShowRules();
        GUIStyle textStyle = new GUIStyle();
        textStyle.fontSize = 30;
        if(mainController.GetGameover() && mainController.GetTreasureNumber() != 0) {
            GUI.Label(new Rect(Screen.width / 2 - 55, Screen.width / 2 - 250, 100, 100), "游戏结束", textStyle);
            if (GUI.Button(new Rect(Screen.width / 2 - 45, Screen.width / 2 - 170, 100, 50), "重新开始")) {
                mainController.Restart();
            }
        }
        else if(mainController.GetTreasureNumber() == 0) {
            GUI.Label(new Rect(Screen.width / 2 - 55, Screen.width / 2 - 250, 100, 100), "恭喜胜利！", textStyle);
            if (GUI.Button(new Rect(Screen.width / 2 - 45, Screen.width / 2 - 170, 100, 50), "重新开始")) {
                mainController.Restart();
            }
        }
    }

    public void Move() {
        float translationX = Input.GetAxis("Horizontal");
        float translationZ = Input.GetAxis("Vertical");
        mainController.MovePlayer(translationX, translationZ);
    }

    public void ShowScore() {
        GUIStyle scoreStyle = new GUIStyle();
        GUIStyle textStyle = new GUIStyle();
        scoreStyle.normal.textColor = Color.yellow;
        scoreStyle.fontSize = 20;
        textStyle.fontSize = 20;
        GUI.Label(new Rect(Screen.width - 100, 5, 200, 50), "分数:", textStyle);
        GUI.Label(new Rect(Screen.width - 50, 5, 200, 50), mainController.GetScore().ToString(), scoreStyle);
        GUI.Label(new Rect(10, 5, 50, 50), "剩余宝箱数:", textStyle);
        GUI.Label(new Rect(125, 5, 50, 50), mainController.GetTreasureNumber().ToString(), scoreStyle);
    }

    // 展示规则
    public void ShowRules() {
        GUIStyle ruleStyle = new GUIStyle();
        ruleStyle.fontSize = 17;
        GUI.Label(new Rect(Screen.width / 2 - 80, 10, 100, 100), "按方向键进行移动", ruleStyle);
        GUI.Label(new Rect(Screen.width / 2 - 190, 30, 100, 100), "每次甩掉一个巡逻兵计一分，与巡逻兵碰撞游戏结束", ruleStyle);
        GUI.Label(new Rect(Screen.width / 2 - 130, 50, 100, 100), "收集完所有的宝箱那么游戏获胜", ruleStyle);
    }
}
