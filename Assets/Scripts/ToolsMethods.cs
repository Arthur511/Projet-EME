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
    public void SpawnScaleSmooth(GameObject UIElement)
    {
        UIElement.SetActive(true);
        UIElement.transform.DOScale(1, 0.8f).SetEase(Ease.OutBounce);
    }
    public void DispearScaleSmooth(GameObject UIElement)
    {
        UIElement.transform.DOScale(0, 0.8f).OnComplete(() => UIElement.SetActive(false)).SetEase(Ease.OutExpo);
    }
    public void BounceScale(GameObject UIElement)
    {
        UIElement.transform.DOScale(1.2f, 0.2f).SetEase(Ease.InExpo).OnComplete(() => UIElement.transform.DOScale(1f, 0.4f).SetEase(Ease.OutExpo));
    }
}
