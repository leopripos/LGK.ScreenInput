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
    public class LongTapGestureMatcher
    {
        readonly float m_MinTime;
        readonly float m_MaxDistance;

        private MatchingState m_State;
        private ushort m_WaitedPointer;
        private Vector2 m_FirstPosition;
        private float m_WaitedTime;

        public LongTapGestureMatcher(float minTime, float maxDistance)
        {
            m_MinTime = minTime;
            m_MaxDistance = maxDistance;
        }

        public bool Success
        {
            get { return m_State == MatchingState.Accepted; }
        }

        public Vector2 Position
        {
            get { return m_FirstPosition; }
        }

        public float TotalTime
        {
            get { return m_WaitedTime; }
        }

        public void PostSuccess()
        {
            Reset();
        }

        private void Reset()
        {
            if (m_State == MatchingState.WaitingFirstDown)
                return;
                
            m_State = MatchingState.WaitingFirstDown;
            m_WaitedPointer = 0;
            m_WaitedTime = 0;
            m_FirstPosition = Vector2.zero;
        }

        public void Process(IList<IPointer> pointers, float deltaTime)
        {
            if (pointers.Count == 1)
            {
                var pointer = pointers[0];

                if (m_State == MatchingState.WaitingFirstDown && pointer.State == PointerState.New && !pointer.IsOverUI)
                {
                    m_State = MatchingState.WaitingLongTime;
                    m_WaitedPointer = pointer.Id;
                    m_FirstPosition = pointer.Position;
                }
                else if (pointer.Id == m_WaitedPointer && m_State == MatchingState.WaitingLongTime && pointer.State == PointerState.Stationary)
                {
                    m_WaitedTime += deltaTime;

                    if (m_WaitedTime >= m_MinTime)
                    {
                        var distance = Vector2.Distance(m_FirstPosition, pointer.Position);
                        if (distance <= m_MaxDistance)
                        {
                            m_State = MatchingState.Accepted;
                        }
                        else
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
            else
            {
                Reset();
            }
        }

        private enum MatchingState
        {
            WaitingFirstDown,
            WaitingLongTime,
            Accepted
        }
    }
}

