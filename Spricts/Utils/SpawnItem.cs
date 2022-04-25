using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static EventItemController;

/// <summary>
///  イベントアイテムを生成するスプリクト
/// </summary>

public class SpawnItem : MonoBehaviour
{
    // イベントアイテムのプレハブ
    [SerializeField]
    private RectTransform prefab;
    // アイテムを並べる際の親
    [SerializeField]
    private RectTransform content;

    /// <summary>
    /// オブジェクトの削除
    /// </summary>
    public void DeleteObject()
    {
        foreach (Transform n in content.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        content.transform.DetachChildren();
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="eventItem"></param>
    public void AddItem(EventItem eventItem)
    {
        // Itemを生成
        Debug.Log("EventItemList:" + EventItemController.EventItemList.Count);

        var item = GameObject.Instantiate(prefab) as RectTransform;

        var button = item.transform.Find("Button");

        button.transform.Find("DateTimeText").gameObject.GetComponent<Text>().text = eventItem.CreateTime;
        if(eventItem.Income)
        {
            button.transform.Find("IncomeText").gameObject.GetComponent<Text>().color = Color.blue;
            button.transform.Find("IncomeText").gameObject.GetComponent<Text>().text = "収入";
        }
        else
        {
            button.transform.Find("IncomeText").gameObject.GetComponent<Text>().color = Color.red;
            button.transform.Find("IncomeText").gameObject.GetComponent<Text>().text = "支出";
        }
        button.transform.Find("AmountTextValue").gameObject.GetComponent<Text>().text = "¥ "+eventItem.Amount.ToString("N0");
        button.transform.Find("GenreText").gameObject.GetComponent<Text>().text = GetGenre(eventItem.Genre);
        button.transform.Find("MemoText").gameObject.GetComponent<Text>().text = ChangeMemo(eventItem.Memo);
        
        button.GetComponent<Button>().onClick.AsObservable()
                .Subscribe(_ =>
                {
                    EventMenuHandler sameHandler = EventMenuHandler.ShowDialog(eventItem,"削除");
                    sameHandler._okButton.onClick.AddListener(() => {
                        // 削除メソッド起動
                        DeleteMenu(eventItem, item);
                    });
                });
        
        // Contentの子として登録  
        item.SetParent(content, false);
        int ObjCount = content.transform.childCount;
        content.sizeDelta = new Vector2(0, 90 * (ObjCount + 1));
    }

    /// <summary>
    /// 削除メニュー
    /// </summary>
    /// <param name="itemId"></param>
    private void DeleteMenu(EventItem selectItem, RectTransform deleteObject)
    {
        DialogHandler isDelete = DialogHandler.ShowDialog("警告！", "本当に削除しますか？ 削除すると２度と戻せません！","Yes", "No");
        isDelete._okButton.onClick.AddListener(() => {
            bool isDeleted =DeleteItem(selectItem);
            if(isDeleted)
            {
                DialogHandler.ShowDialog("削除完了", "削除が完了しました！");
                GameObject.Destroy(deleteObject.transform.gameObject);
            }
            else
            {
                DialogHandler.ShowDialog("削除失敗", "削除が失敗しました…");
            }
        });
    }

    

    /// <summary>
    /// ジャンル取得メソッド
    /// </summary>
    /// <param name="genreCode"></param>
    /// <returns></returns>
    public string GetGenre(int genreCode)
    {
        foreach(var genreItem in GenreController.genreItemList)
        {
            if(genreCode.Equals(genreItem.ID))
            {
                return genreItem.GenreName;
            }
        }

        return "";
    }

    /// <summary>
    /// 行数がはみ出ないように修正するメソッド
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string ChangeMemo(string value)
    {
        int count = value.Count(c => c == '\n') + 1;

        if(value.Length >= 30)
        {
            return value.Substring(0, 30) + "・・・";
        }
        else if(count > 1)
        {
            int end = value.IndexOf("\n");
            return value.Substring(0,end) + "・・・";
        }

        return value;
    }

}
