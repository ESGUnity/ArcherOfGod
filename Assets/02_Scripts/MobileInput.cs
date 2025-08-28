using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    // private 필드(인스펙터 노출)
    [SerializeField] private ButtonType buttonType;

    // 메인
    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Left)
        {
            PlayerController.Instance.MoveLeft();
        }
        else if (buttonType == ButtonType.Right)
        {
            PlayerController.Instance.MoveRight();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Left)
        {
            PlayerController.Instance.MoveLeft();
        }
        else if (buttonType == ButtonType.Right)
        {
            PlayerController.Instance.MoveRight();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerController.Instance.StopMove();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerController.Instance.StopMove();
    }
}
public enum ButtonType
{ 
    Left, Right 
}
