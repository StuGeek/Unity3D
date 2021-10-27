using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    public GameObject diskPrefab; // 飞碟游戏对象，创建新的飞碟游戏对象的复制对象
    private List<DiskData> used; // 正在被游戏使用的飞碟对象
    private List<DiskData> free; // 没有被使用的空闲飞碟对象

    public void Start() {
        diskPrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Disk"), Vector3.zero, Quaternion.identity);
        diskPrefab.SetActive(false);
        used = new List<DiskData>();
        free = new List<DiskData>();
    }

    // 飞碟获取方法，根据ruler获取相应飞碟
    public GameObject GetDisk(Ruler ruler) {
        GameObject disk;

        // 从缓存中获取飞碟，没有则先创建
        int diskNum = free.Count;
        if (diskNum == 0) {
            disk = GameObject.Instantiate(diskPrefab, Vector3.zero, Quaternion.identity);
            disk.AddComponent(typeof(DiskData));
        }
        else {
            disk = free[diskNum - 1].gameObject;
            free.Remove(free[diskNum - 1]);
        }

        // 根据ruler设置disk的速度、颜色、大小、飞入方向
        disk.GetComponent<DiskData>().speed = ruler.speed;
        disk.GetComponent<DiskData>().color = ruler.color;
        disk.GetComponent<DiskData>().size = ruler.size;
        
        // 给飞碟上颜色
        if (ruler.color == "red") {
            disk.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (ruler.color == "green") {
            disk.GetComponent<Renderer>().material.color = Color.green;
        }
        else {
            disk.GetComponent<Renderer>().material.color = Color.blue;
        }

        // 绘制飞碟大小
        disk.transform.localScale = new Vector3(1.2f, 0.1f * (float)ruler.size, 1.2f);
        
        // 选择飞碟飞入屏幕的起始位置
        disk.transform.position = ruler.beginPos;
        
        // 设置飞碟显示
        disk.SetActive(true);
    
        // 将飞碟加入使用队列
        used.Add(disk.GetComponent<DiskData>());

        return disk;
    }

    // 飞碟回收方法，将不使用的飞碟从使用队列放到空闲队列中
    public void FreeDisk(GameObject disk) {
        foreach (DiskData d in used) {
            if (d.gameObject.GetInstanceID() == disk.GetInstanceID()) {
                disk.SetActive(false);
                used.Remove(d);
                free.Add(d);
                break;
            }

        }
    }
}
