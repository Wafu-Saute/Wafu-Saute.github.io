using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///  ダイアログの表示用スプリクト
/// </summary>
public class DialogHandler : MonoBehaviour
{
    [Header("タイトル")]
    [SerializeField] 
    private Text _title;
    [Header("文言")]
    [SerializeField] 
    private Text _description;

    [Header("okボタン")]
    [SerializeField] 
    public Button _okButton;
    [Header("ngボタン")]
    [SerializeField] 
    public Button _ngButton;
    public UnityAction onDestroyed;

    [Header("背景")]
    [SerializeField] 
    private Image _background;

    // プレハブ名
    private static readonly string PREFAB_NAME = "DialogCanvasPrefab";
    // 生成後のGameObject
    private static GameObject prefab;


    public static DialogHandler ShowDialog(
        string title, string description, string ok = null, string ng = null
    )
    {
        if (prefab == null)
        {
            prefab = Resources.Load(PREFAB_NAME) as GameObject;
        }

        var instance = Instantiate(prefab);
        var handler = instance.GetComponent<DialogHandler>();

        handler._title.text = title;
        handler._description.text = description;

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
                handler._okButton.transform.position = new Vector3(0, -347, 0);
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
}