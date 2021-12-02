using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder {
    public int score; // 游戏分数

    public ScoreRecorder() {
        score = 0;
    }

    /* 记录分数，根据点击中的飞碟的大小，速度，颜色计算得分 */
    public void Record(DiskData disk) {
        // 飞碟越小分就越高，大小为1得3分，大小为2得2分，大小为3得1分
        int diskSize = disk.size;
        switch (diskSize) {
            case 1:
                score += 3;
                break;
            case 2:
                score += 2;
                break;
            case 3:
                score += 1;
                break;
            default: break;
        }

        // 速度越快分就越高
        score += disk.speed;

        // 颜色为红色得1分，颜色为黄色得2分，颜色为蓝色得3分
        string diskColor = disk.color;
        if (diskColor.CompareTo("red") == 0) {
            score += 1;
        }
        else if (diskColor.CompareTo("green") == 0) {
            score += 2;
        }
        else if (diskColor.CompareTo("blue") == 0) {
            score += 3;
        }
    }

    /* 重置分数，设为0 */
    public void Reset() {
        score = 0;
    }
}
