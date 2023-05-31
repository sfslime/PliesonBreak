using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using ConstList;
using Photon.Pun;
using Photon.Realtime;

/*
ゲームの進行を管理するマネージャー
すべてのプレイヤーが揃い、ゲームシーン遷移後から管理する
各マネージャーなどの初期化を始め、全員の準備が終わり次第開始する
開始後はエリア解放、捕縛、中止　のメッセージを受け取り、適切に処理する

初期化順
(マスター)
1,接続確認、待機
2,プレイヤー生成
3,アイテム生成、送信
4,看守生成、送信
5,初期化完了、待機
6,コネクトサーバークラスによるゲーム開始関数

(メンバー)
1,接続確認、待機
2,プレイヤー生成
3,アイテム受信待機
4,看守受信待機
5,初期化完了、待機
6,コネクトサーバークラスによるゲーム開始関数

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

    //解放時に関するクラス
    [System.Serializable]
    class ReleaseEffectSetting
    {
        [Range(1,5)] public float ActiveTime;
        public float FontSize;
        public AudioClip EffectSE;
        public float SEvolume;
        public GameObject EffectPanel;
    }

    //ゴールに関するクラス
    [System.Serializable]
    class EscapeSetting
    {
        public bool isGoal;
        public List<InteractObjs> NeedEscapeList = new List<InteractObjs>();
    }

    //初期化用管理クラス
    //各初期化の完了状態を参照できる
    [System.Serializable]
    class InitList
    {
        public bool MapInit;
        public bool ConectInit;
    }

    #endregion

    #region 変数宣言

    #region ゲーム進行変数

    [SerializeField, Tooltip("現在のゲームの状態")] GAMESTATUS GameStatus { get; set; }

    [SerializeField, Tooltip("プレイヤークラス(testでインスペクターから)")] GameObject Player;

    [SerializeField, Tooltip("ゲームの進行状態（エリアの解放状態）")] int ReleaseErea;

    [SerializeField, Tooltip("メッセージ表示UI(インスペクターから)")] Text MessageText;

    [SerializeField, Header("エリア解放時設定"), Tooltip("脱出アイテム・表示時間・音量・SEなどの設定")] ReleaseEffectSetting ReleaseEffectSettings;

    [SerializeField, Header("ゴール処理関係"), Tooltip("必要なアイテム・終了時に呼ばれる関数など")] EscapeSetting EscapeSettings;

    [SerializeField, Header("アイテム関係"), Tooltip("アイテム変更用画像")] List<Sprite> InteractSprits = new List<Sprite>();

    [SerializeField, Tooltip("アイテム出現用プレファブ")] List<GameObject> interactObjectPrefabs = new List<GameObject>();

    private InitList InitLists = new InitList();

    #endregion

    #region マネージャー変数

    //ゲームマネージャーのインスタンス
    public static GameManager GameManagerInstance;

    //マップを管理するマネージャー
    private MapManager MapManager;

    //SEを管理するマネージャー
    private AudioManager AudioManager;

    //オンライン接続マネージャー
    private ConectServer ConectServer;

    #endregion

    #endregion

    #region 関数

    /// <summary>
    /// 初期化関数
    /// 戻り値でエラーチェックになり、falseの場合は直ちに終了すること
    /// </summary>
    /// <returns></returns>
    bool Init()
    {
        GameManagerInstance = this;

        InitLists.ConectInit = false;
        InitLists.MapInit = false;

        //プレイヤーの探索
        //Player = GameObject.Find("")
        //プレイヤーを動けなくする処理

        AudioManager = GetComponent<AudioManager>();
        if(AudioManager == null)
        {
            Debug.Log("AudioManager not found");
            return false;
        }

        MapManager = GetComponent<MapManager>();
        if(MapManager == null)
        {
            Debug.Log("MapManager not found");
            return false;
        }

        ConectServer = GetComponent<ConectServer>();

        return true;

    }



    /// <summary>
    /// ゲーム起動時に呼ばれる関数
    /// 
    /// </summary>
    public void GameStart()
    {
        //テスト
        GameStatus = GAMESTATUS.INGAME;
        ReleaseErea = 0;

        //MessageText.transform.parent.gameObject.SetActive(false);

        Debug.Log("Start OK");
    }

    /// <summary>
    /// 各種初期化が終わったかをチェックする
    /// 現在はテストのため、マップの生成も行っている
    /// </summary>
    public void InitCheck()
    {
        if (!InitLists.ConectInit)
        {
            MessageText.text = "接続中...";
        }
        else
        {
            //動けるようにする
            //Player.GetComponent<PlayerBase>().

            if (PhotonNetwork.LocalPlayer.IsMasterClient == true)
            {
                MessageText.text = "参加人数 : " + PhotonNetwork.PlayerList.Length + "\nSPACEキーで開始";


                if (InitLists.MapInit)
                {
                    GameStart();
                }
                //テスト
                if (Input.GetKeyDown(KeyCode.Space) && !InitLists.MapInit)
                {
                    Debug.Log("マスタークライアント確認。アイテムの生成開始");
                    StartCoroutine(WaitMapPop());

                    //テスト
                    InitLists.MapInit = true;
                }
            }
            else
            {
                MessageText.text = "参加人数 : " + PhotonNetwork.PlayerList.Length + "\n待機中";

                //マスタークライアントからアイテム生成位置を受け取る
                InitLists.MapInit = true;
            }

        }
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
    /// 接続時にコネクトサーバーから呼ばれる
    /// </summary>
    public void RoomJoined()
    {
        InitLists.ConectInit = true;
        ConectServer.PopPlayer();
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

    /// <summary>
    /// マップの生成を待つ
    /// 全員が接続後、ゲームシーンに移った後にマスターが呼ぶ
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitMapPop() 
    {

        MessageText.text = "マップ生成中...(ローディング画面)";
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(MapManager.StartPop());
        Debug.Log("Map OK");
        InitLists.MapInit = true;
        //テスト
        MessageText.transform.parent.gameObject.SetActive(false);
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
    }

    
    void Update()
    {
        if(GameStatus == GAMESTATUS.READY)
        {
            InitCheck();
        }
    }
}
