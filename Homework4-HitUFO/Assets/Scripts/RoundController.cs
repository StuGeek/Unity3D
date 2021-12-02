using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 飞碟获取规则
public struct Ruler {
    public int trialNum; // 当前trial的编号
    public int roundNum; // 当前round的编号
    public int roundSum; // 一共round的总数目
    public int [] roundDisksNum; // 每一轮对于trial的飞碟数量
    public float sendTime; // 发射间隔时间

    public int size; // 飞碟大小
    public int speed; // 飞碟速度
    public string color; // 飞碟颜色
    public Vector3 direction; // 飞碟飞入方向
    public Vector3 beginPos; // 飞碟飞入位置
};

public class RoundController : MonoBehaviour {
    private IActionManager actionManager; // 选择飞碟的运动类型
    private ScoreRecorder scoreRecorder; // 记分器
    private MainController mainController;
    private Ruler ruler; // 飞碟获取规则

    void Start() {
        // 一开始飞碟的运动类型默认为运动学运动
        actionManager = gameObject.AddComponent<CCActionManager>();
        gameObject.AddComponent<PhysisActionManager>();
        scoreRecorder = new ScoreRecorder();
        mainController = Director.GetInstance().mainController;
        gameObject.AddComponent<DiskFactory>();
        InitRuler();
    }

    void InitRuler() {
        ruler.trialNum = 0;
        ruler.roundNum = 0;
        ruler.sendTime = 0;
        ruler.roundDisksNum = new int [10];
        generateRoundDisksNum();
    }

    // 生成每trial同时发出的飞碟数量的数组，同时发出飞碟个数不超过4
    public void generateRoundDisksNum() {
        for (int i = 0; i < 10; ++i) {
            ruler.roundDisksNum[i] = Random.Range(0, 4) + 1;
        }
    }

    public void Reset() {
        InitRuler();
        scoreRecorder.Reset();
    }

    public void Record(DiskData disk) {
        scoreRecorder.Record(disk);
    }

    public int GetScores() {
        return scoreRecorder.score;
    }

    public void SetRoundSum(int roundSum) {
        ruler.roundSum = roundSum;
    }

    // 设置游戏模式，同时支持物理运动模式和动力学运动模式
    public void SetPlayDiskModeToPhysis(bool isPhysis) {
        if (isPhysis) {
            actionManager = Singleton<PhysisActionManager>.Instance as IActionManager;
        }
        else {
            actionManager = Singleton<CCActionManager>.Instance as IActionManager;
        }
    }

    // 发射飞碟
    public void LaunchDisk() {
        // 使飞碟飞入位置尽可能分开，从不同位置飞入使用的数组
        int [] beginPosY = new int [4]{0, 0, 0, 0};

        for (int i = 0; i < ruler.roundDisksNum[ruler.trialNum]; ++i) {
            // 获取随机数
            int randomNum = Random.Range(0, 3) + 1;
            // 飞碟速度随回合数增加而变快，这样难度增加
            ruler.speed = randomNum * (ruler.roundNum + 4);

            // 重新选取随机数，并根据随机数选择飞碟颜色
            randomNum = Random.Range(0, 3) + 1;
            if (randomNum == 1) {
                ruler.color = "red";
            }
            else if (randomNum == 2) {
                ruler.color = "green";
            }
            else {
                ruler.color = "blue";
            }

            // 重新选取随机数，并根据随机数选择飞碟的大小
            ruler.size = Random.Range(0, 3) + 1;

            // 重新选取随机数，并根据随机数选择飞碟飞入的方向
            randomNum = Random.Range(0, 2);
            if (randomNum == 1) {
                ruler.direction = new Vector3(3, 1, 0);
            }
            else {
                ruler.direction = new Vector3(-3, 1, 0);
            }

            // 重新选取随机数，并使不同飞碟的飞入位置尽可能分开
            do {
                randomNum = Random.Range(0, 4);
            } while (beginPosY[randomNum] != 0);
            beginPosY[randomNum] = 1;
            ruler.beginPos = new Vector3(-ruler.direction.x * 4, -0.5f * randomNum, 0);

            // 根据ruler从工厂中生成一个飞碟
            GameObject disk = Singleton<DiskFactory>.Instance.GetDisk(ruler);
        
            // 设置飞碟的飞行动作
            actionManager.PlayDisk(disk, ruler.speed, ruler.direction);
        }
    }

    // 释放工厂飞碟
    public void FreeFactoryDisk(GameObject disk) {
        Singleton<DiskFactory>.Instance.FreeDisk(disk);
    }

    // 释放所有工厂飞碟
    public void FreeAllFactoryDisk() {
        GameObject[] obj = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject g in obj) {
            if (g.gameObject.name == "Disk(Clone)(Clone)") {
                Singleton<DiskFactory>.Instance.FreeDisk(g);
            }
        }
    }

    void Update() {
        if (mainController.GetGameState() == (int)GameState.Playing) {
            ruler.sendTime += Time.deltaTime;
            // 每隔2s发送一次飞碟(trial)
            if (ruler.sendTime > 2) {
                ruler.sendTime = 0;
                // 如果为无限回合或还未到设定回合数
                if (ruler.roundSum == -1 || ruler.roundNum < ruler.roundSum) {
                    // 发射飞碟，次数trial增加
                    mainController.SetViewTip("");
                    LaunchDisk();
                    ruler.trialNum++;
                    // 当次数trial等于10时，说明一个回合已经结束，回合加一，重新生成飞碟数组
                    if (ruler.trialNum == 10) {
                        ruler.trialNum = 0;
                        ruler.roundNum++;
                        generateRoundDisksNum();
                    }
                }
                // 否则游戏结束，提示重新进行游戏
                else {
                    mainController.SetViewTip("Click Restart and Play Again!");
                    mainController.SetGameState((int)GameState.GameOver);
                }
                // 设置回合数和trial数目的提示
                if (ruler.trialNum == 0) mainController.SetViewRoundNum(ruler.roundNum);
                else mainController.SetViewRoundNum(ruler.roundNum + 1);
                mainController.SetViewTrialNum(ruler.trialNum);
            }
        }
    }
}
