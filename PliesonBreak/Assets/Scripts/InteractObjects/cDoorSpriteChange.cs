using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class cDoorSpriteChange : MonoBehaviour
{
    //�ύX����^�C���}�b�v�i�������g�j
    Tilemap tilemap;
    //�ύX��̍��E�̉摜
    [SerializeField, Header("�ύX��摜")] TileBase LeftOpenTile;
    [SerializeField] TileBase RightOpenTile;
    //�ύX����^�C�����W�B�O���b�h���W�ň���
    [SerializeField, Header("�ύX�����W�i�O���b�h���W�j")] List<Vector3Int> LeftDoorPositions = new List<Vector3Int>();
    [SerializeField] List<Vector3Int> RightDoorPositions = new List<Vector3Int>();
    // Start is called before the first frame update
    void Start()
    {
        //�^�C���}�b�v���擾
        tilemap = GetComponent<Tilemap>();
        //�e�X�g
        //ChangeTile(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �T�v�F�h�A�̃^�C���摜��ύX����B
    /// ����:Isopen,�J���Ă��邩�ǂ���
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
