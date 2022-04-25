using UnityEngine;
using UnityEngine.UI;
using static GraphManager;

/// <summary>
/// グラフ詳細のアイテムを作成、表示するスプリクト
/// </summary>
public class GraphItemSpawn : MonoBehaviour
{
    [SerializeField]
    private RectTransform prefab;
    [SerializeField]
    private RectTransform content;

    public void DeleteObject()
    {
        if (content == null)
            return;

        foreach (Transform n in content.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        content.transform.DetachChildren();
    }

    public void AddItem(GraphItem graphItem)
    {
        // Itemを生成
        Debug.Log("EventItemList:" + EventItemController.EventItemList.Count);

        var item = GameObject.Instantiate(prefab) as RectTransform;

        item.transform.Find("NameText").gameObject.GetComponent<Text>().text = graphItem.Genre;
        item.transform.Find("AmountText").gameObject.GetComponent<Text>().text = graphItem.Amount.ToString("N0");
        item.transform.Find("PersentText").gameObject.GetComponent<Text>().text = graphItem.persent.ToString()+"%";
        item.transform.Find("ColorImage").gameObject.GetComponent<Image>().color = graphItem.color;

        item.SetParent(content, false);
        int ObjCount = content.transform.childCount;
        content.sizeDelta = new Vector2(0, 90 * (ObjCount));
    }

}
