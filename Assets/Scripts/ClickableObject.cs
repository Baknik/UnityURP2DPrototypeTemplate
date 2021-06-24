using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Settings")]
    public float ScaleFactor = 0.8f;
    public Color ClickColor = Color.green;
    public SFXPlayer ClickSFX;

    private Vector2 originalScale;
    private Color originalColor;

    private Collider2D c2D;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        this.c2D = this.GetComponent<Collider2D>();
        this.mainCamera = Camera.main;
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.originalScale = this.transform.localScale;
        this.originalColor = this.spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Sequence clickSequence = DOTween.Sequence();
        clickSequence.Append(this.spriteRenderer.DOColor(ClickColor, 0.1f));
        clickSequence.Append(this.spriteRenderer.DOColor(this.originalColor, 0.1f));
        clickSequence.Play();

        ClickSFX.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.DOScale(this.originalScale * ScaleFactor, 0.1f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.DOScale(this.originalScale, 0.05f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.DOScale(this.originalScale, 0.05f);
        }
    }
}
