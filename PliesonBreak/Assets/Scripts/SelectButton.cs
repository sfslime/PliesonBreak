using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
�L�[�{�[�h����Ń{�^������͂ł���悤�ɂ���
�{�^���ɃA�E�g���C��������ꍇ�́A�I������Ă����
�A�E�g���C�����I���ɂȂ�B
�Ȃ���Βǉ����ăI���ɂ���B�����\���������Ȃ��Ȃ�
OutLineSetting��isLine��false�ɂ���
 */
public class SelectButton : MonoBehaviour
{
    //�I���o����{�^���̃��X�g
    [System.Serializable]
    class ButtonRow
    {
        public List<GameObject> Buttons;
    }

    //���͂��󂯕t����L�[
    [System.Serializable]
    class InputButtonKey
    {
        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Right;
        public KeyCode Left;
        public KeyCode Select;
    }

    //�f�t�H���g�Œǉ�����A�E�g���C���̐ݒ�
    [System.Serializable]
    class OutLineSetting
    {
        public Color Color;
        public float Thickness;
        public bool isLine;
    }

    [SerializeField, Tooltip("�f�t�H���g�̃A�E�g���C���ݒ�")] OutLineSetting OutLineSettings;
    [SerializeField, Tooltip("���͂��󂯎��L�[")] InputButtonKey InputButtonKeys;
    [SerializeField, Tooltip("�I���ł���{�^���BElement��ɂ����")] List<ButtonRow> ButtonRows;

    //�I�����ꂽ�ꍇ�̐��l
    public const float Selected = 99;

    //���ݑI�𒆂̃{�^��
    private Vector2 NowSelect;

    // Start is called before the first frame update
    void Start()
    {
        NowSelect = new Vector2(0, 0);

        //��x�S�Ẵ{�^�����m�F
        if (OutLineSettings.isLine)
        {
            foreach (var ButtonRow in ButtonRows)
            {
                foreach (var Button in ButtonRow.Buttons)
                {
                    OutLineCheck(Button).enabled = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SelectUpDate();
    }

    void SelectUpDate()
    {
        
        //�O�ɑI������Ă����{�^���̃A�E�g���C��������
        if(OutLineSettings.isLine) OutLineCheck(ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x]).enabled = false;

        //���͂𔽉f
        var input = InputKey();
        //����Ȃ炻�̃{�^�����N��
        if (input.x == Selected)
        {
            ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x].GetComponent<Button>().onClick.Invoke();
            return;
        }
        NowSelect += input;

        //0��艺�������ő�l�ɂ���
        if (NowSelect.y < 0) NowSelect.y = (ButtonRows.Count == 1) ? 0 : ButtonRows.Count - 1;
        if (NowSelect.x < 0) NowSelect.x = (ButtonRows[(int)NowSelect.y].Buttons.Count == 1) ? 0 : ButtonRows[(int)NowSelect.y].Buttons.Count - 1;
        //�ő�l�𒴂�����0�ɖ߂�
        if (NowSelect.y >= ButtonRows.Count) NowSelect.y = 0;
        if (NowSelect.x >= ButtonRows[(int)NowSelect.y].Buttons.Count) NowSelect.x = 0;

        //�A�E�g���C����\��
        if (OutLineSettings.isLine) OutLineCheck(ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x]).enabled = true;
    }

    /// <summary>
    /// �L�[�̓��͎�t
    /// ����Ȃ�Selected�̒l�ɂ��ĕԂ�
    /// </summary>
    /// <returns></returns>
    Vector2 InputKey()
    {
        Vector2 input = new Vector2(0, 0);
        if (Input.GetKeyDown(InputButtonKeys.Up)) input.y = -1;
        if (Input.GetKeyDown(InputButtonKeys.Down)) input.y = 1;
        if (Input.GetKeyDown(InputButtonKeys.Left)) input.x = -1;
        if (Input.GetKeyDown(InputButtonKeys.Right)) input.x = 1;

        if (Input.GetKeyDown(InputButtonKeys.Select)) input = new Vector2(Selected, Selected);

        return input;
    }

    /// <summary>
    /// OutLine�����邩�ǂ����`�F�b�N���A����ΕԂ�
    /// �Ȃ����OutLineSetting�̍��ڂɊ�Â��Ēǉ�����
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Outline OutLineCheck(GameObject obj)
    {
        var outline = obj.GetComponent<Outline>();
        if (outline == null)
        {
            outline = obj.AddComponent<Outline>();
            outline.effectColor = OutLineSettings.Color;
            outline.effectDistance = new Vector2(OutLineSettings.Thickness, -OutLineSettings.Thickness);
        }
        return outline;
    }
}
