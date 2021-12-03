using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {
    private ScoreController scoreRecorder;  // 记分器
    private PatrolMoveManager moveManager;  // 移动管理器
    private int playerAreaId;  // 玩家所处区域序号
    private GameObject player;  // 玩家对象
    public Camera mainCamera;  // 主相机
    public int treasureNumber = 10;  // 宝箱数量
    public float moveSpeed = 5;  // 移动速度
    public float rotateSpeed = 135f;  // 旋转速度
    private bool isGameOver;  // 游戏是否结束
    
    void Start() {
        Director director = Director.GetInstance();
        director.mainController = this;
        scoreRecorder = gameObject.AddComponent<ScoreController>() as ScoreController;
        moveManager = gameObject.AddComponent<PatrolMoveManager>() as PatrolMoveManager;
        LoadResources();
        mainCamera.GetComponent<CameraFollow>().follow = player;
        isGameOver = false;
    }

    void Update() {
        CheckGameOver();
    }

    // 加载资源
    public void LoadResources() {
        // 生成地图
        Instantiate(Resources.Load<GameObject>("Prefabs/Map"));
        // 生成玩家
        player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(0, 9, 0), Quaternion.identity) as GameObject;
        // 生成宝箱
        Singleton<GameObjectFactory>.Instance.GetTreasures();
        // 生成巡逻兵并让其移动
        MovePatrol();
    }

    public void MovePatrol() {
        // 让所有巡逻兵都移动
        List<GameObject> patrols = Singleton<GameObjectFactory>.Instance.GetPatrols();
        for (int i = 0; i < patrols.Count; i++) {
            moveManager.MoveRect(patrols[i]);
        }
    }

    // 玩家移动
    public void MovePlayer(float translationX, float translationZ) {
        if(!isGameOver) {
            MovePlayerAction(translationX, translationZ);
            if (player.transform.position.y != 0) {
                player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            }     
        }
    }

    // 设置玩家移动时的跑步动作
    public void MovePlayerAction(float translationX, float translationZ) {
        if (translationX != 0 || translationZ != 0) {
            player.GetComponent<Animator>().SetBool("run", true);
        }
        else {
            player.GetComponent<Animator>().SetBool("run", false);
        }
        // 移动和旋转动作
        player.transform.Translate(0, 0, translationZ * moveSpeed * Time.deltaTime);
        player.transform.Rotate(0, translationX * rotateSpeed * Time.deltaTime, 0);
    }

    public void IncreaseScore() {
        scoreRecorder.IncreaseScore();
    }

    public void CheckGameOver() {
        // 所有宝箱都被收集，游戏结束
        if(treasureNumber == 0) {
            Gameover();
        }
    }

    // 游戏结束，释放所有的巡逻兵
    public void Gameover() {
        isGameOver = true;
        Singleton<GameObjectFactory>.Instance.FreePatrols();
        moveManager.DestroyAll();
    }

    public void DecreaseTreasureNumber() {
        treasureNumber--;
    }

    public int GetScore() {
        return scoreRecorder.GetScore();
    }

    public void SetPlayerAreaId(int areaId) {
        playerAreaId = areaId;
    }

    public int GetPlayerAreaId() {
        return playerAreaId;
    }

    public int GetTreasureNumber() {
        return treasureNumber;
    }

    public bool GetGameover() {
        return isGameOver;
    }

    public void Restart() {
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    void OnEnable() {
        GameEventManager.AddScoreAction += IncreaseScore;
        GameEventManager.GameoverAction += Gameover;
        GameEventManager.DecreaseTreasureAction += DecreaseTreasureNumber;
    }

    void OnDisable() {
        GameEventManager.AddScoreAction -= IncreaseScore;
        GameEventManager.GameoverAction -= Gameover;
        GameEventManager.DecreaseTreasureAction -= DecreaseTreasureNumber;
    }
}
