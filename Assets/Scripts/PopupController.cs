using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private float appearScale = 1.2f;
   
    private Color originalPlaceholderColor;
    private Sequence sequenceWarning;
    [HideInInspector]
    public bool Opened;
    private void Start()
    {
        Opened = false;
        popupPanel.SetActive(false);
        originalPlaceholderColor = idInputField.placeholder.color;
    }

    public void ShowPopup()
    {
        if (!Opened)
        {
            popupPanel.SetActive(true);
            Sequence sequence = DOTween.Sequence();
        
            sequence.Append( popupPanel.transform.DOScale(appearScale, animationDuration));
            sequence.Append( popupPanel.transform.DOScale(1, animationDuration));
           
            idInputField.text = "";

            idInputField.Select();
            idInputField.ActivateInputField();
            Opened = true;
        }
    }

    public void HidePopup()
    {
        if (Opened)
        {
            sequenceWarning.Kill();
            RestorePlaceholderColor();
            popupPanel.transform.DOScale(0, animationDuration).OnComplete(() =>
            {
                popupPanel.SetActive(false);
                Opened = false;
            });
        }
    }
    
    public string GetTextInputField()
    {
        return idInputField.text;
    }
    public void FlashWarningText()
    {
        sequenceWarning.Kill();
        idInputField.placeholder.color = Color.red;
        
        sequenceWarning = DOTween.Sequence();
        
        sequenceWarning.Append(idInputField.placeholder.DOFade(0.3f, 0.15f));
        sequenceWarning.AppendInterval(0.1f);
        sequenceWarning.Append(idInputField.placeholder.DOFade(1, 0.15f));
        sequenceWarning.AppendInterval(0.1f);
        sequenceWarning.SetLoops(2);
        sequenceWarning.OnComplete(() => RestorePlaceholderColor());
    }
    void RestorePlaceholderColor()
    {
        idInputField.placeholder.color = originalPlaceholderColor;
    }
}