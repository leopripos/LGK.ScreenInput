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
    public class DragGestureMatcher
    {
        readonly float m_MinStartingDistance;

        private MatchingState m_State;
        private ushort m_WaitedPointer;
        private Vector2 m_LastPosition;
        private Vector2 m_DeltaPosition;

        public DragGestureMatcher(float minStartingDistance)
        {
            m_MinStartingDistance = minStartingDistance;
        }

        public bool Success
        {
            get { return m_State == MatchingState.Accepted; }
        }

        public Vector2 Position
        {
            get { return m_LastPosition; }
        }

        public Vector2 DeltaPosition
        {
            get { return m_DeltaPosition; }
        }

        public void PostSuccess()
        {
            m_State = MatchingState.WaitingFinishDrag;
        }

        private void Reset()
        {
            if (m_State == MatchingState.WaitingDown)
                return;
            
            m_State = MatchingState.WaitingDown;
            m_WaitedPointer = 0;
            m_LastPosition = Vector2.zero;
            m_DeltaPosition = Vector2.zero;
        }

        public void Process(IList<IPointer> pointers, float deltaTime)
        {
            if (pointers.Count == 1)
            {
                var pointer = pointers[0];

                if (m_State == MatchingState.WaitingDown && pointer.State == PointerState.New && !pointer.IsOverUI)
                {
                    m_State = MatchingState.WaitingStartDrag;
                    m_WaitedPointer = pointer.Id;
                    m_LastPosition = pointer.Position;
                }
                else if (m_State == MatchingState.WaitingFinishDrag && pointer.Id == m_WaitedPointer && (pointer.State == PointerState.Moved || pointer.State == PointerState.Stationary))
                {
                    m_DeltaPosition = pointer.Position - m_LastPosition;
                    m_LastPosition = pointer.Position;
                    m_State = MatchingState.Accepted;
                }
                else if (m_State == MatchingState.WaitingStartDrag && pointer.Id == m_WaitedPointer && (pointer.State == PointerState.Moved || pointer.State == PointerState.Stationary))
                {
                    var distance = Vector2.Distance(m_LastPosition, pointer.Position);

                    if (distance >= m_MinStartingDistance)
                    {
                        m_DeltaPosition = pointer.Position - m_LastPosition;
                        m_LastPosition = pointer.Position;
                        m_State = MatchingState.Accepted;
                    }
                }
                else
                {
                    Reset();   
                }
            }
            else
            {
                Reset();
            }
        }

        private enum MatchingState
        {
            WaitingDown,
            WaitingStartDrag,
            Accepted,
            WaitingFinishDrag
        }
    }
}

