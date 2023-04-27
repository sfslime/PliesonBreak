using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
全体の進行を管理するマネージャー
作成者：飛田
 */

public class GameManager : MonoBehaviour
{
    #region 列挙体

    /// <summary>
    /// ゲームの現在の状態を表す列挙体
    /// </summary>
    enum GAMESTATUS
    {
        READY,   //ゲーム開始前
        INGAME,  //ゲーム中
        ENDGAME, //ゲーム終了後
        COUNT    //この列挙体の数
    }

    /// <summary>
    /// ゲームの進行具合を表す列挙体
    /// </summary>
    enum GAMEFAZES
    {
        EXPLORE,  //探索中。最終部屋前までの状態
        LAST,     //最終部屋
        COUNT     //この列挙体の数
    }

    #endregion

    #region 変数宣言

    #region ゲーム進行変数

    [SerializeField, Tooltip("現在のゲームの状態")] GAMESTATUS GameStatus { get; set; }

    [SerializeField, Tooltip("プレイヤークラス")] Player Player;

    #endregion

    #region マネージャー変数

    //ゲームマネージャーのインスタンス
    public static GameManager GameManagerInstance;

    //マップを管理するマネージャー
    //private MapManager MapManager;

    #endregion

    #endregion

    #region 関数

    bool Init()
    {
        GameManagerInstance = this;

        return true;

    }



    /// <summary>
    /// ゲーム起動時に呼ばれる関数
    /// 
    /// </summary>
    public void GameStart()
    {
        //プレイヤーの実体宣言
        //Player = プレイヤーの探索

        //各マネージャーの起動、エラーチェック
        //MapManager = マップマネージャーの検索
        //if(MapManager == null)
        //{
        //    Debug.Log("MapManager Not Find");
        //}else{
        //マップの生成
        //プレイヤーのリスポーン位置の取得
        //}

        //テスト
        GameStatus = GAMESTATUS.INGAME;

        Debug.Log("Start Ok");
    }


    #endregion

    void Start()
    {
        Init();
        //テスト
        GameStart();
    }

    
    void Update()
    {
        
    }
}
