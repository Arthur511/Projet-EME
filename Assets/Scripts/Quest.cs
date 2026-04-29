using UnityEngine;

/// <summary>
/// Quests contains stories. Quests are used for initializing stories and for triggering them when the player clicks on them. They are also used for showing the quest marker on the map.
/// </summary>
public class Quest : MonoBehaviour
{
    public Story Story => _story;

    [SerializeField] Story _story;
}
