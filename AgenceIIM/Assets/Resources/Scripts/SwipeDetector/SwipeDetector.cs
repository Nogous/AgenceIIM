using System;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            SwipeDirection directionData;
            directionData = SwipeDirection.None;
            if (fingerUpPosition.y - fingerDownPosition.y > 0) //DOWN
            {
                if (fingerUpPosition.x - fingerDownPosition.x > 0) //SWIPE DOWN LEFT
                {
                    directionData = SwipeDirection.Left;
                }
                else if (fingerUpPosition.x - fingerDownPosition.x < 0) //SWIPE DOWN RIGHT
                {
                    directionData = SwipeDirection.Down;
                }
            }
            else if (fingerUpPosition.y - fingerDownPosition.y < 0) //UP
            {
                if (fingerUpPosition.x - fingerDownPosition.x > 0) //SWIPE UP LEFT
                {
                    directionData = SwipeDirection.Up;
                }
                else if (fingerUpPosition.x - fingerDownPosition.x < 0) //SWIPE UP RIGHT
                {
                    directionData = SwipeDirection.Right;
                }
            }
        SendSwipe(directionData);     
        fingerUpPosition = fingerDownPosition;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }
    
    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerUpPosition.y - fingerDownPosition.y);
    }
    
    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerUpPosition.x - fingerDownPosition.x);
    }
    //true is Right, false is Left
    private bool SignofHorizontalDistance()
    {
        return fingerUpPosition.x < fingerDownPosition.x;
    }
    //true is Up, false is Down
    private bool SignofVerticalDistance()
    {
        return fingerUpPosition.y < fingerDownPosition.y;
    }
    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            fingerDownPosition = fingerDownPosition,
            fingerUpPosition = fingerUpPosition,
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 fingerDownPosition;
    public Vector2 fingerUpPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right,
    None
}