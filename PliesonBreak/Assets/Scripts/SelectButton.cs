using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
キーボード操作でボタンを入力できるようにする
ボタンにアウトラインがある場合は、選択されている間
アウトラインがオンになる。
なければ追加してオンにする。もし表示したくないなら
OutLineSettingのisLineをfalseにする
 */
public class SelectButton : MonoBehaviour
{
    //選択出来るボタンのリスト
    [System.Serializable]
    class ButtonRow
    {
        public List<GameObject> Buttons;
    }

    //入力を受け付けるキー
    [System.Serializable]
    class InputButtonKey
    {
        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Right;
        public KeyCode Left;
        public KeyCode Select;
    }

    //デフォルトで追加するアウトラインの設定
    [System.Serializable]
    class OutLineSetting
    {
        public Color Color;
        public float Thickness;
        public bool isLine;
    }

    [SerializeField, Tooltip("デフォルトのアウトライン設定")] OutLineSetting OutLineSettings;
    [SerializeField, Tooltip("入力を受け取るキー")] InputButtonKey InputButtonKeys;
    [SerializeField, Tooltip("選択できるボタン。Element一個につき一列")] List<ButtonRow> ButtonRows;

    //選択された場合の数値
    public const float Selected = 99;

    //現在選択中のボタン
    private Vector2 NowSelect;

    // Start is called before the first frame update
    void Start()
    {
        NowSelect = new Vector2(0, 0);

        //一度全てのボタンを確認
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
        
        //前に選択されていたボタンのアウトラインを消す
        if(OutLineSettings.isLine) OutLineCheck(ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x]).enabled = false;

        //入力を反映
        var input = InputKey();
        //決定ならそのボタンを起動
        if (input.x == Selected)
        {
            ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x].GetComponent<Button>().onClick.Invoke();
            return;
        }
        NowSelect += input;

        //0より下回ったら最大値にする
        if (NowSelect.y < 0) NowSelect.y = (ButtonRows.Count == 1) ? 0 : ButtonRows.Count - 1;
        if (NowSelect.x < 0) NowSelect.x = (ButtonRows[(int)NowSelect.y].Buttons.Count == 1) ? 0 : ButtonRows[(int)NowSelect.y].Buttons.Count - 1;
        //最大値を超えたら0に戻す
        if (NowSelect.y >= ButtonRows.Count) NowSelect.y = 0;
        if (NowSelect.x >= ButtonRows[(int)NowSelect.y].Buttons.Count) NowSelect.x = 0;

        //アウトラインを表示
        if (OutLineSettings.isLine) OutLineCheck(ButtonRows[(int)NowSelect.y].Buttons[(int)NowSelect.x]).enabled = true;
    }

    /// <summary>
    /// キーの入力受付
    /// 決定ならSelectedの値にして返す
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
    /// OutLineがあるかどうかチェックし、あれば返す
    /// なければOutLineSettingの項目に基づいて追加する
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
