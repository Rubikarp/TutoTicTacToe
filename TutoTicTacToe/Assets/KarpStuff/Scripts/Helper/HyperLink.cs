using UnityEngine.EventSystems;
using UnityEngine;
using NaughtyAttributes;

public class HyperLink : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public string url = "https://youtu.be/dQw4w9WgXcQ";

    [Button]
    public void OpenLink()
    {
        Application.OpenURL(url);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenLink();
    }
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }
}