using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話データを管理するクラス
/// </summary>
public class TalkDataManager : MonoBehaviour
{
    // 会話を始める為のロジックが書かれたクラス
    private TextController textController;

    //リソースのパス
    [SerializeField]
    private string TalkDataPath = "Datas/TalkDatas";
    [SerializeField]
    private string DateTimeDataPath = "Datas/DateTimes";
    [SerializeField]
    private string TimeDataPath = "Datas/Times";
    [SerializeField]
    private string GenreDataPath = "Datas/Genres";
    //-----

    //全てのデータ
    [SerializeField]
    private TalkData[] talkDatas;
    [SerializeField]
    private DateTimeData[] dateTimeDatas;
    [SerializeField]
    private TimeData[] timeDatas;
    [SerializeField]
    private GenreData[] genreDatas;
    //-----

    //現在使用可能な会話データ
    [SerializeField]
    private List<TalkData> availableTalkDatas = new List<TalkData>();

    //選択されたデータID
    private int selectId;

    // Start is called before the first frame update
    void Start()
    {
        textController = GetComponent<TextController>();

        talkDatas = Resources.LoadAll<TalkData>(TalkDataPath);
        dateTimeDatas = Resources.LoadAll<DateTimeData>(DateTimeDataPath);
        timeDatas = Resources.LoadAll<TimeData>(TimeDataPath);
        genreDatas = Resources.LoadAll<GenreData>(GenreDataPath);

        SetAvailableTalkDatas();
        selectId = UnityEngine.Random.Range(0, availableTalkDatas.Count);
    }

    /// <summary>
    /// 現在の条件で再生可能な会話データを検索
    /// </summary>
    public void SetAvailableTalkDatas()
    {
        availableTalkDatas.Clear();
        foreach (TalkData data in talkDatas)
        {
            if((data.DateId == 0 || dateTimeDatas[data.DateId].Date.Value.Date == DateTime.Now.Date)&&(data.TimeId == 0))
            {
                availableTalkDatas.Add(data);
            }
        }
    }

    /// <summary>
    /// 会話開始
    /// </summary>
    public void SetInTextController()
    {
        textController.StartText(availableTalkDatas[selectId], 0);
        selectId = UnityEngine.Random.Range(0, availableTalkDatas.Count);
    }


}
