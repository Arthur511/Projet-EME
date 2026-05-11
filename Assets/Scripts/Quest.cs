using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Quests contains stories. Quests are used for initializing stories and for triggering them when the player clicks on them. They are also used for showing the quest marker on the map.
/// </summary>
public class Quest : MonoBehaviour
{
    public Story Story => _story;

    [SerializeField] Story _story;
    [SerializeField] List<GameObject> _lockStories;

    bool _done = false;

    public void RevealNextQuests()
    {
        MainGame.Instance.ToolsMethods.DispearScaleSmooth(gameObject);
        if (_lockStories.Count > 0)
        {
            foreach (GameObject story in _lockStories)
            {
                MainGame.Instance.ToolsMethods.SpawnScaleSmooth(story);
            }
        }
    }


}
