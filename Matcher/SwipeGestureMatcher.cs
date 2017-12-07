//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using UnityEngine;
using System.Collections.Generic;

namespace LGK.ScreenInput
{
    public class SwipeGestureMatcher
    {
        readonly float m_MaxTime;
        readonly float m_MinDistance;

        private MatchingState m_State;
        private ushort m_WaitedPointer;
        private Vector2 m_FirstPosition;
        private Vector2 m_Direction;
        private float m_WaitedTime;

        public bool Success
        {
            get { return m_State == MatchingState.Accepted; }
        }

        public Vector2 Direction
        {
            get { return m_Direction; }
        }

        public float TotalTime
        {
            get { return m_WaitedTime; }
        }

        public SwipeGestureMatcher(float maxTime, float minDistance)
        {
            m_MaxTime = maxTime;
            m_MinDistance = minDistance;
        }

        public void PostSuccess()
        {
            Reset();
        }

        private void Reset()
        {
            if (m_State == MatchingState.WaitingDown)
                return;
            
            m_State = MatchingState.WaitingDown;
            m_WaitedPointer = 0;
            m_WaitedTime = 0;
            m_FirstPosition = Vector2.zero;
            m_Direction = Vector2.zero;
        }

        public void Process(IList<IPointer> pointers, float deltaTime)
        {
            if (pointers.Count == 1)
            {
                var pointer = pointers[0];

                if (m_State == MatchingState.WaitingDown && pointer.State == PointerState.New && !pointer.IsOverUI)
                {
                    m_State = MatchingState.WaitingUp;
                    m_WaitedPointer = pointer.Id;
                    m_FirstPosition = pointer.Position;
                }
                else if (m_State == MatchingState.WaitingUp && pointer.Id == m_WaitedPointer)
                {
                    m_WaitedTime += deltaTime;

                    if (pointer.State == PointerState.Expired && m_WaitedTime <= m_MaxTime)
                    {
                        var distance = Vector2.Distance(m_FirstPosition, pointer.Position);
                        if (distance >= m_MinDistance)
                        {
                            m_Direction = (pointer.Position - m_FirstPosition).normalized;
                            m_State = MatchingState.Accepted;
                        }
                        else
                        {
                            Reset();
                        }
                    }
                    else if (pointer.State == PointerState.Expired || m_WaitedTime > m_MaxTime)
                    {
                        Reset();
                    }
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
            WaitingUp,
            Accepted
        }
    }
}

