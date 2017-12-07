//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using System.Collections.Generic;
using UnityEngine;

namespace LGK.ScreenInput
{
    public class PinchZoomGesture
    {
        private MatchingState m_State;
        private Vector2 m_CenterPosition;
        private float m_DeltaScale;
        private float m_LastDistance;

        public PinchZoomGesture()
        {
           
        }

        public bool Success
        {
            get { return m_State == MatchingState.Accepted; }
        }

        public Vector2 FocusPosition
        {
            get { return m_CenterPosition; }
        }

        public float DeltaScale
        {
            get { return m_DeltaScale; }
        }

        public void PostSuccess()
        {
            m_DeltaScale = 0;
            m_State = MatchingState.WaitingMove;
        }

        private void Reset()
        {
            if (m_State == MatchingState.WaitingMove)
                return;

            m_State = MatchingState.WaitingMove;
            m_CenterPosition = Vector2.zero;
            m_LastDistance = 0;
        }

        public void Process(IList<IPointer> pointers, float deltaTime)
        {
            #if UNITY_STANDALONE || UNITY_EDITOR
            var mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            #endif

            if (pointers.Count == 2)
            {
                var firstPointer = pointers[0];
                var secondPointer = pointers[1];

                if ((firstPointer.State == PointerState.Moved || secondPointer.State == PointerState.Moved) && m_State == MatchingState.WaitingMove && (!firstPointer.IsOverUI || !secondPointer.IsOverUI))
                {
                    var firstPosition = firstPointer.Position;
                    var secondPosition = secondPointer.Position;

                    var newDistance = Vector2.Distance(firstPosition, secondPosition);

                    m_DeltaScale = (newDistance - m_LastDistance) / m_LastDistance;
                    m_CenterPosition = new Vector2((firstPosition.x + secondPosition.x) / 2, (firstPosition.y + secondPosition.y) / 2);
                    m_State = MatchingState.Accepted;
                }
            }
            #if UNITY_STANDALONE || UNITY_EDITOR
            else if (mouseWheel > 0)
            {
                m_CenterPosition = Input.mousePosition;
                m_DeltaScale = mouseWheel / 1;
                m_State = MatchingState.Accepted;
            }
            else if (mouseWheel < 0)
            {
                m_CenterPosition = Input.mousePosition;
                m_DeltaScale = mouseWheel / 1;
                m_State = MatchingState.Accepted;
            }
            #endif
            else
            {
                Reset();
            }
        }

        private enum MatchingState
        {
            WaitingMove,
            Accepted
        }
    }
}

