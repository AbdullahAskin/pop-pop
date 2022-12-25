using System;
using System.Collections.Generic;
using System.Numerics;
using Lean.Touch;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class AimManager : MonoBehaviour
{
    [SerializeField] private Sprite indicatorSprite;
    
    [SerializeField] private float minIndicatorScale;
    [SerializeField] private float maxIndicatorScale;

    [SerializeField] private float indicatorTransparencyChangeRate;

    [SerializeField] private int indicatorCount;
    [SerializeField] private float indicatorTimeInterval;

    private readonly List<SpriteRenderer> _indicatorRenderers = new List<SpriteRenderer>();
    
    private AimManager()
    {
        Instance = this;
    }

    private void Start()
    {        
        CreateIndicators();
    }
    
    void CreateIndicators()
    {
        // Create a GameObject for each indicator
        for (var index = indicatorCount; index > 0; index--)
        {
            // Calculate the scale for the current indicator
            var scale = minIndicatorScale + (maxIndicatorScale - minIndicatorScale) * index / indicatorCount;

            // Create the GameObject and set its scale
            var go = new GameObject("indicator" + index + " - "+ scale);
            go.transform.localScale = Vector2.one * scale;

            // Add a SpriteRenderer and set its sprite and sorting order
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = indicatorSprite;
            spriteRenderer.sortingOrder = 1;
            spriteRenderer.enabled = false;

            // Add the SpriteRenderer to the list of indicator renderers
            _indicatorRenderers.Add(spriteRenderer);
        }
    }

    public void ToggleIndicators(bool bind)
    {
        foreach (var indicatorRenderer in _indicatorRenderers)
        {
            indicatorRenderer.enabled = bind;
        }
        
        SetIndicatorsOpacity(0);
    }

public void StepAimGuide(Vector2 force, float normalizedInputRate)
{   
    SetIndicatorsOpacity(normalizedInputRate);

    var currentPlayerPos = (Vector2)GameManager.Instance.currentPlayer.transform.position;

    for (var index = 0; index < _indicatorRenderers.Count; index++)
    {
        Renderer indicator = _indicatorRenderers[index];
        var futurePosition = CalculateFuturePosition(index, currentPlayerPos, force);
        ReflectOffScreen(indicator, futurePosition);
    }
}

void SetIndicatorsOpacity(float inputRate)
{
    var opacityRatio = Mathf.Clamp(inputRate / indicatorTransparencyChangeRate, 0, 1);

    foreach (var indicatorRenderer in _indicatorRenderers)
    {
        var spriteColor = indicatorRenderer.color;
        spriteColor.a = opacityRatio;
        indicatorRenderer.color = spriteColor;
    }
}

Vector2 CalculateFuturePosition(int index, Vector2 currentPlayerPos, Vector2 force)
{
    float time = (index + 1.5f) * indicatorTimeInterval;
    Vector2 spaceVelocity = force * Time.fixedDeltaTime;
    Vector2 finalVelocity = spaceVelocity + Physics2D.gravity * time;
    return finalVelocity * time + currentPlayerPos;
}

void ReflectOffScreen(Component indicator, Vector2 futurePosition)
{
    Vector2 screenPos = Camera.main.WorldToScreenPoint(futurePosition);
    if (screenPos.x < 0)
    {
        indicator.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(-screenPos.x, screenPos.y));
    }
    else if (screenPos.x > Screen.width)
    {
        var reflectedScreenPosX = Screen.width - (screenPos.x - Screen.width);
        indicator.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(reflectedScreenPosX, screenPos.y));
    }
    else
    {
        indicator.transform.position = futurePosition;
    }
}

public static AimManager Instance { get; private set; }

}