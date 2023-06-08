using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
�S���̉摜�ύX���s��

�摜�ύX�̎w����Prison����s����
 */

public class cPrisonSpriteChange : MonoBehaviour
{
    //�ύX����^�C���}�b�v�i�������g�j
    Tilemap tilemap;
    //�ύX��̍��E�̉摜
    [SerializeField, Header("�ύX��摜")] TileBase UpOpenTile;
    [SerializeField] TileBase DownOpenTile;
    [SerializeField, Header("�ύX��摜")] TileBase UpCloseTile;
    [SerializeField] TileBase DownCloseTile;
    //�ύX����^�C�����W�B�O���b�h���W�ň���
    [SerializeField, Header("�ύX�����W�i�O���b�h���W�j")] List<Vector3Int> UpPrisonPositions = new List<Vector3Int>();
    [SerializeField] List<Vector3Int> DownPrisonPositions = new List<Vector3Int>();
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
