using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils {
    /* 船模型 */
    public class BoatModel {
        private string [] roleOnBoat = new string [2]{"", ""};  // 船上的角色，要不为牧师或魔鬼，要不为空

        public void SetRoleOnBoat(string [] roles) {
            roleOnBoat = roles;
        }

        public string [] GetRoleOnBoat() {
            return roleOnBoat;
        }
    }

    /* 岸模型 */
    public class ShoreModel {
		private int side;  // 岸边方向，-1表示左岸即终点岸，1表示右岸即出发岸
        private string [] roleOnShore = new string [6]{"", "", "", "", "", ""};  // 岸上的角色，要不为牧师或魔鬼，要不为空

        public ShoreModel(int side) {
            this.side = side;
        }

        public void SetSide(int side) {
            this.side = side;
        }

        public int GetSide() {
            return side;
        }

        public void SetRoleOnShore(string [] roles) {
            roleOnShore = roles;
        }

        public string [] GetRoleOnShore() {
            return roleOnShore;
        }
    }

    /* 角色模型 */
    public class RoleModel {
        private bool isOnBoat;  // 角色是否在船上

        public void SetIsOnBoat(bool isOnBoat) {
            this.isOnBoat = isOnBoat;
        }

        public bool GetIsOnBoat() {
            return isOnBoat;
        }
    }

    /* 行为模型 */
    public class ActionModel {
        private static int speed = 25;  // 移动速度
        private Vector3 middlePos;  // 移动的中间位置，因为角色以折线形式运动，所以会有一个先移动到的中间位置
        private Vector3 endPos;  // 移动的最后位置，角色或船最后移动到的位置
		private int state;	// 移动状态，0表示没有移动，1表示正在移动到中间位置，2表示正在从中间位置移动到最后位置
        private int boatSide = (int)ShoreSide.Right;  // 如果对象是船的岸边方向，-1表示物体在左岸即终点岸，1表示物体在右岸即出发岸

        public int GetSpeed() {
            return speed;
        }

        public void SetMiddlePos(Vector3 pos) {
            middlePos = pos;
        }

        public Vector3 GetMiddlePos() {
            return middlePos;
        }
        
        public void SetEndPos(Vector3 pos) {
            endPos = pos;
        }

        public Vector3 GetEndPos() {
            return endPos;
        }

        public void SetState(int state) {
            this.state = state;
        }

        public int GetState() {
            return state;
        }

        public void SetSide(int side) {
            boatSide = side;
        }

        public int GetSide() {
            return boatSide;
        }
    }
}
