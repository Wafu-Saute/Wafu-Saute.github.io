using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EventItemController;

/// <summary>
///  イベントの詳細を表示するスプリクト
/// </summary>
public class EventMenuHandler : MonoBehaviour
{
    // テキストや画像の紐づけ
    [Header("日付・時間")]
    [SerializeField] 
    private Text _dateAndTime;
    [Header("収入？支出？")]
    [SerializeField] 
    private Text _income;
    [Header("金額")]
    [SerializeField] 
    private Text _amount;
    [Header("アカウント")]
    [SerializeField] 
    private Text _account;
    [Header("項目")]
    [SerializeField] 
    private Text _genre;
    [Header("メモ")]
    [SerializeField] 
    private Text _memo;
    [Header("背景")]
    [SerializeField]
    private Image _background;
    [Header("okボタン")]
    [SerializeField]
    public Button _okButton;
    [Header("ngボタン")]
    [SerializeField]
    public Button _ngButton;
    public UnityAction onDestroyed;
    //-----

    // プレハブの名前
    private static readonly string PREFAB_NAME = "EventMenuPrefab";
    // 生成後のGameObject
    private static GameObject prefab;


    public static EventMenuHandler ShowDialog(
        EventItem eventItem, string ok = null, string ng = null
    )
    {
        var a = GameObject.Find("EventMenuPrefab(Clone)");
        Destroy(a);

        if (prefab == null)
        {
            prefab = Resources.Load(PREFAB_NAME) as GameObject;
        }

        var instance = Instantiate(prefab);
        var handler = instance.GetComponent<EventMenuHandler>();

        handler._dateAndTime.text = eventItem.CreateTime;
        handler._income.text = eventItem.Income.ToString();
        if (eventItem.Income)
        {
            handler._income.color = Color.blue;
            handler._income.text = "収入";
        }
        else
        {
            handler._income.color = Color.red;
            handler._income.text = "支出";
        }
        handler._amount.text = eventItem.Amount.ToString();
        handler._account.text = eventItem.Medium;
        handler._genre.text = GetGenre(eventItem.Genre);
        handler._memo.text = eventItem.Memo;


        if (string.IsNullOrEmpty(ok))
        {
            Destroy(handler._okButton.gameObject);
            handler._okButton = null;
        }
        else
        {
            handler._okButton.GetComponentInChildren<Text>().text = ok;
            handler._okButton.onClick.AddListener(() => Destroy(handler.gameObject));
            if (string.IsNullOrEmpty(ng))
            {
                handler._okButton.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, -55f,0f);
            }
        }

        if (string.IsNullOrEmpty(ng))
        {
            Destroy(handler._ngButton.gameObject);
            handler._ngButton = null;
        }
        else
        {
            handler._ngButton.GetComponentInChildren<Text>().text = ng;
            handler._ngButton.onClick.AddListener(() => Destroy(handler.gameObject));
        }

        return handler;
    }

    void Start()
    {
        var eventTrigger = _background.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(eventData => { Destroy(this.gameObject); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnDestroy()
    {
        onDestroyed?.Invoke();
    }

    public static string GetGenre(int genreCode)
    {
        foreach (var genreItem in GenreController.genreItemList)
        {
            if (genreCode.Equals(genreItem.ID))
            {
                return genreItem.GenreName;
            }
        }

        return "";
    }
}