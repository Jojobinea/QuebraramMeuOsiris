using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverEffectTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Target References")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;

    [Header("Hover Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.cyan;
    [SerializeField] private Color normalTextColor = Color.black;
    [SerializeField] private Color hoverTextColor = Color.white;
    [SerializeField, Range(1f, 1.5f)] private float scaleMultiplier = 1.1f;
    [SerializeField, Range(1f, 20f)] private float transitionSpeed = 10f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private bool isHovered = false;

    void Start()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (buttonText == null)
            buttonText = GetComponentInChildren<TMP_Text>();

        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    void Update()
    {
        // Smooth scaling
        Vector3 targetScale = isHovered ? originalScale * scaleMultiplier : originalScale;
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        // Smooth color transitions
        Color targetButtonColor = isHovered ? hoverColor : normalColor;
        Color targetTextColor = isHovered ? hoverTextColor : normalTextColor;

        button.image.color = Color.Lerp(button.image.color, targetButtonColor, Time.deltaTime * transitionSpeed);
        buttonText.color = Color.Lerp(buttonText.color, targetTextColor, Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
