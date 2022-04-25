using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CalendarManager : MonoBehaviour
{

    //ボタンの親オブジェクト
    [SerializeField]
    private GameObject calenderParent;
    //来月へ
    [SerializeField]
    private Button nextButton;
    //先月へ
    [SerializeField]
    private Button prevButton;
    //年表示テキスト
    [SerializeField]
    private Text yearText;
    //月表示テキスト
    [SerializeField]
    private Text monthText;
    //カレンダーの日時
    [SerializeField]
    private DateTime _current;
    //Buttonオブジェクト
    private GameObject[] objDays = new GameObject[42];
    //カレンダーの日付マス
    private CalendarButton[] Days = new CalendarButton[42];

    public DateTime current
    {
        get { return _current; }
    }

    // Use this for initialization
    void Start()
    {
        _current = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        InitCalendarComponent();
        SetCalendar();

        if (nextButton != null)
        {
            //押されたら起動
            nextButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    //一つ月を進める
                    _current = _current.AddMonths(1);
                    SetCalendar();
                });
        }
        if (prevButton != null)
        {
            prevButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    _current = _current.AddMonths(-1);
                    SetCalendar();
                });
        }
    }

    /// <summary>コンポーネントの取得、設定</summary>
    void InitCalendarComponent()
    {
        //行
        for (int i = 0; i < calenderParent.transform.childCount; i++)
        {
            //子オブジェクトを保存
            objDays[i] = calenderParent.transform.GetChild(i).gameObject;
            //コンポーネントを設定、取得
            Days[i] = objDays[i].AddComponent<CalendarButton>();
            Days[i].index = i + 1;
        }
    }

    /// <summary>カレンダーに日付をセット</summary>
    void SetCalendar()
    {
        int day = 1;
        //今月の1日目
        var first = new DateTime(_current.Year, _current.Month, day);

        // テキストの記入
        yearText.text = _current.Year.ToString() + "年";
        monthText.text = _current.Month.ToString() + "月";

        //来月
        var nextMonth = _current.AddMonths(1);
        int nextMonthDay = 1;
        //先月
        var prevMonth = _current.AddMonths(-1);
        //先月の場合は後ろから数える。
        int prevMonthDay =
            DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month) - (int)first.DayOfWeek + 1;

        foreach (var cDay in Days)
        {
            //今月の1日より前のマスには先月の日にちを入れる。
            if (cDay.index <= (int)first.DayOfWeek)
            {
                cDay.dateValue = new DateTime(prevMonth.Year, prevMonth.Month, prevMonthDay);
                prevMonthDay++;
            }
            //今月の最終日より後ろのマスには来月の日にちを入れる。
            else if (day > DateTime.DaysInMonth(_current.Year, _current.Month))
            {
                cDay.dateValue = new DateTime(nextMonth.Year, nextMonth.Month, nextMonthDay);
                nextMonthDay++;
            }
            //今月の日にちをマスに入れる。
            else
            {
                cDay.dateValue = new DateTime(_current.Year, _current.Month, day);
                day++;
            }
            
        }
    }
}