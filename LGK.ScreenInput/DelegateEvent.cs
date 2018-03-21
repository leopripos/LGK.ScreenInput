//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using UnityEngine;

namespace LGK.ScreenInput
{
    public delegate void PointerEvent(IPointer pointer);
    public delegate void TapEvent(Vector2 position,float totalTime);
    public delegate void SwipeEvent(Vector2 direction,float totalTime);
    public delegate void DragEvent(Vector2 position,Vector2 deltaPosition);
    public delegate void PinchEvent(Vector2 position,float deltaScale);
}

