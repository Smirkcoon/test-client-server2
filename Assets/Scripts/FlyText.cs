using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlyText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private float _moveShowTime = .5f;
    private float _flyShowUpY = 15f;
    private float _scaleShowTime = 0.2f;
    private float _moveHideTime = .7f;
    private float _flyHideUpY = 30f;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = (RectTransform) transform;
        _text = GetComponent<TextMeshProUGUI>();
    }
    public void Fly(string text)
    {
        _text.text = text;
        Sequence sequence = DOTween.Sequence();

        // проявляем текст
        sequence.Append(_rectTransform
                .DOLocalMove(_rectTransform.localPosition + new Vector3(0, _flyShowUpY, 0), _moveShowTime)
                .SetEase(Ease.OutCubic))
            .Join(_rectTransform
                .DOScale(1.0f, _scaleShowTime)
                .SetEase(Ease.OutCubic));

        //плавно летим вверх
        sequence.Append(_rectTransform
            .DOLocalMove(_rectTransform.localPosition + new Vector3(0, _flyHideUpY, 0), _moveHideTime)
            .SetEase(Ease.InSine));

        //плавно фейдим
        sequence.Join(_text.DOFade(0, _moveHideTime)
            .SetEase(Ease.InSine));
        
        sequence.SetLink(gameObject);
        sequence.OnComplete(()=> Destroy(gameObject));
    }
}
