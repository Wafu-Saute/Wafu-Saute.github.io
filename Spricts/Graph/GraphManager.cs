using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// グラフを作成したり、その他ボタン処理等を管理するスプリクト
/// </summary>
public class GraphManager : MonoBehaviour
{
    // 合計表示
    [SerializeField] 
    Text sumText;
    // 年
    [SerializeField]
    Text yearText;
    // 月
    [SerializeField] 
    Text monthText;
    // グラフ詳細用アイテム
    [SerializeField] 
    GraphItemSpawn graphItemSpawn;
    // グラフの親
    public Transform roulette;
    // 円グラフの要素の元
    public GameObject plate;
    // 割合のリスト
    private int[] sizeList;
    // 合計金額
    decimal sumAmount = 0;
    // 収入 or　支出
    bool isIncome;
    // 指定した月
    int month;
    // 指定した年
    int year;

    [System.Serializable]
    public class GraphItem
    {
        public int count;
        public bool Income;                 // true:収入 false:支出
        public string Genre;                // ジャンル　(光熱費や交通費、娯楽などの項目を選択)
        public decimal Amount;              // 金額
        public int persent;
        public Vector4 color;

    }
    public static List<GraphItem> GraphItemList = new List<GraphItem>();

    void Start()
    {
        month = DateTime.Now.Month;
        year = DateTime.Now.Year;
        isIncome = false;
        Awake();

    }

    void Awake()
    {
        if (!EventItemController.isLoad)
        {
            EventItemController.LoadAllEventItemData();
        }
        if (!GenreController.isLoad)
        {
            GenreController.LoadAllGenreItemData();
        }
        Init();
        InitGraphItems();
        StartCoroutine(ShowAnim());
    }

    /// <summary>
    /// 円グラフのリセット
    /// </summary>
    public void Reset()
    {
        Init();
    }

    /// <summary>
    /// 前月ボタン押下処理
    /// </summary>
    public void PrevButtonTap()
    {
        if(month == 1 )
        {
            if(year > 0)
            {
                month = 12;
                year -= 1;
            }
            
        }else
        {
            month -= 1;
        }

        Awake();
    }

    /// <summary>
    /// 次月ボタン押下処理
    /// </summary>
    public void NextButtonTap()
    {
        if (month == 12)
        {
            if (year > 0)
            {
                month = 1;
                year += 1;
            }

        }
        else
        {
            month += 1;
        }

        Awake();
    }

    /// <summary>
    /// 円グラフの描画処理
    /// </summary>
    public void Show()
    {
        StartCoroutine(ShowAnim());
    }

    /// <summary>
    /// 円グラフが表示される演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowAnim()
    {
        bool flg = true;
        roulette.GetComponent<Image>().fillAmount = 0;
        float speed = 0.02f;
        while (flg)
        {
            roulette.GetComponent<Image>().fillAmount += speed;
            if (roulette.GetComponent<Image>().fillAmount >= 1) flg = false;
            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// グラフデータの初期化
    /// </summary>
    private void Init()
    {
        Create();

        sumAmount = 0;
        foreach (GraphItem graphItem in GraphItemList)
        {
            sumAmount = decimal.Add(sumAmount, graphItem.Amount);
        }

        sumText.text = "合計\n¥ " + sumAmount.ToString("N0");

        yearText.text = year.ToString() + "年";
        monthText.text = month.ToString() + "月";

        // 既に作成したグラフがあれば削除する
        foreach (Transform tran in roulette)
        {
            if (tran.name != "Plate")
                Destroy(tran.gameObject);
        }

        int kindCount = GraphItemList.Count;  // 最大の色の数
        sizeList = new int[kindCount];       // 割合のリスト
        int max = 100;                      // グラフの比率 100が最大　ここからどんどん引いていく
        for (int i = 0; i < kindCount; i++)
        {
            if (max <= 0) break;
            // Plateをコピーしてサイズなどを調節
            GameObject plateCopy = Instantiate(plate) as GameObject;
            plateCopy.transform.SetParent(roulette);
            plateCopy.transform.localPosition = Vector3.zero;
            plateCopy.transform.localScale = Vector3.one;

            // 最後の１つの場合は残りの全てをあてはめる
            if (i == kindCount - 1)
                sizeList[i] = max;
            else
            {
                sizeList[i] = (int)(GraphItemList[i].Amount / sumAmount * 100m);
            }
            // zの角度を設定
            plateCopy.transform.localEulerAngles = new Vector3(0, 0, (100f - (float)max) / 100f * -360f);

            // 円のサイズをfillAmountに設定
            plateCopy.GetComponent<Image>().fillAmount = (float)sizeList[i] / 100f;

            // 色をランダムに設定 明るめにしてます
            plateCopy.GetComponent<Image>().color = new Vector4(UnityEngine.Random.Range(0.6f, 1f), UnityEngine.Random.Range(0.6f, 1f), UnityEngine.Random.Range(0.6f, 1f), 1);
            plateCopy.SetActive(true);
            max -= sizeList[i];
            GraphItemList[i].color = plateCopy.GetComponent<Image>().color;
            GraphItemList[i].persent = sizeList[i];

        }
        roulette.GetComponent<Image>().fillAmount = 1;
    }

    /// <summary>
    /// グラフの詳細用データを作成
    /// </summary>
    private void Create()
    {
        GraphItemList.Clear();

        SpawnItem spawnItem = new SpawnItem();

        foreach(EventItemController.EventItem item in EventItemController.EventItemList)
        {
            if(item.CreateTimeYear == year && item.CreateTimeMonth == month && item.Income == isIncome)
            {
                bool isadded = false;
                int i = 0;
                foreach (var genreItem in GraphItemList)
                {
                    if (spawnItem.GetGenre(item.Genre).Equals(genreItem.Genre))
                    {
                        GraphItemList[i].count++;
                        GraphItemList[i].Amount += item.Amount;
                        isadded = true;
                    }
                    i++;
                }

                if(!isadded)
                {
                    GraphItem graphItem = new GraphItem();
                    graphItem.count = 1;
                    graphItem.Income = item.Income;
                    graphItem.Genre = spawnItem.GetGenre(item.Genre);
                    graphItem.Amount = item.Amount;
                    GraphItemList.Add(graphItem);
                }
            }
            
        }


    }

    /// <summary>
    /// グラフの詳細リストアイテムを作成
    /// </summary>
    private void InitGraphItems()
    {
        graphItemSpawn.DeleteObject();
        foreach (GraphItem graphItem in GraphItemList)
        {
            graphItemSpawn.AddItem(graphItem);
        }
    }
}
