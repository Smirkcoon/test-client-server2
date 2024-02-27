using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyObjSafeArea : MonoBehaviour
{
    private void Start()
    {
        var rectTransformComponent = GetComponent<RectTransform>();
        var safeAreaScreen = Screen.safeArea;
        var anchorMinPos = safeAreaScreen.position;
        var anchorMaxPos = anchorMinPos + safeAreaScreen.size;// + Vector2.up * 40;
        anchorMinPos.x /= Screen.width;
        anchorMinPos.y = 0;
        anchorMaxPos.x /= Screen.width;
        anchorMaxPos.y /= Screen.height;
        rectTransformComponent.anchorMin = anchorMinPos;
        rectTransformComponent.anchorMax = anchorMaxPos;
    }
}