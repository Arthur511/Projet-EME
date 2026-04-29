using UnityEngine;
using DG.Tweening;
public class ToolsMethods : MonoBehaviour
{
    
    public void MoveUpUISmooth(GameObject UIElement)
    {
        UIElement.transform.DOMoveY(250, 1f);
    }
    public void MoveDownUISmooth(GameObject UIElement)
    {
        UIElement.transform.DOMoveY(-250, 1f);
    }
}
