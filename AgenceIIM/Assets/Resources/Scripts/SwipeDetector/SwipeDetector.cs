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
            SwipeDirection direction;
            if((fingerDownPosition.x <= (Screen.width / 2)) && (fingerDownPosition.y <= (Screen.height / 2)) && (fingerUpPosition.x > (Screen.width / 2)) && (fingerUpPosition.y > (Screen.height / 2)))
            {
                direction = SwipeDirection.Left;
            }
            else if((fingerDownPosition.x > (Screen.width / 2)) && (fingerDownPosition.y > (Screen.height / 2)) && (fingerUpPosition.x <= (Screen.width / 2)) && (fingerUpPosition.y <= (Screen.height / 2)))
            {
                direction = SwipeDirection.Right;
            }
            else if((fingerDownPosition.x > (Screen.width / 2)) && (fingerDownPosition.y <= (Screen.height / 2)) && (fingerUpPosition.x <= (Screen.width / 2)) && (fingerUpPosition.y > (Screen.height / 2)))
            {
                direction = SwipeDirection.Down;
            }
            else if((fingerDownPosition.x <= (Screen.width / 2)) && (fingerDownPosition.y > (Screen.height / 2)) && (fingerUpPosition.x > (Screen.width / 2)) && (fingerUpPosition.y <= (Screen.height / 2)))
            {
                direction = SwipeDirection.Up;
            }
            else
            {
                return;
            }
            SendSwipe(direction);     
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
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
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