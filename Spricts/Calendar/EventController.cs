using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class EventController : MonoBehaviour
{
    [SerializeField] 
    private SpawnItem spawnItem;

    // Start is called before the first frame update
    void Start()
    {
        if (!EventItemController.isLoad)
        {
            EventItemController.LoadAllEventItemData();
        }
        if (!GenreController.isLoad)
        {
            GenreController.LoadAllGenreItemData();
        }
        // Itemを生成
        Debug.Log("EventItemList:" + EventItemController.EventItemList.Count);

        foreach (var eventItem in EventItemController.EventItemList)
        {
            DateTime eventday = DateTime.Parse(eventItem.CreateTime);
            eventday = DateTime.Parse(eventday.Year.ToString() + " " + eventday.Month.ToString() +" "+ eventday.Day.ToString());
            DateTime nowTime = DateTime.Parse(DateTime.Now.Year.ToString() + " " + DateTime.Now.Month.ToString() + " " + DateTime.Now.Day.ToString());
            if (eventday.Equals(nowTime))
            {
                spawnItem.AddItem(eventItem);
            }
            
        }
    }

}
