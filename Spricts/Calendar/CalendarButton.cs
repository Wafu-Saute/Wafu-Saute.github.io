using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// カレンダーの日付のボタンロジックのスプリクト
/// 選択した日付にイベントデータが存在すれば、そのアイテムを生成し、
/// リストで表示する
/// </summary>
public class CalendarButton : MonoBehaviour
{
    // カレンダーを管理するクラス
    private CalendarManager calendar;
    // ボタン
    private Button button;
    // 日付出力用
    private Text text;
    // イベント表示用のクラス
    private SpawnItem spawnItem;

    /// <summary>マスの日時</summary>
    private DateTime _dateValue;
    /// <summary>ボタン番号</summary>
    private int _index;

    // カラー用変数
    private static string dodayColor = "#00FFF1";
    private static string dayColor = "#FFFFFF";

    public DateTime dateValue
    {
        get { return _dateValue; }
        set { _dateValue = value; }
    }

    public int index
    {
        get { return _index; }
        set { _index = value; }
    }

    // Use this for initialization
    void Start()
    {
        spawnItem = FindObjectOfType<SpawnItem>();
        calendar = FindObjectOfType<CalendarManager>();
        button = GetComponent<Button>();
        text = button.GetComponentInChildren<Text>();
        text.fontSize = 30;

        this.ObserveEveryValueChanged(date => date._dateValue)
            .Subscribe(_ =>
            {
                text.text = _dateValue.isDefault() ? "" : _dateValue.Day.ToString();
                text.color = GetColorDayOfWeek(_dateValue.DayOfWeek);
                if (_dateValue == DateTime.Today)
                {
                    Color newCol;
                    if (ColorUtility.TryParseHtmlString(dodayColor, out newCol))
                    {
                        GetComponent<Image>().color = newCol;
                    }
                        
                }
                else
                {
                    Color newCol;
                    if (ColorUtility.TryParseHtmlString(dayColor, out newCol))
                    {
                        GetComponent<Image>().color = newCol;
                    }
                }

            });
        button.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log("タップした日付：" + _dateValue);
                    spawnItem.DeleteObject();
                    foreach (var eventItem in EventItemController.EventItemList)
                    {
                        DateTime eventday = DateTime.Parse(eventItem.CreateTime);
                        eventday = DateTime.Parse(eventday.Year.ToString() + " " + eventday.Month.ToString() + " " + eventday.Day.ToString());
                        DateTime nowTime = DateTime.Parse(_dateValue.Year.ToString() + " " + _dateValue.Month.ToString() + " " + _dateValue.Day.ToString());
                        if (eventday.Equals(nowTime))
                        {
                            spawnItem.AddItem(eventItem);
                        }

                    }
                });

    }

    /// <summary>色を指定</summary>
    Color GetColorDayOfWeek(DayOfWeek dayOfWeek)
    {
        if (_dateValue.Month == calendar.current.Month)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return Color.blue;
                case DayOfWeek.Sunday:
                    return Color.red;
                default:
                    return Color.black;
            }
        }
        else
        {
            return Color.gray;
        }
    }
}

/// <summary>DateTime拡張</summary>
public static class DateTimeExtension
{
    /// <summary>デフォルトの0001/01/01が入る</summary>
    static DateTime Default = new DateTime();
    /// <summary>デフォルト値と比較</summary>
    public static bool isDefault(this DateTime d) { return d.Equals(Default); }
}