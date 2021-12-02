using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject {
    public bool enable = true; // 动作可进行
    public bool destroy = false; // 动作已完成可被销毁

    public GameObject gameObject { get; set; } // 附着游戏对象
    public Transform transform { get; set; } // 游戏对象的的运动
    public ISSActionCallback callback { get; set; } // 回调函数

    public virtual void Start() {} // Start()重写方法

    public virtual void Update() {} // Update()重写方法
}
