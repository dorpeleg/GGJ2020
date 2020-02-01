using System;
using UnityEngine;

public enum SwipeDirection
{
	NONE,
	UP,
	DOWN,
	LEFT,
	RIGHT
}

public class GestureManager : MonoBehaviour
{
	public static GestureManager Instance
	{
		get
		{
			if (_instance == null)
			{
				var go = new GameObject();
				_instance = go.AddComponent<GestureManager>();
			}
			return _instance;
		}
	}
	private static GestureManager _instance;
	
	private Vector2 _startPos;
	private Vector2 _direction;

	public event SwipeEventHandler SwipeEvent;
	public delegate void SwipeEventHandler(object source, GestureEventArgs e);
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

    // Update is called once per frame
    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

			// Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
					// Record initial touch position.
					Debug.Log("Test");
                    _startPos = touch.position;
                    break;
                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    _direction = touch.position - _startPos;
                    break;
                case TouchPhase.Ended:
					// Report that the touch has ended when it ends
					var swipeDir = GetSwipeDirection();
					SwipeEvent.Invoke(this, new GestureEventArgs(swipeDir));
                    break;
            }
		}
    }
	
	private SwipeDirection GetSwipeDirection()
	{
		if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y))
		{
			if (_direction.x > 0)
			{
				return SwipeDirection.RIGHT;
			}
			else
			{
				return SwipeDirection.LEFT;
			}
		}
		else
		{
			if (_direction.y > 0)
			{
				return SwipeDirection.UP;
			}
			else
			{
				return SwipeDirection.DOWN;
			}
		}
	}
}

public class GestureEventArgs : EventArgs
{
	public SwipeDirection Direction { get; private set; }

	public GestureEventArgs(SwipeDirection direction)
	{
		Direction = direction;
	}
}
