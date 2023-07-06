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
        public List<KeyCode> Up;
        public List<KeyCode> Down;
        public List<KeyCode> Right;
        public List<KeyCode> Left;
        public List<KeyCode> Select;
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
    [SerializeField, Tooltip("�{�^������C���^���N�e�B�u�ł��N�����邩")] bool isForcePush;

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

    /// <summary>
    /// �I���Ɋւ���UpDate
    /// </summary>
    void SelectUpDate()
    {
        
        //�O�ɑI������Ă����{�^���̃A�E�g���C��������
        if(OutLineSettings.isLine) OutLineCheck(ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x]).enabled = false;

        //���͂𔽉f
        var input = InputKey();
        //����Ȃ炻�̃{�^�����N��
        if (input.x == Selected)
        {
            var Button = ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x].GetComponent<Button>();
            Debug.Log(Button.interactable);
            if (Button.interactable)
            {
                Button.onClick.Invoke();
            }
            else
            {
                if(isForcePush) Button.onClick.Invoke();
            }
            
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

        //�e�L�[�̎�t�m�F
        foreach(var upkey in InputButtonKeys.Up)
        {
            if (Input.GetKeyDown(upkey)) input.y = -1;
        }
        foreach(var downkey in InputButtonKeys.Down)
        {
            if (Input.GetKeyDown(downkey)) input.y = 1;
        }
        foreach(var leftkey in InputButtonKeys.Left)
        {
            if (Input.GetKeyDown(leftkey)) input.x = -1;
        }
        foreach(var rightkey in InputButtonKeys.Right)
        {
            if (Input.GetKeyDown(rightkey)) input.x = 1;
        }
        
        foreach(var selectkey in InputButtonKeys.Select)
        {
            if (Input.GetKeyDown(selectkey)) input = new Vector2(Selected, Selected);
        }

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

    /// <summary>
    /// �{�^����ǉ�����
    /// �Ō���Ƀ��X�g�Ƃ��Ēǉ�
    /// </summary>
    /// <param name="ButtonList"></param>
    public void AddButton(List<GameObject> ButtonList)
    {
        ButtonRow buttonRow = new ButtonRow();
        buttonRow.Buttons = ButtonList;
        ButtonRows.Add(buttonRow);
    }
}
