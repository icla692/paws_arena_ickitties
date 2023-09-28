using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineOnHover : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Outline[] outlines;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var _outline in outlines)
        {
            _outline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var _outline in outlines)
        {
            _outline.enabled = false;
        }
    }
}
