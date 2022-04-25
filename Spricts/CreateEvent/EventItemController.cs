using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
///  イベントアイテムをロードやセーブするスプリクト
/// </summary>
public class EventItemController : MonoBehaviour
{
    [System.Serializable]
    public class EventItem
    {
        public int ID;
        public string CreateTime;     // 作成時間
        public int CreateTimeMonth;
        public int CreateTimeYear;
        public bool Income;             // true:収入 false:支出
        public int Amount;          // 金額
        public string Medium;           // 媒体　現金とかクレカとか・・・
        public int Genre;               // ジャンル　(光熱費や交通費、娯楽などの項目を選択)
        public string Memo = "";             // 詳しい事柄とか

        // 固定変数
        public static string SCreateTime = "CreateTime";
        public static string SCreateTimeMonth = "CreateTimeMonth";
        public static string SCreateTimeYear = "CreateTimeYear";
        public static string SIncome = "Income";
        public static string SAmount = "Amount";
        public static string SMedium = "Medium";
        public static string SGenre = "Genre";
        public static string SMemo = "Memo";
        //-----
    }

    public static List<EventItem> EventItemList = new List<EventItem>();
    public static bool isLoad = false;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// イベントデータのセーブ
    /// </summary>
    /// <param name="eventItem"></param>
    public static void saveEventItemData(EventItem eventItem)
    {
        EventItemList.Add(eventItem);

        PlayerPrefs.SetInt("AllEventItem", EventItemList.Count);
        //Debug.Log("セーブデータ更新：" + PlayerPrefs.GetInt("AllEventItem"));

        // データをセーブ
        PlayerPrefs.SetString(EventItem.SCreateTime+ eventItem.ID, eventItem.CreateTime);
        PlayerPrefs.SetInt(EventItem.SCreateTimeMonth + eventItem.ID, eventItem.CreateTimeMonth);
        PlayerPrefs.SetInt(EventItem.SCreateTimeYear + eventItem.ID, eventItem.CreateTimeYear);
        PlayerPrefs.SetInt(EventItem.SIncome + eventItem.ID, eventItem.Income? 1:0);
        PlayerPrefs.SetInt(EventItem.SAmount + eventItem.ID, eventItem.Amount);
        PlayerPrefs.SetString(EventItem.SMedium + eventItem.ID, eventItem.Medium);
        PlayerPrefs.SetInt(EventItem.SGenre + eventItem.ID, eventItem.Genre);
        PlayerPrefs.SetString(EventItem.SMemo + eventItem.ID, eventItem.Memo);
        //-----

    }

    /// <summary>
    /// 全てのイベントデータをロードする
    /// </summary>
    /// <returns></returns>
    public static List<EventItem> LoadAllEventItemData()
    {
        EventItemList.Clear();
        int eventNum = PlayerPrefs.GetInt("AllEventItem");
        //Debug.Log("セーブデータ："+ eventNum);
        for (int i = 1; i <= eventNum; i++ )
        {
            // データをロード
            EventItem item = new EventItem();
            item.CreateTime = PlayerPrefs.GetString(EventItem.SCreateTime + i, "");
            item.CreateTimeMonth = PlayerPrefs.GetInt(EventItem.SCreateTimeMonth + i, 0);
            item.CreateTimeYear = PlayerPrefs.GetInt(EventItem.SCreateTimeYear + i, 0);
            item.Income = PlayerPrefs.GetInt(EventItem.SIncome + i, 0) == 1? true:false;
            item.Amount = PlayerPrefs.GetInt(EventItem.SAmount + i, 0);
            item.Medium = PlayerPrefs.GetString(EventItem.SMedium + i, "");
            item.Genre = PlayerPrefs.GetInt(EventItem.SGenre + i, 0);
            item.Memo = PlayerPrefs.GetString(EventItem.SMemo + i, "");
            //-----
            EventItemList.Add(item);
        }

        isLoad = true;
        return EventItemList;
    }

    /// <summary>
    /// イベントアイテムを削除
    /// </summary>
    /// <param name="deleteItem"></param>
    /// <returns></returns>
    public static bool DeleteItem(EventItem deleteItem)
    {
        try
        {
            EventItemList.Remove(deleteItem);
            PlayerPrefs.DeleteKey(EventItem.SCreateTime + deleteItem.ID);
            PlayerPrefs.DeleteKey(EventItem.SCreateTimeMonth + deleteItem.ID);
            PlayerPrefs.DeleteKey(EventItem.SCreateTimeYear + deleteItem.ID);
            PlayerPrefs.DeleteKey(EventItem.SIncome + deleteItem.ID);
            PlayerPrefs.DeleteKey(EventItem.SAmount + deleteItem.ID);
            PlayerPrefs.DeleteKey(EventItem.SMedium + deleteItem.ID);
            PlayerPrefs.DeleteKey(EventItem.SGenre + deleteItem.ID);
            PlayerPrefs.DeleteKey(EventItem.SMemo + deleteItem.ID);
            PlayerPrefs.SetInt("AllEventItem", EventItemList.Count);
        }
        catch(Exception ex)
        {
            //Debug.Log("セーブエラー：" + ex);
            return false;
        }
        return true;
    }

}
