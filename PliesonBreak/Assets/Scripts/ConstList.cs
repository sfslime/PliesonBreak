using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace ConstList
{
    public enum AnimCode
    {
        None,
        Idel,
        Walk,
        Run,
        Search
    }

    public enum InteractObjs
    {
        None,
        Key,
        Door,
        NullDrop,
        Search,
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
    }

    public enum ItemID
    {
        None,
        Key,
        EscapeItem1,
        EscapeItem2,
        Count
    }

    public enum SEid
    {
        None,
        Search,
        SearchHit,
        SearchNull
    }

    public enum SceanNames
    {
        TITLE,
        WAITROOM,
        GAME,
        COUNT
    }

    public static class PhotonCustumPropertie
    {
        private const string GameStatusKey = "Gs";
        private const string InitStatusKey = "Is";

        private static readonly ExitGames.Client.Photon.Hashtable propsToSet = new ExitGames.Client.Photon.Hashtable();

        /// <summary>
        /// 引数でPhotonのプレイヤーを渡すことで
        /// 戻り値でGameStatusが返ってくる。int型で返るので、キャストする
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int GetGameStatus(this Player player)
        {
            return (player.CustomProperties[GameStatusKey] is int status) ? status : 0;
        }

        /// <summary>
        /// 引数でPhotonのプレイヤーとGameStatusを渡すことで
        /// 他プレイヤーに送信する
        /// </summary>
        /// <param name="player"></param>
        public static void SetGameStatus(this Player player, int status)
        {
            propsToSet[GameStatusKey] = status;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }

        /// <summary>
        /// 引数でPhotonのプレイヤーを渡すことで
        /// 戻り値でそのプレイヤーの初期化情報が返る
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool GetInitStatus(this Player player)
        {
            return (player.CustomProperties[InitStatusKey] is bool status) ? status : false;
        }

        /// <summary>
        /// 引数でPhotonのプレイヤーと初期化状態を渡すことで
        /// 他プレイヤーに送信する
        /// </summary>
        /// <param name="player"></param>
        public static void SetInitStatus(this Player player, bool status)
        {
            propsToSet[InitStatusKey] = status;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }
    }
}
