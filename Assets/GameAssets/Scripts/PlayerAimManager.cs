using System;
using System.Collections.Generic;
using System.Numerics;
using Lean.Touch;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerAimManager : MonoBehaviour
{
    [SerializeField] private Sprite indicatorSprite;
    [SerializeField] private Vector2 indicatorScaleLimits;
    [SerializeField] private int indicatorCount;
    [SerializeField] private float maxForce;
    [SerializeField] private float indicatorTimeInterval;

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

        for (var index = 0; index < indicatorCount; index++)
        {
            var scale  = indicatorScaleLimits.x + (indicatorScaleLimits.y - indicatorScaleLimits.x) * index / indicatorCount;
            var go = new GameObject("indicator" + index)
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
        var guideDir = -(finger.ScreenPosition - finger.StartScreenPosition).normalized;

        var currentPos = (Vector2)_player.transform.position;

        for (var index = 0; index < _indicators.Count; index++)
        {
            var indicator = _indicators[index];
            
            var time = index * indicatorTimeInterval;

            // Calculate the initial velocity of the object
            var initialVelocity = guideDir * (maxForce * Time.fixedDeltaTime);

            // Calculate the final velocity of the object after time has elapsed
            var finalVelocity = initialVelocity + Physics2D.gravity * time;

            // Calculate the future position of the object
            var futurePosition = finalVelocity * time + currentPos;

            // Update the position of the indicator object
            indicator.transform.position = futurePosition;

        }
    }
    
    public static PlayerAimManager Instance { get; private set; }

}
