using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 游戏状态，0为准备进行，1为正在进行游戏，2为结束 */
enum GameState {
    Ready = 0, Playing = 1, GameOver = 2
};

public class MainController : MonoBehaviour {
    private RoundController roundController; // 回合控制器
    private View view; // 游戏视图
    private int N = 2; // 默认游戏回合
    private int gameState;
    public GUISkin gameSkin;

    void Start() {
        Director.GetInstance().mainController = this;
        roundController = gameObject.AddComponent<RoundController>();
        view = gameObject.AddComponent<View>();
        gameState = (int)GameState.Ready;
        view.gameSkin = gameSkin;
    }

    public int GetN() {
        return N;
    }

    public void Restart() {
        view.Init();
        roundController.Reset();
    }

    public void SetGameState(int state) {
        gameState = state;
    }

    public int GetGameState() {
        return gameState;
    }

    public void ShowPage() {
        switch(gameState) {
            case 0:
                view.ShowHomePage();
                break;
            case 1:
                view.ShowGamePage();
                break;
            case 2:
                view.ShowRestart();
                break;
        }
    }

    public void SetRoundSum(int roundSum) {
        roundController.SetRoundSum(roundSum);
    }

    public void SetPlayDiskModeToPhysis(bool isPhysis) {
        roundController.SetPlayDiskModeToPhysis(isPhysis);
    }

    public void SetViewTip(string tip) {
        view.SetTip(tip);
    }

    public void SetViewScore(int score) {
        view.SetScore(score);
    }

    public void SetViewRoundNum(int round) {
        view.SetRoundNum(round);
    }

    public void SetViewTrialNum(int trial) {
        view.SetTrialNum(trial);
    }

    public void Hit(Vector3 position) {
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(position);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<DiskData>() != null) {
                // 把击中的飞碟移出屏幕，触发回调释放
                hit.collider.gameObject.transform.position = new Vector3(0, -6, 0);
                // 记录飞碟得分
                roundController.Record(hit.collider.gameObject.GetComponent<DiskData>());
                // 显示当前得分
                view.SetScore(roundController.GetScores());
            }
        }
    }

    // 释放所有工厂飞碟
    public void FreeAllFactoryDisk() {
        roundController.FreeAllFactoryDisk();
    }
}
