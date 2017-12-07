//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using UnityEngine;
using UnityEngine.EventSystems;

namespace LGK.ScreenInput
{
    public class MousePointer : IPointer
    {
        readonly ushort m_Id;
        readonly int m_ButtonId;

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
            m_IsOverIU = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

            if (Input.GetMouseButton(m_ButtonId))
            {
                var newPosition = (Vector2)Input.mousePosition;

                m_DeltaPosition = newPosition - m_Position;
                m_Position = newPosition;

                if (m_DeltaPosition.x >= 1 || m_DeltaPosition.x >= 1)
                {
                    m_State = PointerState.Moved;
                }
                else
                {
                    m_State = PointerState.Stationary;
                }
            }
            else
            {
                m_State = PointerState.Expired;
            }
        }

        #endregion

        public MousePointer(ushort id, int mouseButton)
        {
            m_Id = id;
            m_ButtonId = mouseButton;

            m_State = PointerState.New;
            m_IsOverIU = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
            m_Position = Input.mousePosition;
            m_DeltaPosition = Vector2.zero;
        }
    }
}

