using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestsAndDevils {
	/* 河岸岸边，左岸终点岸为-1，右岸出发岸为1 */
	enum ShoreSide {
		Left = -1, Right = 1
	};

	/* 移动状态，0表示没有移动，1表示正在移动到中间位置，2表示正在从中间位置移动到最后位置 */
	enum MoveState {
		Not_Moving = 0, Moving_To_Middle = 1, Moving_To_End = 2
	};

	/* 游戏状态，0为正在进行，1为游戏失败，2为游戏成功 */
	enum GameState {
		Playing = 0, Lose = 1, Win = 2
	};

	/* 移动控制器，一般会挂载到某个具体的游戏对象上 */
	public class MoveController : MonoBehaviour {
		private MoveModel moveModel;
		private MoveView moveView;

		void Start() {
			moveModel = new MoveModel();
			moveView = gameObject.AddComponent<MoveView>() as MoveView;
		}

		void Update() {
			int state = moveModel.GetState();
			Vector3 middlePos = moveModel.GetMiddlePos();
			Vector3 endPos = moveModel.GetEndPos();
			int speed = moveModel.GetSpeed();
			// 根据速度、中间位置、终点位置、移动状态来移动物体
			Move(state, middlePos, endPos, speed);
		}

		/* 初始化控制器，设置物体不移动 */
		public void InitMoveController() {
			moveModel.SetState((int)MoveState.Not_Moving);
		}

		/* 根据速度、中间位置、终点位置、移动状态来移动物体 */
		public void Move(int state, Vector3 middlePos, Vector3 endPos, int speed) {
			switch (state) {
				case (int)MoveState.Not_Moving:
					break;
				case (int)MoveState.Moving_To_Middle:
					// 移动物体到中间位置，移动到后，设置物体的下一个状态为移动到终点
					moveView.MoveTo(middlePos, speed);
					if (transform.position == middlePos) {
						moveModel.SetState((int)MoveState.Moving_To_End);
					}
					break;
				case (int)MoveState.Moving_To_End:
					// 移动物体到终点位置，移动到后，设置物体的下一个状态为不移动
					moveView.MoveTo(endPos, speed);
					if (transform.position == endPos) {
						moveModel.SetState((int)MoveState.Not_Moving);
					}
					break;
			}
		}

		/* 设置物体要移动到的位置 */
		public void SetMovePos(Vector3 pos) {
			// 设置物体开始往中间位置移动
			moveModel.SetState((int)MoveState.Moving_To_Middle);
			// 设置最终移动位置为pos
			moveModel.SetEndPos(pos);
			// 如果要移动到的位置的y坐标等于物体所在位置的y坐标，说明是船在移动
			if (pos.y == transform.position.y) {
				// 船沿直线运动，所以中间位置也可设为最终位置
				moveModel.SetMiddlePos(pos);
				// 移动状态可以直接设为往终点移动
				moveModel.SetState((int)MoveState.Moving_To_End);
			}
			// 如果要移动到的位置的y坐标小于物体所在位置的y坐标，说明是物体从岸上移动到船上
			else if (pos.y < transform.position.y) {
				moveModel.SetMiddlePos(new Vector3(pos.x, transform.position.y, pos.z));
			}
			// 如果要移动到的位置的y坐标小于物体所在位置的y坐标，说明是物体从船上移动到岸上
			else {
				moveModel.SetMiddlePos(new Vector3(transform.position.x, pos.y, pos.z));
			}
		}
	}

	/* 点击控制器，一般会挂载在某个游戏对象上 */
	public class ClickController : MonoBehaviour {
		private RoleController clickRole; // 如果点击的是角色，那么记录点击的对象的控制器

		public void SetClickRole(RoleController role) {
			clickRole = role;
		}

		void OnMouseDown() {
			// 如果鼠标点击的对象是船，那么移动船只
			if (gameObject.name == "Boat") {
				SingleObject.getInstance().GetMainController().MoveBoat();
			} else {
				// 如果鼠标点击的对象是角色，那么移动对象
				SingleObject.getInstance().GetMainController().MoveRole(clickRole);
			}
		}
	}

	public class BoatController {
		// 船在左岸时装载角色的位置
		private static Vector3 [] leftLoadPos = new Vector3 [] {new Vector3(-3, -0.5F, 0), new Vector3(-2, -0.5F, 0)};
		// 船在右岸时装载角色的位置
		private static Vector3 [] rightLoadPos =  new Vector3 [] {new Vector3(2, -0.5F, 0), new Vector3(3, -0.5F, 0)};

		private BoatModel boatModel; // 船模型
		private BoatView boatView; // 船视图
		private MoveController moveController; // 移动控制器

		public BoatController() {
			// 船开始在右岸
			boatModel = new BoatModel((int)ShoreSide.Right);
			boatView = new BoatView();
			// 添加移动控制器
			moveController = boatView.AddMoveScript();
			// 添加点击控制器
			boatView.AddClickScript();
		}

		/* 初始化船控制器 */
		public void InitBoatController() {
			// 先初始化移动控制器
			moveController.InitMoveController();
			// 获取船在哪边
			int side = boatModel.GetSide();
			// 在左边的话，船会回到右边去
			if (side == (int)ShoreSide.Left) {
				MoveToOtherSide();
			}
			// 初始化船模型没有乘客
			boatModel.SetRoleOnBoat(new string [2]{"", ""});
		}

		// 获取船所在的岸边
		public int GetBoatSide() {
			return boatModel.GetSide();
		}

		// 获取船的游戏对象
		public GameObject GetBoat() {
			return boatView.GetGameObj();
		}

		/* 将船移动到岸的另一侧 */
		public void MoveToOtherSide() {
			// 先获取船在哪边
			int side = boatModel.GetSide();
			// 移动船到另一边
			if (side == (int)ShoreSide.Left) {
				moveController.SetMovePos(new Vector3(2.5F, -1, 0));
				boatModel.SetSide((int)ShoreSide.Right);
			} else {
				moveController.SetMovePos(new Vector3(-2.5F, -1, 0));
				boatModel.SetSide((int)ShoreSide.Left);
			}
		}

		/* 获取船上空闲的位置 */
		public int GetFreeIndex() {
			string [] roleOnBoat = boatModel.GetRoleOnBoat();
			// 按照顺序获取船上空闲的位置
			if (roleOnBoat[0] == "") {
				return 0;
			} else if (roleOnBoat[1] == "") {
				return 1;
			} else {
				// 如果没有空闲位置返回-1
				return -1;
			}
		}

		/* 判断船上是否没有角色 */
		public bool IsNoRole() {
			string [] roleOnBoat = boatModel.GetRoleOnBoat();
			if (roleOnBoat[0] != "" || roleOnBoat[1] != "") {
				return false;
			} else {
				return true;
			}
		}

		/* 根据船靠岸的方向获取对应方向里船上空闲位置 */
		public Vector3 GetFreePosition() {
			string [] roleOnBoat = boatModel.GetRoleOnBoat();
			int freeIndex = GetFreeIndex();
			// 如果没有空闲位置，直接返回
			if (freeIndex < 0) {
				return new Vector3(-1, -1, -1);
			}
			int side = boatModel.GetSide();

			if (side == (int)ShoreSide.Left) {
				return leftLoadPos[freeIndex];
			} else {
				return rightLoadPos[freeIndex];
			}
		}

		/* 将角色放到船上 */
		public void PutRoleOnBoat(string roleName) {
			// 获取船上空闲位置
			int freeIndex = GetFreeIndex();
			// 如果没有空闲位置，直接返回
			if (freeIndex < 0) {
				return;
			}
			// 利用角色名字将角色放入船
			string [] roleOnBoat = boatModel.GetRoleOnBoat();
			roleOnBoat[freeIndex] = roleName;
		}

		/* 将角色离开船上 */
		public void PutRoleOffBoat(string roleName) {
			string [] roleOnBoat = boatModel.GetRoleOnBoat();
			// 如果船的相应位置有角色名字，将其置为空
			for (int i = 0; i < roleOnBoat.Length; i++) {
				if (roleOnBoat[i] == roleName) {
					roleOnBoat [i] = "";
					break;
				}
			}
		}

		/* 获取船上的各个角色的数目 */
		public int[] GetRoleCountOnBoat() {
			int [] count = {0, 0};
			string [] roleOnBoat = boatModel.GetRoleOnBoat();
			for (int i = 0; i < roleOnBoat.Length; i++) {
				string roleName = roleOnBoat[i];
				if (roleName == "") continue;
				if (roleName.Remove(roleName.Length - 1, 1) == "Priest") {
					count[0]++;
				} else {
					count[1]++;
				}
			}
			return count;
		}
	}

	/* 岸控制器 */
	public class ShoreController {
		private ShoreModel shoreModel; // 岸模型
		private ShoreView shoreView; // 岸视图

		// 右岸上可以放角色的位置，左岸的位置只需将坐标的x数值乘-1即可
		private static Vector3 [] positions = new Vector3[] {new Vector3(4.5F, 0.25F, 0), new Vector3(5.5F, 0.25F, 0), new Vector3(6.5F, 0.25F, 0), 
				new Vector3(7.5F, 0.25F, 0), new Vector3(8.5F, 0.25F, 0), new Vector3(9.5F, 0.25F, 0)};

		public ShoreController(int side, string sideName) {
			shoreModel = new ShoreModel(side);
			shoreView = new ShoreView(side, sideName);
		}

		public void InitShoreController() {
			shoreModel.SetRoleOnShore(new string [6]{"", "", "", "", "", ""});
		}

		/* 获取岸边 */
		public int GetShoreSide() {
			return shoreModel.GetSide();
		}

		/* 获取岸上空闲的位置 */
		public int GetFreeIndex() {
			string [] roleOnShore = shoreModel.GetRoleOnShore();
			for (int i = 0; i < roleOnShore.Length; i++) {
				if (roleOnShore[i] == "") {
					return i;
				}
			}
			// 如果没有空闲位置返回-1
			return -1;
		}

		/* 根据船靠岸的方向获取对应方向里岸上空闲位置 */
		public Vector3 GetFreePosition() {
			int freeIndex = GetFreeIndex();
			// 如果没有空闲位置，直接返回
			if (freeIndex < 0) {
				return new Vector3(-1, -1, -1);
			}
			Vector3 pos = positions[freeIndex];
			// 坐标的x数值乘1是右边岸上的位置，乘-1即可得到左边岸上的位置
			pos.x *= shoreModel.GetSide();
			return pos;
		}

		/* 将角色放到岸上 */
		public void PutRoleOnShore(string roleName) {
			// 获取岸上空闲位置
			int freeIndex = GetFreeIndex();
			// 如果没有空闲位置，直接返回
			if (freeIndex < 0) {
				return;
			}
			// 利用角色名字将角色放上岸
			string [] roleOnShore = shoreModel.GetRoleOnShore();
			roleOnShore[freeIndex] = roleName;
		}

		/* 将角色离开岸上 */
		public void PutRoleOffShore(string roleName) {
			string [] roleOnShore = shoreModel.GetRoleOnShore();
			// 如果岸的相应位置有角色名字，将其置为空
			for (int i = 0; i < roleOnShore.Length; i++) {
				if (roleOnShore[i] == roleName) {
					roleOnShore [i] = "";
					break;
				}
			}
		}

		/* 获取岸上的各个角色的数目 */
		public int [] GetRoleCountOnShore() {
			int [] count = {0, 0};
			string [] roleOnShore = shoreModel.GetRoleOnShore();
			for (int i = 0; i < roleOnShore.Length; i++) {
				string roleName = roleOnShore[i];
				if (roleName == "") continue;
				if (roleName.Remove(roleName.Length - 1, 1) == "Priest") {
					count[0]++;
				} else {
					count[1]++;
				}
			}
			return count;
		}
	}

	/* 角色控制器 */
	public class RoleController {
		private RoleModel roleModel; // 角色模型
		private RoleView roleView; // 角色视图

		private MoveController moveController; // 移动控制器
		private ClickController clickController; // 点击控制器

		private ShoreController shoreController; // 角色所在相应岸上的岸控制器

		public RoleController(string roleName, int number) {
			roleModel = new RoleModel();
			roleView = new RoleView(roleName, number);
			
			// 添加移动控制器
			moveController = roleView.AddMoveScript();
			// 添加点击控制器，并记录点击对象的控制器
			clickController = roleView.AddClickScript();
			clickController.SetClickRole(this);
		}

		public void InitRoleController() {
			// 初始化移动控制器
			moveController.InitMoveController();
			// 获取主控制器中的右岸控制器
			shoreController = (SingleObject.getInstance().GetMainController() as MainController).GetRightShore();
			// 将角色放到右岸上
			PutRoleOnShore(shoreController);
			SetRolePos(shoreController.GetFreePosition());
			// 右岸控制器中关于角色的信息也被初始化
			shoreController.PutRoleOnShore(this.GetRoleViewName());
		}

		public ShoreController GetShoreController() {
			return shoreController;
		}

		/* 设置角色游戏对象位置 */
		public void SetRolePos(Vector3 pos) {
			roleView.SetPos(pos);
		}

		/* 将角色移动到指定位置 */
		public void MoveToPosition(Vector3 endPosination) {
			moveController.SetMovePos(endPosination);
		}

		/* 获取角色游戏对象名字 */
		public string GetRoleViewName() {
			return roleView.GetName();
		}

		/* 获取游戏对象是否在船上 */
		public bool IsOnBoat() {
			return roleModel.GetIsOnBoat();
		}

		/* 将角色放到船上 */
		public void PutRoleOnBoat(BoatController boatController) {
			// 设置角色不在岸上，所以也没有岸控制器
			shoreController = null;
			// 设置角色在船上
			roleModel.SetIsOnBoat(true);
			// 设置角色游戏对象的父对象是船只
			roleView.SetRoleParentTrans(boatController.GetBoat().transform);
		}

		/* 将角色放到岸上 */
		public void PutRoleOnShore(ShoreController shoreController) {
			// 设置角色在相应岸上的岸控制器
			this.shoreController = shoreController;
			// 设置角色不在船上
			roleModel.SetIsOnBoat(false);
			// 设置角色游戏对象的父对象为没有
			roleView.SetRoleParentTrans(null);
		}
	}

	/* 主控制器 */
	public class MainController : MonoBehaviour {

		private BoatController boat; // 船控制器
		private ShoreController leftShore; // 左岸控制器
		private ShoreController rightShore; // 右岸控制器
		private RoleController [] roles; // 所有角色的控制器
		private RiverView riverView; // 河流视图
		private FunctionView functionView; // 功能视图，包括提示框，游戏信息等

		private int gameState = (int)GameState.Playing; // 游戏状态，0表示游戏正在进行，1表示游戏失败，2表示游戏成功

		void Awake() {
			// 设置单例对象中只有唯一一个主控制器
			SingleObject instance = SingleObject.getInstance();
			instance.SetMainController(this);
			// 展现游戏视图
			ShowView();
		}

		/* 通过创建控制器的方式初始化游戏视图 */
		public void ShowView() {
			boat = new BoatController();
			leftShore = new ShoreController((int)ShoreSide.Left, "LeftShore");
			rightShore = new ShoreController((int)ShoreSide.Right, "RightShore");
			roles = new RoleController[6];
			CreateRolesController();
			riverView = new RiverView();
			functionView = gameObject.AddComponent<FunctionView>() as FunctionView;
		}

		/* 为每个角色创建控制器并初始化视图 */
		private void CreateRolesController() {
			for (int i = 0; i < 3; i++) {
				roles[i] = new RoleController("Priest", i + 1);
				roles[i].SetRolePos(rightShore.GetFreePosition());
				roles[i].PutRoleOnShore(rightShore);
				rightShore.PutRoleOnShore(roles[i].GetRoleViewName());
			}

			for (int i = 0; i < 3; i++) {
				roles[i + 3] = new RoleController("Devil", i + 1);
				roles[i + 3].SetRolePos(rightShore.GetFreePosition());
				roles[i + 3].PutRoleOnShore(rightShore);
				rightShore.PutRoleOnShore(roles[i + 3].GetRoleViewName());
			}
		}

		/* 通过初始化控制器和视图的方式初始化游戏 */
		public void InitGame() {
			boat.InitBoatController();
			leftShore.InitShoreController();
			rightShore.InitShoreController();
			for (int i = 0; i < roles.Length; i++) {
				roles[i].InitRoleController();
			}
			functionView.InitFunctionView();
			gameState = (int)GameState.Playing;
		}

		public ShoreController GetRightShore() {
			return rightShore;
		}

		/* 根据不同的游戏状态返回不同的提示框内容 */
		public string StateToTip(int gameState) {
			switch (gameState) {
				case (int)GameState.Playing:
					return "Click and Play the Game";
				case (int)GameState.Lose:
					return "You lose! Click \"Reset\" to play again!";
				case (int)GameState.Win:
					return "You win! Click \"Reset\" to play again!";
				default:
					return "";
			}
		}

		/* 通过船控制器来移动船只 */
		public void MoveBoat() {
			// 如果游戏状态不是正在进行，那么不移动
			if (gameState != (int)GameState.Playing) {
				return;
			}
			// 如果船上没有角色，不能开船
			if (boat.IsNoRole()) {
				return;
			} else {
				// 否则直接将船开到另一边
				boat.MoveToOtherSide();
			}
			// 检查游戏状态
			gameState = CheckGameState();
			// 显示相应提示
			functionView.SetTipContent(StateToTip(gameState));
		}

		/* 通过角色控制器移动角色 */
		public void MoveRole(RoleController role) {
			// 如果游戏状态不是正在进行，那么不移动
			if (gameState != (int)GameState.Playing) {
				return;
			}
			// 如果角色在船上，那么要将角色放上相应岸边
			if (role.IsOnBoat()) {
				// 获取船所在的岸边
				ShoreController boatShore;
				if (boat.GetBoatSide() == (int)ShoreSide.Left) {
					boatShore = leftShore;
				} else {
					boatShore = rightShore;
				}

				/* 设置船和岸以及角色控制器以将角色放到岸上 */
				role.MoveToPosition(boatShore.GetFreePosition());
				boat.PutRoleOffBoat(role.GetRoleViewName());
				role.PutRoleOnShore(boatShore);
				boatShore.PutRoleOnShore(role.GetRoleViewName());
			} else {
				// 否则角色在岸上，要将角色放到船上，首先获取角色所在岸边的控制器
				ShoreController boatShore = role.GetShoreController();

				// 如果船上没有位置，或者船和物体所在的岸不是一边，直接返回
				if (boat.GetFreeIndex() == -1 || boatShore.GetShoreSide() != boat.GetBoatSide()) {
					return;
				}

				/* 设置船和岸以及角色控制器以将角色放到船上 */
				role.MoveToPosition(boat.GetFreePosition());
				boat.PutRoleOnBoat(role.GetRoleViewName());
				role.PutRoleOnBoat(boat);
				boatShore.PutRoleOffShore(role.GetRoleViewName());
			}
			// 检查游戏状态
			gameState = CheckGameState();
			// 显示相应提示
			functionView.SetTipContent(StateToTip(gameState));
		}

		/* 检查游戏状态 */
		public int CheckGameState() {
			// 初始化左边和右边的牧师和魔鬼数量都为0
			int leftPriest = 0, rightPriest = 0;
			int leftDevil = 0, rightDevil = 0;

			// 获取左岸和右岸以及船上的牧师和魔鬼的数量数组
			int [] leftShoreCount = leftShore.GetRoleCountOnShore();
			int [] rightShoreCount = rightShore.GetRoleCountOnShore();
			int [] boatCount = boat.GetRoleCountOnBoat();

			// 如果船在左边，那么左边的牧师和魔鬼的数量为左岸数量加上船里的数量
			if (boat.GetBoatSide() == (int)ShoreSide.Left) {
				leftPriest = leftShoreCount[0] + boatCount[0];
				leftDevil = leftShoreCount[1] + boatCount[1];
				rightPriest = rightShoreCount[0];
				rightDevil = rightShoreCount[1]; 
			} else {
				// 如果船在右边，那么右边的牧师和魔鬼的数量为右岸数量加上船里的数量
				leftPriest = leftShoreCount[0];
				leftDevil = leftShoreCount[1];
				rightPriest = rightShoreCount[0] + boatCount[0];
				rightDevil = rightShoreCount[1] + boatCount[1]; 
			}

			// 如果左边牧师加魔鬼的数量等于6且魔鬼全部上岸，那么游戏成功
			if (leftPriest + leftDevil == 6 && leftShoreCount[1] == 3) {
				return (int)GameState.Win;
			} else if (leftPriest < leftDevil && leftPriest > 0) {
				// 如果任意一边牧师的数量小于魔鬼的数量且有牧师，那么游戏失败
				return (int)GameState.Lose;
			} else if (rightPriest < rightDevil && rightPriest > 0) {
				return (int)GameState.Lose;
			} else {
				// 否则游戏正在进行中
				return (int)GameState.Playing;
			}
		}
	}

	/* 单例对象，里面有唯一一个主控制类 */
	public class SingleObject {
		private static SingleObject instance = new SingleObject();
		private MainController mainController;

		public static SingleObject getInstance() {
			return instance;
		}

		public void SetMainController(MainController controller) {
			mainController = controller;
		}

		public MainController GetMainController() {
			return mainController;
		}
	}
}