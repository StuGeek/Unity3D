using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {
    private int score = 0;  // 分数

    public int GetScore() {
        return score;
    }

    public void IncreaseScore() {
        score++;
    }
}
