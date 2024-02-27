using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListViewItem : MonoBehaviour
{
    [SerializeField] private TMP_Text textId;
    [SerializeField] private Toggle animationType;
    [SerializeField] private TMP_Text textName;
    [SerializeField] private Image image;
    public Button btn;
    [HideInInspector]
    public ListViewModel modelData;

    private void Start()
    {
        animationType.onValueChanged.AddListener((isOn)=> modelData.animationType = isOn);
    }

    public void Setup(ListViewModel modelData)
    {
        this.modelData = modelData;
        gameObject.name = this.modelData.id.ToString();
        textId.text = "Id: " + this.modelData.id;
        
        animationType.isOn = this.modelData.animationType;
        textName.text = "Name: " + this.modelData.text;
        float[] color = this.modelData.color;

        float h = color[0] > 1 ? color[0]/360 : color[0];//fix random in mock
        
        image.color = Color.HSVToRGB(h,color[1], color[2]);
    }
}
