using System;
using System.Collections.Generic;
using System.Numerics;
using Lean.Touch;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerAimManager : MonoBehaviour
{
    [SerializeField] private Sprite indicatorSprite;
    
    [SerializeField] private float minIndicatorScale;
    [SerializeField] private float maxIndicatorScale;
    
    [SerializeField] private float minIndicatorOpacityRate;
    [SerializeField] private float maxIndicatorOpacityRate;

    [SerializeField] private int indicatorCount;
    [SerializeField] private float indicatorTimeInterval;
    
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float screenRateMaxForce;
    
    private List<GameObject> _indicators = new List<GameObject>();
    private Player _player;
    
    private PlayerAimManager()
    {
        Instance = this;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        
        //

        for (var index = indicatorCount; index > 0; index--)
        {
            var scale  = minIndicatorScale + (maxIndicatorScale - minIndicatorScale) * index / indicatorCount;
            var go = new GameObject("indicator" + index + " - "+ scale)
            {
                transform =
                {
                    localScale = Vector2.one * scale
                }
            };

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = indicatorSprite;
            renderer.sortingOrder = 1;
            
            _indicators.Add(go);
        }
    }
    
    public void StepAimGuide(LeanFinger finger)
    {
        var fingerDeltaPos = finger.ScreenPosition - finger.StartScreenPosition;
        var fingerForceRatio = Mathf.Clamp(fingerDeltaPos.magnitude / (Screen.width * screenRateMaxForce), 0, 1);
        
        if (fingerForceRatio < maxIndicatorOpacityRate)
        {
            var opacityRatio = Mathf.Clamp(fingerForceRatio - minIndicatorOpacityRate, .0f, maxIndicatorOpacityRate - minIndicatorOpacityRate) / (maxIndicatorOpacityRate - minIndicatorOpacityRate);
            
            foreach (var indicator in _indicators)
            {
                var spriteRenderer = indicator.GetComponent<SpriteRenderer>();
                var spriteColor = spriteRenderer.color;
                spriteColor.a = opacityRatio;
                spriteRenderer.color = spriteColor;
            }
        }
        
        //
        
        var currentPlayerPos = (Vector2)_player.transform.position;
        
        var forceMagnitude = minForce + (maxForce - minForce) * fingerForceRatio;
        var force = -fingerDeltaPos.normalized * forceMagnitude;

        for (var index = 0; index < _indicators.Count; index++)
        {
            var indicator = _indicators[index];
            
            var time = index * indicatorTimeInterval;

            var spaceVelocity = force * Time.fixedDeltaTime;
            var finalVelocity = spaceVelocity + Physics2D.gravity * time;
            var futurePosition = finalVelocity * time + currentPlayerPos;

            // Out of screen indicator reflection 
            var screenPos = Camera.main.WorldToScreenPoint(futurePosition);
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
            
            print(screenPos.x);
        }
    }
    
    public static PlayerAimManager Instance { get; private set; }

}