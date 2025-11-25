using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

[RequireComponent(typeof(Button))]
public class CardView : MonoBehaviour
{
    [Header("References")]
    public Image frontImage;
    public Image backImage;
    public RectTransform rectTransform;
    public Button button;

    // events for presenter
    public Subject<Unit> OnClicked { get; } = new Subject<Unit>();

    void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClicked.OnNext(Unit.Default));
    }

    public void SetFrontSprite(Sprite s) => frontImage.sprite = s;

    // TODO Test animation when UI and temp game logic is completed
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

    // TODO Test animation when UI and temp game logic is completed
    public Tween PlayMatchedAnimation()
    {
        return rectTransform
            .DOScale(0f, 0.4f)
            .SetEase(Ease.InBack)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void SetInteractable(bool v) => button.interactable = v;
}