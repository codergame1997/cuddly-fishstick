using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

[RequireComponent(typeof(Button))]
public class CardView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;

    // events for presenter
    public Subject<Unit> OnClicked { get; } = new Subject<Unit>();

    void OnEnable()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClicked.OnNext(Unit.Default));
    }

    public void SetFrontSprite(Sprite s) => frontImage.sprite = s;

    // Card flip animation
    public Tween Flip(bool faceUp, float duration = 0.35f)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOScaleX(0f, duration * 0.5f).SetEase(Ease.InOutQuad));
        seq.AppendCallback(() =>
        {
            frontImage.gameObject.SetActive(faceUp);
            backImage.gameObject.SetActive(!faceUp);
        });
        seq.Append(rectTransform.DOScaleX(1f, duration * 0.5f).SetEase(Ease.InOutQuad));
        return seq;
    }

    // Card match animation
    public Tween PlayMatchedAnimation()
    {
        return rectTransform
            .DOScale(0f, 0.4f)
            .SetEase(Ease.InBack)
            .OnComplete(() => button.interactable = false);
    }

    public void SetInteractable(bool v) => button.interactable = v;
}