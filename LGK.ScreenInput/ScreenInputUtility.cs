//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace LGK.ScreenInput
{
    public static class ScreenInputUtility
    {
        public static ESwipe4Direction ConvertSwipeTo4Direction(Vector2 direction)
        {
            Assert.IsFalse(direction == Vector2.zero);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                    return ESwipe4Direction.Right;
                else
                    return ESwipe4Direction.Left;
            }
            else
            {
                if (direction.y > 0)
                    return ESwipe4Direction.Up;
                else
                    return ESwipe4Direction.Down;
            }
        }

        public static ESwipe8Direction ConvertSwipeTo8Direction(Vector2 direction)
        {
            Assert.IsFalse(direction == Vector2.zero);

            var angle = Vector2.Angle(Vector2.up, direction);

            if (direction.x > 0)
            {
                if (angle <= 22.5f)
                    return ESwipe8Direction.Up;
                if (angle <= 67.5f)
                    return ESwipe8Direction.RightUp;
                if (angle <= 112.5f)
                    return ESwipe8Direction.Right;
                if (angle <= 157.5)
                    return ESwipe8Direction.RighDown;

                return ESwipe8Direction.Down;
            }
            else
            {
                if (angle <= 22.5f)
                    return ESwipe8Direction.Up;
                if (angle <= 67.5f)
                    return ESwipe8Direction.LeftUp;
                if (angle <= 112.5f)
                    return ESwipe8Direction.Left;
                if (angle <= 157.5)
                    return ESwipe8Direction.LeftDown;

                return ESwipe8Direction.Down;
            }

        }

        public static bool IsPointerOverUIObject(Vector2 position)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = position;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}

