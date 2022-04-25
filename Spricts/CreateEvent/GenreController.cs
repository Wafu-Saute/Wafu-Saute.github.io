using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントの項目を管理するスプリクト
/// </summary>
public class GenreController : MonoBehaviour
{
    [System.Serializable]
    public class GenreItem
    {
        public int ID;
        public string GenreName;               // ジャンル名　(光熱費や交通費、娯楽などの項目を選択)

        // 固定変数
        public static string SaveName = "Genre";
    }

    public static List<GenreItem> genreItemList = new List<GenreItem>();
    public static bool isLoad = false;

    void Start()
    {
        DontDestroyOnLoad(this);
        
    }

    /// <summary>
    /// 項目をセーブする
    /// </summary>
    /// <param name="genreItem"></param>
    public static void saveGenreItemData(GenreItem genreItem)
    {
        genreItemList.Add(genreItem);

        PlayerPrefs.SetInt("AllGenreItem", genreItemList.Count);
        //Debug.Log("セーブデータ更新：" + PlayerPrefs.GetInt("AllGenreItem"));
        PlayerPrefs.SetString(GenreItem.SaveName + genreItem.ID, genreItem.GenreName);
    }

    /// <summary>
    /// 全ての保存している項目をロード
    /// </summary>
    /// <returns></returns>
    public static List<GenreItem> LoadAllGenreItemData()
    {
        genreItemList.Clear();
        //PlayerPrefs.SetInt("AllGenreItem", 0);
        int eventNum = PlayerPrefs.GetInt("AllGenreItem");
        //Debug.Log("セーブデータ："+ eventNum);

        if(eventNum == 0)
        {
            CreateItemData();
        }
        else
        {
            for (int i = 1; i <= eventNum; i++)
            {
                GenreItem item = new GenreItem()
                {
                    ID = i,
                    GenreName = PlayerPrefs.GetString(GenreItem.SaveName + i, "")
                };
                genreItemList.Add(item);
            }

        }

        isLoad = true;
        return genreItemList;
    }

    /// <summary>
    /// 項目データがなかった場合に初期データを作成、保存する
    /// </summary>
    /// <returns></returns>
    public static bool CreateItemData()
    {
        PlayerPrefs.SetInt("AllGenreItem", 0);
        genreItemList.Clear();
        string[] addGenre = { "家賃","食費","水道・光熱費","交通費","医療費","娯楽","給与","その他"};

        int i = 1;
        foreach(string genre in addGenre)
        {
            GenreItem item = new GenreItem()
            {
                ID = i,
                GenreName = genre
            };
            PlayerPrefs.SetString(GenreItem.SaveName + item.ID, item.GenreName);

            genreItemList.Add(item);

            i++;
        }

        PlayerPrefs.SetInt("AllGenreItem", genreItemList.Count);
        //Debug.Log("セーブデータ更新：" + PlayerPrefs.GetInt("AllGenreItem"));

        return true;
    }

}
