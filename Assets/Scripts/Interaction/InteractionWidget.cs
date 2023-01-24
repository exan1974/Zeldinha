using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private CanvasGroup canvasGroup;
    public Camera worldUICamera;

    private string inputString = "E";
    private string actionString;
    public bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        inputText.text = inputString;
        actionText.text = actionString;

        // Auto Hide
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        // Visibility
        var targetAlpha = isVisible ? 1 : 0;
        var currentAlpha = canvasGroup.alpha;
        var newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, 0.1f );
        canvasGroup.alpha = newAlpha;

        // Face camera
        transform.rotation = worldUICamera.transform.rotation;
    }

    public void SetActionText(string text)
    {
        actionString = text;
        actionText.text = actionString;
    }

    public void SetInputText(string text)
    {
        inputString = text;
        inputText.text = inputString;
    }

    public void Show()
    {
        isVisible = true;
    }

    public void Hide()
    {
        isVisible = false;
    }
}
