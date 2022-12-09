using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerAimManager : MonoBehaviour
{
    [SerializeField] private Sprite indicatorSprite;
    [SerializeField] private Vector2 indicatorScaleLimits;
    [SerializeField] private float indicatorGap;
    [SerializeField] private float indicatorGapScaleRate;
    [SerializeField] private int indicatorCount;

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
        var currentIndicatorGap = indicatorGap;
        
        foreach (var indicator in _indicators)
        {
            indicator.transform.position = currentPos;
            currentPos += currentIndicatorGap * guideDir;
            currentIndicatorGap *= indicatorGapScaleRate;
        }
    }
    
    public static PlayerAimManager Instance { get; private set; }

}
