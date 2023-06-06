using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using ConstList;

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

    #region クラス内クラス

    [System.Serializable]
    class ReleaseEffectSetting
    {
        [Range(1,5)] public float ActiveTime;
        public float FontSize;
        public AudioClip EffectSE;
        public float SEvolume;
        public GameObject EffectPanel;
    }

    [System.Serializable]
    class EscapeSetting
    {
        public bool isGoal;
        public List<InteractObjs> NeedEscapeList = new List<InteractObjs>();
    }

    #endregion

    #region 変数宣言

    #region ゲーム進行変数

    [SerializeField, Tooltip("現在のゲームの状態")] GAMESTATUS GameStatus { get; set; }

    [SerializeField, Tooltip("プレイヤークラス(testでインスペクターから)")] GameObject Player;

    [SerializeField, Tooltip("ゲームの進行状態（エリアの解放状態）")] int ReleaseErea;

    [SerializeField, Header("エリア解放時設定"), Tooltip("脱出アイテム・表示時間・音量・SEなどの設定")] ReleaseEffectSetting ReleaseEffectSettings;

    [SerializeField, Header("ゴール処理関係"), Tooltip("必要なアイテム・終了時に呼ばれる関数など")] EscapeSetting EscapeSettings;

    [SerializeField, Header("アイテム関係"), Tooltip("アイテム変更用画像")] List<Sprite> InteractSprits = new List<Sprite>();

    [SerializeField, Tooltip("アイテム出現用プレファブ")] List<GameObject> interactObjectPrefabs = new List<GameObject>();

    #endregion

    #region マネージャー変数

    //ゲームマネージャーのインスタンス
    public static GameManager GameManagerInstance;

    //マップを管理するマネージャー
    //private MapManager MapManager;

    //SEを管理するマネージャー
    private AudioManager AudioManager;

    #endregion

    #endregion

    #region 関数

    bool Init()
    {
        GameManagerInstance = this;

        AudioManager = GetComponent<AudioManager>();
        if(AudioManager == null)
        {
            Debug.Log("AudioManager not found");
            return false;
        }

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
        ReleaseErea = 0;

        Debug.Log("Start OK");
    }

    /// <summary>
    /// 概要：次のエリアに続くドアが開いた時に、ドアから呼ばれる関数
    /// 　　　開いたエリア番号を各ユーザーの画面に表示させる
    /// 　　　この時、EreaReleaseEffectコルーチンで演出を行う
    /// 引数：EreaNm>開いたエリアの番号を受け取る
    /// </summary>
    /// <param name="EreaNm"></param>
    public void EreaRelease(int EreaNm)
    {
        //正常に空いているかを判定
        if (ReleaseErea + 1 != EreaNm)
        {
            Debug.Log("EreaNm Error!"); 
            return;
        }

        ReleaseErea = EreaNm;
        StartCoroutine(EreaReleaseEffect(EreaNm));
    }

    public void PlaySE(SEid id,Vector2 pos)
    {
        AudioManager.SE(id, pos);
    }

    /// <summary>
    /// アイテムの画像変更用に引数に応じてスプライトを返す
    /// </summary>
    /// <param name="ObjID"></param>
    /// <returns></returns>
    public Sprite ReturnSprite(InteractObjs ObjID)
    {
        return InteractSprits[(int)ObjID];
    }

    /// <summary>
    /// アイテム出現用に引数に応じてゲームオブジェクトを返す
    /// </summary>
    /// <param name="ObjectID"></param>
    /// <returns></returns>
    public GameObject GetObjectPrefab(InteractObjs ObjectID)
    {
        return interactObjectPrefabs[(int)ObjectID];
    }

    public GameObject GetPlayer()
    {
        return Player;
    }

    public List<InteractObjs> GetNeedItemList()
    {
        return EscapeSettings.NeedEscapeList;
    }


    #endregion

    #region コルーチン

    /// <summary>
    /// エリア解放時の演出コルーチン
    /// 一定時間後に演出終了処理を行う
    /// </summary>
    /// <param name="EreaNm"></param>
    /// <returns></returns>
    IEnumerator EreaReleaseEffect(int EreaNm)
    {
        Debug.Log(EreaNm + " Erea Release");
        //ReleaseEffectSettings.EffectPanel.SetActive(true);

        //SEを鳴らす

        while (true)
        {
            //ここでゆっくりと表示
            break;
        }
        Debug.Log("Release Effect Wait");

        yield return new WaitForSeconds(ReleaseEffectSettings.ActiveTime);

        while (true)
        {
            //ゆっくりと消える
            break;
        }

        Debug.Log("Release Effect End");
        yield break;
    }

    #endregion

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
        //テスト
        GameStart();
    }

    
    void Update()
    {
        
    }
}
