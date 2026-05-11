using UnityEngine;
using DG.Tweening;
public class ToolsMethods : MonoBehaviour
{

    [HideInInspector] public char[] Vowels = { 'a', 'e', 'i', 'o', 'u', 'y'};

    public void MoveUpUISmooth(GameObject UIElement)
    {
        UIElement.transform.DOMoveY(250, 1f);
    }
    public void MoveDownUISmooth(GameObject UIElement)
    {
        UIElement.transform.DOMoveY(-250, 1f);
    }
}
