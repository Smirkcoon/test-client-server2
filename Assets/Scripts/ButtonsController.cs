using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private WebManager webManager;
    [SerializeField] private PopupController popupController;
    [SerializeField] private ScrollViewController scrollViewController;
    
    public void OnClickCreate()
    {
        popupController.HidePopup();
        webManager.StartPost();
    }

    public void OnClickDelete()
    {
        if (!string.IsNullOrEmpty(popupController.GetTextInputField()))
            webManager.StartDelete();
        else
        {
            if(popupController.Opened)
                popupController.FlashWarningText();
        }
        popupController.ShowPopup();
    }

    public void OnClickUpdate()
    {
        if (!string.IsNullOrEmpty(popupController.GetTextInputField()))
            webManager.StartPut();
        else
        {
            if(popupController.Opened)
                popupController.FlashWarningText();
        }
        popupController.ShowPopup();
    }

    public void OnClickRefresh()
    {
        if (popupController.Opened)
            webManager.StartGet();
        popupController.ShowPopup();
    }
}
