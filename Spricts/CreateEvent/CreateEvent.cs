using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EventItemController;
using static GenreController;

/// <summary>
/// イベント作成スプリクト
/// 入力したデータを変数で受け取り、
/// そのデータを元にイベントを作成する
/// </summary>
public class CreateEvent : MonoBehaviour
{
    // イベントデータのクラス
    EventItem eventItem;
    // 日付の入力欄
    [Header("日付入力用Input")]
    [SerializeField] 
    private InputField dateText;
    // 時刻の入力欄
    [Header("日付入力用Input")]
    [SerializeField] 
    private InputField timeText;
    // 時刻の入力欄
    [Header("時刻入力用Input")]
    [SerializeField] 
    private InputField amountText;
    // アカウントのドロップダウン
    [Header("アカウント入力用Dropdown")]
    [SerializeField] 
    private Dropdown accountDropdown;
    // 項目用のドロップダウン
    [Header("項目入力用Dropdown")]
    [SerializeField] 
    private Dropdown genreDropdown;
    // メモの入力欄
    [Header("メモ入力用Input")]
    [SerializeField]
    private Text memoText;
    // エラーメッセージ
    private string errorMessage = "";
    // 日付保存用変数
    private string date = "";
    // 時刻保存用変数
    private string time = "";

    // Start is called before the first frame update
    void Start()
    {
        eventItem = new EventItem();
        LoadAllEventItemData();
        LoadAllGenreItemData();
    }

    /// <summary>
    /// 日付自動入力ボタン押下時に発火
    /// </summary>
    public void AddDateTime()
    {
        
        string nowDate = DateTime.Now.ToString("yyyy/M/d");
        dateText.text = nowDate;
        string nowTime = DateTime.Now.ToString("HH:mm");
        timeText.text = nowTime;
        eventItem.CreateTime = nowDate +" "+ nowTime;

    }

    /// <summary>
    /// 日付入力時に発火
    /// </summary>
    /// <param name="value">日付</param>
    public void AddDateValue(string value)
    {
        date = value;
        eventItem.CreateTime = date + " " + time;
        
    }

    /// <summary>
    /// 時刻入力時に発火
    /// </summary>
    /// <param name="value">時刻</param>
    public void AddTimeValue(string value)
    {
        time = value;
        eventItem.CreateTime = date + " " + time;
    }

    /// <summary>
    /// 収入か支出かの選択肢を変更時に発火
    /// </summary>
    /// <param name="Value">true:収入, false:支出</param>
    public void AddIncomeValue(bool Value)
    {
        eventItem.Income = Value;
    }

    /// <summary>
    /// 金額を入力時に発火
    /// パースエラー時は0を代入する
    /// </summary>
    /// <param name="value">金額</param>
    public void AddAmountValue(string value)
    {
        try
        {
            eventItem.Amount = int.Parse(value);
        }
        catch (Exception ex)
        {
            eventItem.Amount = 0;
            amountText.text = "";
        }
    }

    /// <summary>
    /// アカウントを選択時に発火
    /// </summary>
    public void AddMediumValue()
    {
        eventItem.Medium = accountDropdown.options[accountDropdown.value].text;
    }

    /// <summary>
    /// 項目を選択時に発火
    /// </summary>
    public void AddTGenreValue()
    {
        
        try
        {
            eventItem.Genre = genreDropdown.value;
        }
        catch (Exception ex)
        {
            eventItem.Genre = 1;
        }
    }

    /// <summary>
    /// メモ入力時に発火
    /// </summary>
    /// <param name="value">メモ</param>
    public void AddMemoValue(string value)
    {
        eventItem.Memo = value;
    }

    /// <summary>
    /// イベント作成メソッド
    /// </summary>
    public void CreateItem()
    {
        Debug.Log("データチェック！");

        bool isOK = CheckData();

        if(isOK)
        {
            Debug.Log("セーブ実行");
            eventItem.ID = EventItemList.Count + 1;

            saveEventItemData(eventItem);
            DialogHandler sameHandler = DialogHandler.ShowDialog("作成完了！", "イベントの追加に成功しました！");
            sameHandler.onDestroyed += () => { SceneManager.LoadScene("Main"); };
            Debug.Log("処理完了");
        }
        else
        {
            DialogHandler.ShowDialog("エラー", errorMessage);
            errorMessage = "";
        }
    }

    /// <summary>
    /// データの中身がちゃんとできているかチェックするメソッド
    /// </summary>
    /// <returns></returns>
    private bool CheckData()
    {
        if (eventItem == null)
        {
            errorMessage = "予期せぬエラーです！";
            return false;
        }
        if (eventItem.Medium == "アカウントを選択")
        {
            errorMessage = "アカウントを入力してください！";
            return false;
        }
        else if (eventItem.Genre == 0)
        {
            errorMessage = "ジャンルを入力してください！";
            return false;
        }
        else if (string.IsNullOrEmpty(dateText.text))
        {
            errorMessage = "日付を入力してください！";
            return false;
        }
        else if (string.IsNullOrEmpty(timeText.text))
        {
            errorMessage = "時間を入力してください！";
            return false;
        }else if(eventItem.Memo != null && eventItem.Memo.Length > 50)
        {
            errorMessage = "メモは５０文字までです！";
            return false;
        }
        

        try
        {
            DateTime datetime = DateTime.Parse(eventItem.CreateTime);

            eventItem.CreateTimeMonth = datetime.Month;
            eventItem.CreateTimeYear = datetime.Year;

        }
        catch (FormatException ex)
        {
            Debug.Log("〜 エラー 〜/n"+ex);
            errorMessage = "日付、時間を正しく入力して下さい。\n入力する時は半角で入力して下さい。\n例：2019/1/1 18:30";
            return false;
        }

        return true;
    }
}
