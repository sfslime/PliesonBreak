using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
6,ゲーム開始関数

(メンバー)
1,接続確認、待機
2,プレイヤー生成
3,アイテム受信待機
4,看守受信待機
5,初期化完了、待機
6,ゲーム開始関数

作成者：飛田
 */

[AddComponentMenu("Managers/GAME/GameManager",10)]

public class GameManager : MonoBehaviour
{

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

    //生成したプレイヤーの見た目（オンラインオブジェクト）
    private GameObject PlayerSprite;

    [SerializeField, Tooltip("出現させるプレイヤーのプレファブ名")] string PlayerPrefabName;

    [SerializeField, Tooltip("牢屋の場所")] GameObject PrisonPoint;

    [SerializeField, Tooltip("看守の出現場所の元兼出現先")] GameObject JailersRoot;

    [SerializeField, Tooltip("看守の巡回先")] GameObject PatrolRoot;

    [SerializeField, Tooltip("ゲームの進行状態（エリアの解放状態）")] int ReleaseErea;

    [SerializeField, Header("エリア解放時設定"), Tooltip("脱出アイテム・表示時間・音量・SEなどの設定")] ReleaseEffectSetting ReleaseEffectSettings;

    [SerializeField, Header("ゴール処理関係"), Tooltip("必要なアイテム・終了時に呼ばれる関数など")] EscapeSetting EscapeSettings;

    [SerializeField, Header("アイテム関係"), Tooltip("アイテム変更用画像")] List<Sprite> InteractSprits = new List<Sprite>();

    [SerializeField, Tooltip("アイテム出現用プレファブ")] List<GameObject> interactObjectPrefabs = new List<GameObject>();

    private InitList InitLists = new InitList();

    //現在はテストで結果を記録
    public static bool GameResult;

    #endregion

    #region マネージャー変数

    //ゲームマネージャーのインスタンス
    public static GameManager GameManagerInstance;

    //マップを管理するマネージャー
    private MapManager MapManager;

    //SEを管理するマネージャー
    private AudioManager AudioManager;

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

        GameStatus = GAMESTATUS.READY;

        InitLists.ConectInit = false;
        InitLists.MapInit = false;

        //プレイヤーの探索
        //Player = GameObject.Find("")
        //プレイヤーを動けなくする処理

        //チュートリアル内で捕まった情報をリセット
        PhotonNetwork.LocalPlayer.SetArrestStatus(false);

        AudioManager = GameObject.Find("AudioManager").gameObject.GetComponent<AudioManager>();
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

        BGMManager.Instance.SetBGM(BGMid.DEFALTGAME);

        Debug.Log("Start OK");
    }

    /// <summary>
    /// 各種初期化が終わったかをチェックする
    /// </summary>
    bool InitCheck()
    {
        if (InitLists.ConectInit && InitLists.MapInit) return true;
        return false;
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
       AudioManager.PlaySE(id, pos);
    }

    /// <summary>
    /// 看守を生成する
    /// </summary>
    void PopJailer()
    {
        for(int i=0;i< JailersRoot.transform.childCount;i++)
        {
            var obj = PhotonNetwork.Instantiate("Jailer", JailersRoot.transform.GetChild(i).transform.position, Quaternion.identity);
            for(int j = 0; j < PatrolRoot.transform.GetChild(i).childCount; j++)
            {
                obj.GetComponent<JailerLink>().AddPatrolPoint(PatrolRoot.transform.GetChild(i).GetChild(j).position);
            }
            
        }
    }

    /// <summary>
    /// プレイヤーを捕まえたときに看守から呼ばれる
    /// ローカルプレイヤーであればプレイヤーの場所や画面に演出を起こし、
    /// 他プレイヤーであれば画面にメッセージを表示する
    /// </summary>
    /// <param name="player"></param>
    public void ArrestPlayer(GameObject player)
    {
        StartCoroutine(ArrestEffect(player));
    }

    /// <summary>
    /// 牢屋が解放されたときにメッセージを表示する
    /// 牢屋から呼ばれる
    /// </summary>
    public void ReleasePrison()
    {
        //解放メッセージ

        //テストで全員を解放
        foreach(var player in PhotonNetwork.PlayerList)
        {
            player.SetArrestStatus(false);
        }
    }

    /// <summary>
    /// 捕縛状態から解放する
    /// 牢屋の柵に触れたときに呼ばれ、カウント減少を停止する
    /// </summary>
    /// <param name="player"></param>
    public void ReleasePlayer(GameObject player)
    {
        if (PlayerSprite.GetComponent<PlayerLink>().GetOrigin() == player)
        {
            if (PhotonNetwork.LocalPlayer.GetArrestStatus())
            {
                PhotonNetwork.LocalPlayer.SetArrestStatus(false);
            }
        }
    }

    /// <summary>
    /// ゲーム開始前に呼ぶ待機関数
    /// UpDate内で呼ばれる
    /// </summary>
    void Ready()
    {
        if (InitCheck())
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GameStart();
            //    Debug.Log("GameStart");
            //}
            GameStart();
            Debug.Log("GameStart");
        }
    }

    /// <summary>
    /// ローカルプレイヤーのカウントを減少させる
    /// 他プレイヤーはそれぞれのゲームマネージャーで減少させる
    /// その結果を表示する
    /// </summary>
    void ArrestCountUpDate()
    {
        if (PhotonNetwork.LocalPlayer.GetArrestStatus())
        {
            PhotonNetwork.LocalPlayer.SetArrestCntStatus(PhotonNetwork.LocalPlayer.GetArrestCntStatus() - Time.deltaTime);
        }

        //全員つかまっているか判定する
        bool isAllArrest = true;
        foreach(var player in PhotonNetwork.PlayerList)
        {
            if(player == PhotonNetwork.LocalPlayer)
            {
                if (player.GetArrestStatus())
                {
                    //自身のカウントを表示
                    player.GetArrestCntStatus();
                }
                else
                {
                    //通常画像を表示

                    isAllArrest = false;
                }
            }
            else
            {
                if (player.GetArrestStatus())
                {
                    //他プレイヤーのカウントを表示
                    player.GetArrestCntStatus();
                }
                else
                {
                    //通常画像を表示
                    isAllArrest = false;
                }
            }
        }
        if (isAllArrest)
        {
            GameOver();
        }
    }

    /// <summary>
    /// ゴールにすべてのアイテムが入った時にゴールから呼ばれる
    /// チュートリアル中かどうかによって処理を派生させる
    /// </summary>
    public void GameClear()
    {
        if (EscapeSettings.isGoal) return;
        EscapeSettings.isGoal = true;
        if(SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString())
        {
            TutorialManager.Instance.TutorialTrriger(true);
            return;
        }
        GameResult = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(SceanNames.ENDGAME.ToString());
        }
    }

    /// <summary>
    /// 全員が捕まった時に呼ばれる
    /// ゲーム状態を終了にセットし、敗北状態で次のシーンに移る
    /// </summary>
    void GameOver()
    {
        if (SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString() || GameStatus == GAMESTATUS.ENDGAME_LOSE) return;

        GameStatus = GAMESTATUS.ENDGAME_LOSE;
        GameResult = false;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(SceanNames.ENDGAME.ToString());
        }
    }

    /// <summary>
    /// 起動時に牢屋から呼ばれ、捕まった際にここに転送される
    /// </summary>
    /// <param name="point"></param>
    public void Setprisonpoint(GameObject point)
    {
        PrisonPoint = point;
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

    /// <summary>
    /// プレイヤーオブジェクトを返す
    /// ゲームオブジェクトなので必要に応じてGetComponentする
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return Player;
    }

    /// <summary>
    /// ゴールに必要なアイテム情報を返す
    /// 主にゴールから呼ばれる
    /// </summary>
    /// <returns></returns>
    public List<InteractObjs> GetNeedItemList()
    {
        return EscapeSettings.NeedEscapeList;
    }

    /// <summary>
    /// 現在のゲーム状態を返す
    /// </summary>
    /// <returns></returns>
    public GAMESTATUS GetGameStatus()
    {
        return GameStatus;
    }


    #endregion

    #region コルーチン

    /// <summary>
    /// 捕まった時の演出等を順番に処理する
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator ArrestEffect(GameObject player)
    {
        if (PlayerSprite.GetComponent<PlayerLink>().GetOrigin() == player)
        {
            //捕まり演出(その場にエフェクト、SE、画面にメッセージ、アニメーション)

            player.transform.position = PrisonPoint.transform.position;

            if (SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString())
            {
                yield break;
            }

            //捕まり処理（カウント開始や捕まり状態送信）
            PhotonNetwork.LocalPlayer.SetArrestStatus(true);
        }
        else
        {
            //捕まり演出(画面にメッセージ)
        }

        yield break;
    }

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
        yield return StartCoroutine(MapManager.StartPop());
        Debug.Log("Map OK");
        InitLists.MapInit = true;
        yield break;
    }

    /// <summary>
    /// シーン移動後に各種初期化を行う
    /// それぞれの初期化後に他のプレイヤーの待機を行う
    /// </summary>
    /// <returns></returns>
    IEnumerator InitGame()
    {
        //1,接続確認して他プレイヤーを待つ
        //接続まで待機して現在の情報（初期化中）を設定
        while(!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LocalPlayer.SetGameStatus((int)GameStatus);
        //すべてのプレイヤーの状態を確認
        while (true)
        {
            //状態確認用
            bool isReady = true;
            //すべてのプレイヤーに対して確認
            foreach(var player in PhotonNetwork.PlayerListOthers)
            {
                //初期化中でないなら変更
                if(player.GetGameStatus() != (int)GAMESTATUS.READY)
                {
                    isReady = false;
                }
            }

            //すべてのプレイヤーが初期化中なら進む
            if (isReady) break;
            //フリーズ回避で1フレーム待機
            yield return null;
        }

        InitLists.ConectInit = true;
        Debug.Log("すべてのプレイヤーの接続確認");

        //2,プレイヤー生成
        var Link = PhotonNetwork.Instantiate(PlayerPrefabName, Player.transform.position, Quaternion.identity);
        Link.GetComponent<PlayerLink>().SetOrigin(Player);
        Link.GetComponent<Collider2D>().enabled = false;
        PlayerSprite = Link;

        //3,(マスター)アイテム生成、送信(メンバー)アイテム受信待機
        if(PhotonNetwork.LocalPlayer.IsMasterClient == true)
        {
            yield return StartCoroutine(WaitMapPop());

            //生成したアイテム位置を必要とする場合、MapManagerから取得し送信
            //(現在は必要ないため生成のみ)

            //4,看守生成
            PopJailer();
        }
        else
        {
            //行うことはないのでスキップ
            InitLists.MapInit = true;
            Debug.Log("Map Ok");
        }

        //5,初期化待機

        //自身の初期化完了を送信
        PhotonNetwork.LocalPlayer.SetInitStatus(true);
        while (true)
        {
            //状態確認用
            bool isReady = true;
            //すべてのプレイヤーに対して確認
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                //初期化中でないなら変更
                if (player.GetInitStatus() != true)
                {
                    isReady = false;
                }
            }

            //すべてのプレイヤーが初期化中なら進む
            if (isReady) break;
            //フリーズ回避で1フレーム待機
            yield return null;
        }

        Debug.Log("全プレイヤー初期化完了");
        yield break;
    }


    #endregion

    private void Awake()
    {
        if (Init())
        {
            StartCoroutine(InitGame());
        }else if(SceneManager.GetActiveScene().name == SceanNames.TUTORIAL.ToString())
        {
            //チュートリアルの例外処理
            var Link = PhotonNetwork.Instantiate(PlayerPrefabName, Player.transform.position, Quaternion.identity);
            Link.GetComponent<PlayerLink>().SetOrigin(Player);
            Link.GetComponent<Collider2D>().enabled = false;
            PlayerSprite = Link;
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        switch (GameStatus)
        {
            case GAMESTATUS.READY:
                Ready();
                break;

            case GAMESTATUS.INGAME:
                ArrestCountUpDate();
                break;

            default:
                break;
        }
    }
}
