using UnityEngine;
using UnityEngine.EventSystems;

public class ActivateObjectOnHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private GameObject objectToShow;

    private void OnEnable()
    {
        objectToShow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        objectToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objectToShow.SetActive(false);
    }
}
