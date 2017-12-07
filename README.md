# LGK Screen Input
LGK Screen Input is a library for manage screen (touch) input for Unity Game Engine.
You can take alook [Unity-LGKScreenInput-Example](https://github.com/NED-Studio/Unity-LGKScreenInput-Example) for the example.

# Features
1. Pointer Event : Pointer Down, Pointer Move and Pointer Up
2. Pointer Action : Tap, Long Tap, Drag, Pinch Swipe
3. Standalone and Mobile (standlone using mouse pointer and mobile using touch)
4. Blocked by Unity UI

# Usage

1. Instansiate instance of `ScreenInputManager`
```c#
ScreenInputManager manager = new ScreenInputManager ();
 ```
 
2. Suscribe and unsuscribe input event in `MonoBehaviour.OnEnable()` and  `MonoBehaviour.OnDisable`
```c#
private void OnEnable ()
{
  manager.OnPointerDown += OnPointerDown_Handler;
  manager.OnPointerMoved += OnPointerMoved_Handler;
  manager.OnPointerUp += OnPointerUp_Handler;
  manager.OnTap += OnTap_Handler;
  manager.OnDoubleTap += OnDoubleTap_Handler;
  manager.OnLongTap += OnLongTap_Handler;
  manager.OnSwipe += OnSwipe_Handler;
  manager.OnDrag += OnDrag_Handler;
  manager.OnPinch += OnPinch_Handler;
}

private void OnDisable ()
{
  manager.OnPointerDown -= OnPointerDown_Handler;
  manager.OnPointerMoved -= OnPointerMoved_Handler;
  manager.OnPointerUp -= OnPointerUp_Handler;
  manager.OnTap -= OnTap_Handler;
  manager.OnDoubleTap -= OnDoubleTap_Handler;
  manager.OnLongTap -= OnLongTap_Handler;
  manager.OnSwipe -= OnSwipe_Handler;
  manager.OnDrag -= OnDrag_Handler;
  manager.OnPinch -= OnPinch_Handler;
}
```
3. Call `ScreenInputManager.Update(float)` on your `MonoBehavour.Update()`
```c#
private void Update ()
{
  manager.Update (Time.deltaTime);
}
```
### Event Type
```c#
public delegate void PointerEvent(IPointer pointer);
public delegate void TapEvent(Vector2 position,float totalTime);
public delegate void SwipeEvent(Vector2 direction,float totalTime);
public delegate void DragEvent(Vector2 position,Vector2 deltaPosition);
public delegate void PinchEvent(Vector2 position,float deltaScale);
```

# Contribution
* You can post issue for `Idea`, or `Bugs`
* You can create merge request
