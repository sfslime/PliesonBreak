using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class cDoorSpriteChange : MonoBehaviour
{
    //変更するタイルマップ（自分自身）
    Tilemap tilemap;
    //変更先の左右の画像
    [SerializeField, Header("変更先画像")] TileBase LeftOpenTile;
    [SerializeField] TileBase RightOpenTile;
    //変更するタイル座標。グリッド座標で扱う
    [SerializeField, Header("変更元座標（グリッド座標）")] List<Vector3Int> LeftDoorPositions = new List<Vector3Int>();
    [SerializeField] List<Vector3Int> RightDoorPositions = new List<Vector3Int>();
    // Start is called before the first frame update
    void Start()
    {
        //タイルマップを取得
        tilemap = GetComponent<Tilemap>();
        //テスト
        //ChangeTile(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 概要：ドアのタイル画像を変更する。
    /// 引数:Isopen,開いているかどうか
    /// </summary>
    /// <param name="isOpen"></param>
    public void ChangeTile(bool isOpen)
    {
        if (isOpen)
        {
            foreach (Vector3Int pos in LeftDoorPositions)
            {
                tilemap.SetTile(pos, LeftOpenTile);
            }
            foreach (Vector3Int pos in RightDoorPositions)
            {
                tilemap.SetTile(pos, RightOpenTile);
            }
        }
    }
}
