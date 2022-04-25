using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  キャラクターに会話させるデータ
///  このデータを元にタップ後のセリフを作成します。
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "Talk", menuName = "CreateData/CreateTalkData")]
public class TalkData : ScriptableObject
{
    /// <summary>
    /// タイトル
    /// </summary>
    [SerializeField]
    private string _title = "タイトル";

    /// <summary>
    /// 会話データのID
    /// </summary>
    [SerializeField]
    private int _id = 0;

    /// <summary>
    /// テキストデータ
    /// </summary>
    [SerializeField]
    private string _textContent = "";

    /// <summary>
    /// 音声データ
    /// </summary>
    [SerializeField]
    private AudioClip _audioData = null;

    /// <summary>
    /// ジャンル
    /// </summary>
    [SerializeField]
    private int _genreId = 0;

    /// <summary>
    /// 日付指定
    /// </summary>
    [SerializeField]
    private int _dateId = 0;

    /// <summary>
    /// 時間指定
    /// </summary>
    [SerializeField]
    private int _timeId = 0;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    List<StatChangeTiming> changeTimings = null;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string TextContent
    {
        get { return _textContent; }
        set { _textContent = value; }
    }

    public AudioClip AudioData
    {
        get { return _audioData; }
        set { _audioData = value; }
    }

    public int GenreId
    {
        get { return _genreId; }
        set { _genreId = value; }
    }

    public int DateId
    {
        get { return _dateId; }
        set { _dateId = value; }
    }

    public int TimeId
    {
        get { return _timeId; }
        set { _timeId = value; }
    }

    public List<StatChangeTiming> ChangeTimings
    {
        get { return changeTimings; }
        set { changeTimings = value; }
    }

}
