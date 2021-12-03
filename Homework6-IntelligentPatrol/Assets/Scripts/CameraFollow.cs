using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject follow;  // 跟随的物体
    public float speed = 5f;  // 相机跟随物体的的速度
    Vector3 offsetPos;  // 相机和物体的相对偏移位置

    void Start() {
        offsetPos = transform.position - follow.transform.position;
    }

    void FixedUpdate() {
        Vector3 targetPos = follow.transform.position + offsetPos;
        // 摄像机平滑过渡到目标位置
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
