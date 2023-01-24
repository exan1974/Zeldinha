using System;
using UnityEngine;
using EventArgs;

public class Interaction : MonoBehaviour
{
    public GameObject widgetPrefab; 
    [SerializeField] private Vector3 widgetOffset;
    public float radius = 5f;

    private GameObject widget;
    private bool isAvailable = true;
    private bool isActive;

    public event EventHandler<InteractionEventArgs> OnInteraction;

    private void OnEnable() 
    {
        GameManager.Instance.interactionList.Add(this);
    }

    private void OnDisable() 
    {
        GameManager.Instance.interactionList.Remove(this);

    }

    void Awake() 
    {
        // Create widget
        widget = Instantiate(widgetPrefab, transform.position + widgetOffset, 
        widgetPrefab.transform.rotation);
        widget.transform.SetParent(gameObject.transform, true);
    }

    void Start()
    {
        // Set widget camera
        var worldUICamera = GameManager.Instance.worldUICamera;
        var canvas = widget.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = worldUICamera;
        }

        var interactionWidgetComponent = widget.GetComponent<InteractionWidget>();
        if (interactionWidgetComponent != null)
        {
            interactionWidgetComponent.worldUICamera = worldUICamera;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool isActive)
    {
        var wasActive = this.isActive;
        this.isActive = isActive;

        // Update InteractionWidget
        var interactionWidget = widget.GetComponent<InteractionWidget>();
        if (isActive) {
            interactionWidget.Show();
        } else {
            interactionWidget.Hide();
        }

    }

    public bool IsAvailable()
    {
        return isAvailable;
    }

    public void SetAvailable (bool isAvailable)
    {
        this.isAvailable = isAvailable;
    }

    public void Interact()
    {
        // Invoke event
        OnInteraction?.Invoke(this, new InteractionEventArgs());
        
    }

    public void SetActionText(string text)
    {
        // Update InteractionWidget
        var interactionWidget = widget.GetComponent<InteractionWidget>();
        interactionWidget.SetActionText(text);
    }
}
