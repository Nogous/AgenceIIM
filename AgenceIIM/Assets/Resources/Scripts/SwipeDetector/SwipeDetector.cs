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
            //true is horrizontal, false is vertical
            bool direction = HorizontalMovementDistance() > VerticalMovementDistance();
            if(direction)
            {
                //true is Right, false is Left
                bool signHor = SignofHorizontalDistance();
                if(signHor)
                {
                    directionData = SwipeDirection.Right;
                }
                else
                {
                    directionData = SwipeDirection.Left;
                }
            }
            else
            {
                //true is Up, false is Down
                bool signVer = SignofVerticalDistance();
                if(signVer)
                {
                    directionData = SwipeDirection.Up;
                }
                else
                {
                    directionData = SwipeDirection.Down;
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
    //true is Right, false is Left
    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerUpPosition.x - fingerDownPosition.x);
    }
    //true is Up, false is Down
    private bool SignofHorizontalDistance()
    {
        return fingerUpPosition.x > fingerDownPosition.x;
    }
    private bool SignofVerticalDistance()
    {
        return fingerUpPosition.y > fingerDownPosition.y;
    }
    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition,
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}