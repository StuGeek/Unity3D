using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour {
    /* 井字棋中每一个棋格中的逻辑控制常量，代表这个棋格的状态 */
    private const int NOPLAYER = 0;  // 0代表这个棋格没有玩家
    private const int PLAYER1 = 1;  // 1代表玩家1占据这个棋格
    private const int PLAYER2 = 2;  // 2代表玩家2占据这个棋格

    /* 整个游戏需要用到的逻辑控制变量 */
    private int gameTurn = PLAYER1;  // 游戏回合，PLAYER1代表这个游戏回合是玩家1的，PLAYER2代表是玩家2的回合
    private int totalMoves = 0;  // 两个玩家总共进行的回合数
    private int totalPlayer = 0;  // 游戏的玩家数，0代表还未选择游戏模式，1代表与电脑进行对决，2代表双人游戏
    private int [,] chessBoard = new int [3, 3];  // 井字棋盘

    /* 井字棋棋格的布局设置 */
    private static int buttonWidth = 80;  // 每个棋格的宽度
    private static int buttonHeight = 80;  // 每个棋格的高度
    // 井字棋中最左上角第一个棋格的横坐标位置 
    private static int firstButtonX = (Screen.width - 3 * TicTacToe.buttonWidth) / 2;
    // 井字棋中最左上角第一个棋格的纵坐标位置 
    private static int firstButtonY = Screen.height - 3 * TicTacToe.buttonHeight - 10;
    
    /* 游戏界面设计用到的变量 */
    public Texture2D backgroundImage;  // 游戏背景图片
    public GUISkin gameSkin;  // 游戏控件的皮肤风格

    /* 初始化游戏 */
    void InitGame() {
        // 游戏回合从Player1开始
        gameTurn = PLAYER1;
        // 总进行回合数设为0
        totalMoves = 0;
        // 棋盘的每一格都还没被玩家占据
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                chessBoard[i, j] = NOPLAYER;
            }
        }
    }

    /* 单人游戏模式中电脑的移动 */
    void ComputerMove() {
        // 在0~8中选择一个随机数
        System.Random ran = new System.Random();
        int randomNum = ran.Next(0, 8);
        int xIndex, yIndex;
        do {
            // 算出随机数对应的横坐标和纵坐标
            xIndex = randomNum / 3;
            yIndex = randomNum % 3;
            // 如果棋盘上对应的棋格已有玩家，那么随机数加一寻找另外没有玩家的棋格
            randomNum = (randomNum + 1) % 8;
        } while (chessBoard[yIndex, xIndex] != NOPLAYER);
        // 设置棋格和游戏回合，总回合数加一
        chessBoard[yIndex, xIndex] = gameTurn;
        gameTurn = (gameTurn == PLAYER1 ? PLAYER2 : PLAYER1);
        totalMoves++;
        SetGameButton(xIndex, yIndex);
    }

    /* 检查是否有赢家 */
    int CheckWinner() {
        // 一共有8种赢的情况，首先检查3行3列的6种赢的情况
        for (int i = 0; i < 3; ++i) {
            if (chessBoard[i, 0] != NOPLAYER && 
                chessBoard[i, 0] == chessBoard[i, 1] && 
                chessBoard[i, 1] == chessBoard[i, 2]) {
                // 有玩家胜出，那么游戏回合置为NOPLAYER，返回这个玩家对应的值，1代表玩家1，2代表玩家2
                gameTurn = NOPLAYER;
                return chessBoard[i, 0];
            }
            if (chessBoard[0, i] != NOPLAYER && 
                chessBoard[0, i] == chessBoard[1, i] && 
                chessBoard[1, i] == chessBoard[2, i]) {
                gameTurn = NOPLAYER;
                return chessBoard[0, i];
            }
        }
        // 检查对角线的2种赢的情况
        if (chessBoard[1, 1] != NOPLAYER) {
            if ((chessBoard[0, 0] == chessBoard[1, 1] && 
                chessBoard[1, 1] == chessBoard[2, 2]) || 
                (chessBoard[0, 2] == chessBoard[1, 1] && 
                chessBoard[1, 1] == chessBoard[2, 0])) {
                gameTurn = NOPLAYER;
                return chessBoard[1, 1];
            }
        }
        // 没人胜出，那么返回NOPLAYER，代表没有赢家
        return NOPLAYER;
    }

    /* 添加游戏背景 */
    void AddBackground() {
        GUIStyle backgroundStyle = new GUIStyle();
        // 设置游戏背景图片
        backgroundStyle.normal.background = backgroundImage;
        GUI.Label(new Rect(0, 0, 710, 388), "", backgroundStyle);
    }

    /* 添加游戏标题 */
    void AddTitle() {
        // 设置标题样式
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 40;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.black;
        // 标题显示内容为TicTacToe Game
        GUI.Label(new Rect(Screen.width / 2 - 150, 20, 300, 50), "TicTacToe Game", titleStyle);
    }

    /* 添加提示框 */
    void AddTip() {
        // 选择Label的皮肤风格
        GUI.skin = gameSkin;
        SetTip();
    }

    /* 添加按钮实现的井字棋格和功能按钮 */
    void AddButton() {
        // 选择按钮的皮肤风格
        GUI.skin = gameSkin;
        // 添加井字棋格
        AddGameButton();
        // 添加重置按钮
        AddResetButton();
        // 添加游戏模式选择按钮
        AddChooseGameModeButton();
    }

    /* 添加井字棋格 */
    void AddGameButton() {
        // 按照坐标从左往右，从上往下设置棋格按钮的样式和功能
        for (int xIndex = 0; xIndex < 3; ++xIndex) {
            for (int yIndex = 0; yIndex < 3; ++yIndex) {
                SetGameButton(xIndex, yIndex);
            }
        }
    }

    /* 添加重置按钮 */
    void AddResetButton() {
        GUIStyle resetStyle = new GUIStyle("button");
        resetStyle.fontSize = 20;
        // 按下重置按钮，游戏被初始化
        if (GUI.Button(new Rect(firstButtonX + 3 * buttonWidth + 50, Screen.height - 70, 80, 50), "Reset", resetStyle)) {
            InitGame();
        }
    }

    /* 添加游戏模式选择按钮 */
    void AddChooseGameModeButton() {
        GUIStyle resetStyle = new GUIStyle("button");
        resetStyle.fontSize = 20;
        // 按下单人游戏模式按钮，游戏被初始化，游戏玩家人数变为1
        if (GUI.Button(new Rect(firstButtonX - 200, Screen.height - 2 * buttonHeight - 40, 180, 50), "1 Player Mode", resetStyle)) {
            InitGame();
            totalPlayer = 1;
        }
        // 按下双人游戏模式按钮，游戏被初始化，游戏玩家人数变为2
        if (GUI.Button(new Rect(firstButtonX - 200, Screen.height - buttonHeight - 40, 180, 50), "2 Players Mode", resetStyle)) {
            InitGame();
            totalPlayer = 2;
        }
    }

    /* 根据是否有赢家设置提示框中的内容 */
    void SetTip() {
        // 检查是否有赢家
        int winner = CheckWinner();
        switch (winner) {
            // 如果没有赢家
            case NOPLAYER:
                // 如果总玩家数为0，即还未选择游戏模式，那么提示选择游戏模式
                if (totalPlayer == 0) {
                    GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), "Please choose a game mode.");
                } else if (totalMoves == 0) {  // 如果总回合数为0，说明还未开始游戏，提示点击棋格并进行游戏
                    GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), "Click and play the game.");
                } else if (totalMoves == 9) {  // 如果总回合数为9，说明游戏结束且无玩家胜出，提示没有赢家
                    GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), "No Winner!");
                } else {
                    // 如果是单人模式且游戏正在进行，提醒正在进行单人游戏模式
                    if (totalPlayer == 1) {
                        GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), "1 Player Mode Playing...");
                    } else if (totalPlayer == 2) {  // 如果是双人模式且游戏正在进行，提醒正在进行双人游戏模式
                        GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), "2 Players Mode Playing...");
                    }
                }
                break;
            // 如果玩家1胜出
            case PLAYER1:
                // 提示框显示玩家1胜出
                GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), "Player1(O) Wins!");
                break;
            // 如果玩家2胜出
            case PLAYER2:
                // 提示框显示玩家2胜出
                GUI.Label(new Rect(Screen.width / 2 - 320, 70, 650, 50), "Player2(X) Wins!");
                break;
        }
    }

    /* 按照横坐标和纵坐标设置井字棋格 */
    void SetGameButton(int xIndex, int yIndex) {
        // 按照横纵坐标找到棋格相应的位置
        int buttonX = firstButtonX + xIndex * buttonWidth;
        int buttonY = firstButtonY + yIndex * buttonHeight;
        // 获取对应坐标中棋格的信息
        int Player = chessBoard[yIndex, xIndex];
        switch (Player) {
            // 如果这个棋格中为NOPLAYER
            case NOPLAYER:
                // 设置按钮中不显示任何内容
                if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "")) {
                    // 如果还未选择游戏模式，那么进行提示，且点击按钮无反应
                    if (totalPlayer == 0) {
                        // AddTip(NOPLAYER);
                        return;
                    }
                    // 选择游戏模式并点击棋格后，该棋格设置为这个游戏回合对应的玩家，游戏回合转换，总回合数加一
                    chessBoard[yIndex, xIndex] = gameTurn;
                    gameTurn = (gameTurn == PLAYER1 ? PLAYER2 : PLAYER1);
                    totalMoves++;
                    // 如果是单人游戏模式且游戏还未结束，那么电脑直接来走一步
                    if (totalPlayer == 1 && totalMoves < 9 && CheckWinner() == NOPLAYER) {
                        ComputerMove();
                    }
                }
                break;
            // 如果这个棋格中为PLAYER1
            case PLAYER1:
                // 棋格中显示内容为O
                GUI.Button(new Rect(buttonX, buttonY, buttonHeight, buttonWidth), "O");
                break;
            // 如果这个棋格中为PLAYER2
            case PLAYER2:
                // 棋格中显示内容为X
                GUI.Button(new Rect(buttonX, buttonY, buttonHeight, buttonWidth), "X");
                break;
        }
    }

    /* 进行游戏 */
    void PlayGameSystem() {
        AddBackground();  // 添加游戏背景
        AddTitle();  // 添加游戏标题
        AddTip();  // 添加提示框
        AddButton();  // 添加游戏按钮
    }

    void OnGUI() {
        PlayGameSystem();
    }
}
