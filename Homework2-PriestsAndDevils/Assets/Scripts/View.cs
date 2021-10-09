using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils {
    /* 船视图 */
    public class BoatView {
        private GameObject boat; // 创建船视图需要的游戏对象

        /* 创建船视图 */
        public BoatView() {
            boat = Object.Instantiate(Resources.Load ("Perfabs/Boat", typeof(GameObject)), new Vector3(2.5F, -1, 0), Quaternion.identity, null) as GameObject;
			boat.name = "Boat";
        }

        public GameObject GetGameObj() {
            return boat;
        }

        /* 为游戏对象添加移动控制器 */
        public MoveController AddMoveScript() {
            MoveController moveScript = boat.AddComponent(typeof(MoveController)) as MoveController;
            return moveScript;
        }

        /* 为游戏对象添加点击控制器 */
        public void AddClickScript() {
            boat.AddComponent(typeof(ClickController));
        }
    }

    /* 岸视图 */
    public class ShoreView {
        private GameObject shore; // 创建岸视图需要的游戏对象

        /* 根据岸的边侧创建游戏对象和视图 */
        public ShoreView(int side, string sideName) {
            shore = Object.Instantiate(Resources.Load("Perfabs/Shore", typeof(GameObject)), new Vector3(7 * side, -1, 0), Quaternion.identity, null) as GameObject;
            shore.name = sideName;
        }
    }

    /* 角色视图 */
    public class RoleView {
        private GameObject role; // 创建角色视图需要的游戏对象

        /* 根据角色类型和序号创建游戏对象和视图 */
        public RoleView(string roleName, int number) {
            role = Object.Instantiate(Resources.Load ("Perfabs/" + roleName, typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            role.name = roleName + number;
        }

        /* 为游戏对象添加移动控制器 */
        public MoveController AddMoveScript() {
            MoveController moveScript = role.AddComponent(typeof(MoveController)) as MoveController;
            return moveScript;
        }

        /* 为游戏对象添加点击控制器 */
        public ClickController AddClickScript() {
            return role.AddComponent(typeof(ClickController)) as ClickController;
        }

        /* 获取对象名字 */
        public string GetName() {
            return role.name;
        }

        /* 设置角色父对象 */
        public void SetRoleParentTrans(Transform trans) {
            role.transform.parent = trans;
        }

        /* 设置角色位置 */
        public void SetPos(Vector3 pos) {
            role.transform.position = pos;
        }
    }

    /* 河流视图 */
    public class RiverView {
        private GameObject river; // 创建河流视图需要的游戏对象

        /* 创建河流视图 */
        public RiverView() {
            river = Object.Instantiate(Resources.Load ("Perfabs/River", typeof(GameObject)), new Vector3(0, -1.5F, 0), Quaternion.identity, null) as GameObject;
			river.name = "River";
        }
    }

    /* 移动视图 */
    public class MoveView : MonoBehaviour {
        /* 在界面上根据移动位置和速度呈现物体移动效果 */
        public void MoveTo(Vector3 pos, int speed) {
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        }
    }

    /* 功能视图 */
    public class FunctionView : MonoBehaviour {
        private string tipContent; // 提示框显示内容

        public FunctionView() {
            tipContent = "Click and Play the Game";
        }

        public void SetTipContent(string tip) {
            tipContent = tip;
        }

        /* 初始化提示框 */
        public void InitFunctionView() {
            tipContent = "Click and Play the Game";
        }

        void ShowResetButton() {
            GUIStyle resetStyle = new GUIStyle("button");
			resetStyle.fontSize = 20;

            // 按下重置按钮，游戏被初始化
			if (GUI.Button(new Rect(30, 20, 70, 30), "Reset", resetStyle)) {
				SingleObject.getInstance().GetMainController().InitGame();
			}
        }

        void ShowTip(string tipContent) {
            GUIStyle tipStyle = new GUIStyle();

            tipStyle.fontSize = 20;
            GUI.Label(new Rect(Screen.width - 140, 20, 100, 50), "Priest: Yellow\nDevil: Red", tipStyle);

			tipStyle.fontSize = 30;
			tipStyle.alignment = TextAnchor.MiddleCenter;

            // 展示提示内容
			GUI.Label(new Rect(Screen.width / 2 - 50, 70, 100, 50), tipContent, tipStyle);
        }

        void OnGUI() {
            ShowTip(tipContent); // 展现提示框
            ShowResetButton(); // 展现重置按钮
		}

    }
}
