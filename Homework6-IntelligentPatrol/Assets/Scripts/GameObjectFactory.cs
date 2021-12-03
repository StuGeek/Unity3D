using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFactory : MonoBehaviour {
    private List<GameObject> usedPatrol = new List<GameObject>();  // 正在被使用的巡逻兵对象
    private List<GameObject> usedTreasure = new List<GameObject>();  // 正在被使用的宝箱对象

    // 巡逻兵获取方法
    public List<GameObject> GetPatrols() {
        // 巡逻兵游戏对象
        GameObject patrolPrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Patrol"), Vector3.zero, Quaternion.identity);
        patrolPrefab.SetActive(false);
        float[] posX = { -5.5f, 4.5f, 12.5f };
        float[] posZ = { -4.5f, 5.5f, -12.5f };
        // 生成巡逻兵的初始位置
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                Vector3 startPos = new Vector3(posX[i], 0, posZ[j]);
                GameObject patrol = GameObject.Instantiate(patrolPrefab, startPos, Quaternion.identity);
                patrol.SetActive(true);
                patrol.GetComponent<PatrolData>().patrolAreaId = i * 3 + j + 1;
                patrol.GetComponent<PatrolData>().startPos = startPos;
                usedPatrol.Add(patrol);
            }
        }
        return usedPatrol;
    }

    // 巡逻兵回收方法
    public void FreePatrols() {
        // 巡逻兵停止
        for (int i = 0; i < usedPatrol.Count; ++i) {
            usedPatrol[i].gameObject.GetComponent<Animator>().SetBool("run", false);
        }
    }

    // 宝箱获取方法
    public List<GameObject> GetTreasures() {
        // 宝箱游戏对象
        GameObject treasurePrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Treasure"), Vector3.zero, Quaternion.identity);
        treasurePrefab.SetActive(false);
        for (int i = 0; i < Director.GetInstance().mainController.GetTreasureNumber(); ++i) {
            int xIndex = Random.Range(0, 24) - 12;
            int zIndex = Random.Range(0, 24) - 12;
            GameObject treasure = GameObject.Instantiate(treasurePrefab, new Vector3(xIndex, 0, zIndex), Quaternion.identity);
            treasure.SetActive(true);
            usedTreasure.Add(treasure);
        }
        return usedTreasure;
    }
}
