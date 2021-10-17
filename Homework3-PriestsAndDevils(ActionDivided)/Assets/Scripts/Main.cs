using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils {
	public class Main : MonoBehaviour {
        void Start() {
            // 创建主控制器
            MainController mainController = gameObject.AddComponent<MainController>() as MainController;
        }
    }
}
