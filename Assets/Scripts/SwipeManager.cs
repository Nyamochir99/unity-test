using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeManager : MonoBehaviour
{
    public static bool tap,
        swipeLeft,
        swipeRight,
        swipeUp,
        swipeDown;

    public float minSwipeDistance = 100f;

    private bool isDragging = false;
    private Vector2 startTouch;

    private void Update()
    {
        tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;

        bool pressedThisFrame = false;
        bool releasedThisFrame = false;
        bool isPressed = false;
        Vector2 position = Vector2.zero;

        // Mobile touch
        Touchscreen touchscreen = Touchscreen.current;
        if (
            touchscreen != null
            && (
                touchscreen.primaryTouch.press.isPressed
                || touchscreen.primaryTouch.press.wasReleasedThisFrame
            )
        )
        {
            pressedThisFrame = touchscreen.primaryTouch.press.wasPressedThisFrame;
            releasedThisFrame = touchscreen.primaryTouch.press.wasReleasedThisFrame;
            isPressed = touchscreen.primaryTouch.press.isPressed;
            position = touchscreen.primaryTouch.position.ReadValue();
        }
        // Mouse (editor / PC)
        else if (Mouse.current != null)
        {
            pressedThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
            releasedThisFrame = Mouse.current.leftButton.wasReleasedThisFrame;
            isPressed = Mouse.current.leftButton.isPressed;
            position = Mouse.current.position.ReadValue();
        }

        if (pressedThisFrame)
        {
            tap = true;
            isDragging = true;
            startTouch = position;
        }

        if (isDragging && (isPressed || releasedThisFrame))
        {
            Vector2 swipeDelta = position - startTouch;

            if (swipeDelta.magnitude > minSwipeDistance)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    if (swipeDelta.x < 0)
                        swipeLeft = true;
                    else
                        swipeRight = true;
                }
                else
                {
                    if (swipeDelta.y < 0)
                        swipeDown = true;
                    else
                        swipeUp = true;
                }

                // One swipe per drag
                isDragging = false;
            }
        }

        if (releasedThisFrame)
        {
            isDragging = false;
        }
    }
}
