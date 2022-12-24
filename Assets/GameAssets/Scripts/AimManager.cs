using System;
using System.Collections.Generic;
using System.Numerics;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class AimManager : MonoBehaviour
{
    [NonSerialized] public bool Active;
    
    [SerializeField] private Sprite indicatorSprite;

    [SerializeField] private float minIndicatorScale;
    [SerializeField] private float maxIndicatorScale;

    [SerializeField] private float minIndicatorOpacityRate;
    [SerializeField] private float maxIndicatorOpacityRate;

    [SerializeField] private int indicatorCount;
    [SerializeField] private float indicatorTimeInterval;

    private float _minForce;
    private float _maxForce;
    
    private readonly List<SpriteRenderer> _indicatorRenderers = new List<SpriteRenderer>();

    private AimManager()
    {
        Instance = this;
    }

    private void Start()
    {
        _minForce = GameManager.Instance.minForce;
        _maxForce = GameManager.Instance.maxForce;
        
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
            var go = new GameObject("indicator" + index + " - " + scale);
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
    }

    public void StepAimGuide(Vector2 fingerDir, float fingerForceRatio)
    {
        Active = fingerForceRatio >= minIndicatorOpacityRate;  
            
        // Set indicators opacity
        if (fingerForceRatio < maxIndicatorOpacityRate)
        {
            var opacityRatio =
                Mathf.Clamp(fingerForceRatio - minIndicatorOpacityRate, .0f,
                    maxIndicatorOpacityRate - minIndicatorOpacityRate) /
                (maxIndicatorOpacityRate - minIndicatorOpacityRate);

            SetIndicatorsOpacity(opacityRatio);
        }
        else
        {
            SetIndicatorsOpacity(1);
        }

        // Predict indicator positions
        var currentPlayerPos = (Vector2)GameManager.Instance.CurrentPlayer.transform.position;

        var forceMagnitude = _minForce + (_maxForce - _minForce) * fingerForceRatio;
        var force = -fingerDir * forceMagnitude;

        for (var index = 0; index < _indicatorRenderers.Count; index++)
        {
            var indicator = _indicatorRenderers[index];

            var time = (index + 1.5f) * indicatorTimeInterval;

            var spaceVelocity = force * Time.fixedDeltaTime;
            var finalVelocity = spaceVelocity + Physics2D.gravity * time;
            var futurePosition = finalVelocity * time + currentPlayerPos;

            // Out of screen indicator reflection 
            var screenPos = Camera.main.WorldToScreenPoint(futurePosition);
            if (screenPos.x < 0)
            {
                indicator.transform.position =
                    (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(-screenPos.x, screenPos.y));
            }
            else if (screenPos.x > Screen.width)
            {
                var reflectedScreenPosX = Screen.width - (screenPos.x - Screen.width);
                indicator.transform.position =
                    (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(reflectedScreenPosX, screenPos.y));
            }
            else
            {
                indicator.transform.position = futurePosition;
            }
        }
    }

    void SetIndicatorsOpacity(float opacityRatio)
    {
        foreach (var indicatorRenderer in _indicatorRenderers)
        {
            var spriteColor = indicatorRenderer.color;
            spriteColor.a = opacityRatio;
            indicatorRenderer.color = spriteColor;
        }
    }

    public static AimManager Instance { get; private set; }
}