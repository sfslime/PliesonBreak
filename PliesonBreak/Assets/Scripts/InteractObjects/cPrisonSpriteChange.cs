using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
牢屋の画像変更を行う

画像変更の指示はPrisonから行われる
 */

public class cPrisonSpriteChange : MonoBehaviour
{
    //変更するタイルマップ（自分自身）
    Tilemap tilemap;
    //変更先の左右の画像
    [SerializeField, Header("変更先画像")] TileBase UpOpenTile;
    [SerializeField] TileBase DownOpenTile;
    [SerializeField, Header("変更先画像")] TileBase UpCloseTile;
    [SerializeField] TileBase DownCloseTile;
    //変更するタイル座標。グリッド座標で扱う
    [SerializeField, Header("変更元座標（グリッド座標）")] List<Vector3Int> UpPrisonPositions = new List<Vector3Int>();
    [SerializeField] List<Vector3Int> DownPrisonPositions = new List<Vector3Int>();
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
            foreach (Vector3Int pos in UpPrisonPositions)
            {
                tilemap.SetTile(pos, UpOpenTile);
            }
            foreach (Vector3Int pos in DownPrisonPositions)
            {
                tilemap.SetTile(pos, DownOpenTile);
            }
        }
        else
        {

            foreach (Vector3Int pos in UpPrisonPositions)
            {
                tilemap.SetTile(pos, UpCloseTile);
            }
            foreach (Vector3Int pos in DownPrisonPositions)
            {
                tilemap.SetTile(pos, DownCloseTile);
            }

        }
    }
}
