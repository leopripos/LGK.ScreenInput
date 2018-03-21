//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace LGK.ScreenInput
{
    public class TouchPointer : IPointer
    {
        readonly ushort m_Id;
        readonly int m_FingerId;

        private PointerState m_State;
        private Vector2 m_Position;
        private Vector2 m_DeltaPosition;
        private bool m_IsOverIU;

        #region IPointer implementation

        public ushort Id
        {
            get
            {
                return m_Id;
            }
        }

        public Vector2 Position
        {
            get
            {
                return m_Position;
            }
        }

        public Vector2 DeltaPosition
        {
            get
            {
                return m_DeltaPosition;
            }
        }

        public PointerState State
        {
            get
            {
                return m_State;
            }
        }

        public bool IsOverUI
        {
            get
            {
                return m_IsOverIU;
            }
        }

        public void Validate()
        {
            Touch touch;
            if (TryGetTouch(m_FingerId, out touch))
            {
                m_State = ConvertState(touch.phase);

                if (EventSystem.current != null)
                {
                    m_IsOverIU = ScreenInputUtility.IsPointerOverUIObject(touch.position);
                }
                else
                {
                    m_IsOverIU = false;
                }

                if (m_State == PointerState.Moved)
                {
                    m_Position = touch.position;
                    m_DeltaPosition = touch.deltaPosition;
                }
            }
            else
            {
                m_State = PointerState.Expired;
            }

        }

        #endregion

        public TouchPointer(ushort id, Touch touch)
        {
            m_Id = id;
            m_FingerId = touch.fingerId;

            m_State = PointerState.New;
            if (EventSystem.current != null)
            {
                m_IsOverIU = ScreenInputUtility.IsPointerOverUIObject(touch.position);
            }
            else
            {
                m_IsOverIU = false;
            }
            m_Position = touch.position;
            m_DeltaPosition = Vector2.zero;
        }

        private bool TryGetTouch(int fingerId, out Touch touch)
        {
            var touchCount = Input.touchCount;
            var touches = Input.touches;
            for (int i = 0; i < touchCount; i++)
            {
                var temp = touches[i];
                if (m_FingerId == temp.fingerId)
                {
                    touch = temp;
                    return true;
                }
            }

            touch = default(Touch);
            return false;
        }

        public static PointerState ConvertState(TouchPhase phase)
        {
            if (phase == TouchPhase.Began)
            {
                return PointerState.New;
            }
            else if (phase == TouchPhase.Stationary)
            {
                return PointerState.Stationary;
            }
            else if (phase == TouchPhase.Moved)
            {
                return PointerState.Moved;
            }
            else //if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
            {
                return PointerState.Expired;
            }
        }
    }
}

