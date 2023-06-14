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
        Key1,
        Key2,
        Key3,
        Door,
        Prison,
        NullDrop,
        Search,
        EscapeItem1,
        EscapeItem2,
        EscapeObj,
        OpenBearTrap,
        CloseBearTrap,
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
        private const string ArrestStatusKey = "As";
        private const string ArrestCntStatusKey = "ACs";

        private static readonly ExitGames.Client.Photon.Hashtable propsToSet = new ExitGames.Client.Photon.Hashtable();

        /// <summary>
        /// ������Photon�̃v���C���[��n�����Ƃ�
        /// �߂�l��GameStatus���Ԃ��Ă���Bint�^�ŕԂ�̂ŁA�L���X�g����
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int GetGameStatus(this Player player)
        {
            return (player.CustomProperties[GameStatusKey] is int status) ? status : 0;
        }

        /// <summary>
        /// ������Photon�̃v���C���[��GameStatus��n�����Ƃ�
        /// ���v���C���[�ɑ��M����
        /// </summary>
        /// <param name="player"></param>
        public static void SetGameStatus(this Player player, int status)
        {
            propsToSet[GameStatusKey] = status;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }

        /// <summary>
        /// ������Photon�̃v���C���[��n�����Ƃ�
        /// �߂�l�ł��̃v���C���[�̏�������񂪕Ԃ�
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool GetInitStatus(this Player player)
        {
            return (player.CustomProperties[InitStatusKey] is bool status) ? status : false;
        }

        /// <summary>
        /// ������Photon�̃v���C���[�Ə�������Ԃ�n�����Ƃ�
        /// ���v���C���[�ɑ��M����
        /// </summary>
        /// <param name="player"></param>
        public static void SetInitStatus(this Player player, bool status)
        {
            propsToSet[InitStatusKey] = status;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }

        /// <summary>
        /// ������Photon�̃v���C���[��n�����Ƃ�
        /// �߂�l�ł��̃v���C���[�̕ߔ���񂪕Ԃ�
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool GetArrestStatus(this Player player)
        {
            return (player.CustomProperties[ArrestStatusKey] is bool status) ? status : false;
        }

        /// <summary>
        /// ������Photon�̃v���C���[�ƕߔ���Ԃ�n�����Ƃ�
        /// ���v���C���[�ɑ��M����
        /// </summary>
        /// <param name="player"></param>
        public static void SetArrestStatus(this Player player, bool status)
        {
            propsToSet[ArrestStatusKey] = status;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }

        /// <summary>
        /// ������Photon�̃v���C���[��n�����Ƃ�
        /// �߂�l�ł��̃v���C���[�̕ߔ���񂪕Ԃ�
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static float GetArrestCntStatus(this Player player)
        {
            return (player.CustomProperties[ArrestCntStatusKey] is int cnt) ? cnt : 0;
        }

        /// <summary>
        /// ������Photon�̃v���C���[�ƕߔ���Ԃ�n�����Ƃ�
        /// ���v���C���[�ɑ��M����
        /// </summary>
        /// <param name="player"></param>
        public static void SetArrestCntStatus(this Player player, float cnt)
        {
            propsToSet[ArrestCntStatusKey] = cnt;
            player.SetCustomProperties(propsToSet);
            propsToSet.Clear();
        }
    }
}
