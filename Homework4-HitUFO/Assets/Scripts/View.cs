using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {
    private MainController mainController;
    private int score;
    private string tip;
    private string roundNum;
    private string trialNum;
    public GUISkin gameSkin;  // 游戏控件的皮肤风格

    void Start() {
        score = 0;
        tip = "";
        roundNum = "";
        trialNum = "";
        mainController = Director.GetInstance().mainController;
    }

    public void SetTip(string tip) {
        this.tip = tip;
    }

    public void SetScore(int score) {
        this.score = score;
    }

    public void SetRoundNum(int round) {
        roundNum = "回合: " + round;
    }

    public void SetTrialNum(int trial) {
        if (trial == 0) trial = 10;
        trialNum = "Trial: " + trial;
    }

    public void Init() {
        score = 0;
        tip = "";
        roundNum = "";
        trialNum = "";
    }

    public void AddTitle() {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.normal.textColor = Color.black;
        titleStyle.fontSize = 50;

        GUI.Label(new Rect(Screen.width / 2 - 80, 20, 60, 100), "Hit UFO", titleStyle);
    }

    public void AddChooseModeButton() {
        GUI.skin = gameSkin;
        if (GUI.Button(new Rect(280, 100, 160, 80), "普通模式\n(默认为" + mainController.GetN() + "回合)")) {
            mainController.SetRoundSum(mainController.GetN());
            mainController.Restart();
            mainController.SetGameState((int)GameState.Playing);
        }
        if (GUI.Button(new Rect(280, 210, 160, 80), "无尽模式\n(回合数无限)")) {
            mainController.SetRoundSum(-1);
            mainController.Restart();
            mainController.SetGameState((int)GameState.Playing);
        }
    }

    public void ShowHomePage() {
        AddChooseModeButton();
    }

    public void AddActionModeButton() {
        GUI.skin = gameSkin;
        if (GUI.Button(new Rect(10, Screen.height - 100, 110, 40), "运动学模式")) {
            mainController.FreeAllFactoryDisk();
            mainController.SetPlayDiskModeToPhysis(false);
        }
        if (GUI.Button(new Rect(10, Screen.height - 50, 110, 40), "物理模式")) {
            mainController.FreeAllFactoryDisk();
            mainController.SetPlayDiskModeToPhysis(true);
        }
    }

    public void AddBackButton() {
        GUI.skin = gameSkin;
        if (GUI.Button(new Rect(10, 10, 60, 40), "Back")) {
            mainController.FreeAllFactoryDisk();
            mainController.Restart();
            mainController.SetGameState((int)GameState.Ready);
        }
    }

    public void AddGameLabel() {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black;
        labelStyle.fontSize = 30;

        GUI.Label(new Rect(570, 10, 100, 50), "得分: " + score, labelStyle);
        GUI.Label(new Rect(170, 80, 50, 200), tip, labelStyle);
        GUI.Label(new Rect(570, 60, 100, 50), roundNum, labelStyle);
        GUI.Label(new Rect(570, 110, 100, 50), trialNum, labelStyle);
    }

    public void AddRestartButton() {
        if (GUI.Button(new Rect(300, 150, 100, 60), "Restart")) {
            mainController.FreeAllFactoryDisk();
            mainController.Restart();
            mainController.SetGameState((int)GameState.Playing);
        }
    }

    public void ShowGamePage() {
        AddGameLabel();
        AddBackButton();
        AddActionModeButton();
        if (Input.GetButtonDown("Fire1")) {
            mainController.Hit(Input.mousePosition);
        }
    }

    public void ShowRestart() {
        ShowGamePage();
        AddRestartButton();
    }

    void OnGUI() {
        AddTitle();
        mainController.ShowPage();
    }
}
