using UnityEngine;

public class Quest : MonoBehaviour
{
    public Story Story => _story;

    [SerializeField] Story _story;
}
