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
    public class ShortTapGestureMatcher
    {
        readonly float m_MaxTime;
        readonly float m_MaxDistance;
        readonly float m_MaxSpaceTime;

        private MatchingState m_State;
        private ushort m_WaitedPointer;
        private Vector2 m_FirstPosition;
        private float m_WaitedTime;
        private float m_WaitedSpaceTime;
        private byte m_Count;

        public ShortTapGestureMatcher(float maxTime, float maxSpaceTime, float maxDistance)
        {
            m_MaxTime = maxTime;
            m_MaxSpaceTime = maxSpaceTime;
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

        public byte Count
        {
            get { return m_Count; }
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
            m_WaitedSpaceTime = 0;
            m_FirstPosition = Vector2.zero;
            m_Count = 0;
        }

        public void Process(IList<IPointer> pointers, float deltaTime)
        {
            if (pointers.Count == 1)
            {
                var pointer = pointers[0];

                if ((m_State == MatchingState.WaitingFirstDown || m_State == MatchingState.WaitingSecondDown) && pointer.State == PointerState.New && !pointer.IsOverUI)
                {
                    m_State = (m_State == MatchingState.WaitingFirstDown) ? MatchingState.WaitingFirstUp : MatchingState.WaitingSecondUp;
                    m_WaitedPointer = pointer.Id;
                    m_FirstPosition = pointer.Position;
                }
                else if ((m_State == MatchingState.WaitingFirstUp || m_State == MatchingState.WaitingSecondUp) && pointer.Id == m_WaitedPointer)
                {
                    m_WaitedTime += deltaTime;

                    if (pointer.State == PointerState.Expired && m_State == MatchingState.WaitingFirstUp)
                    {
                        var distance = Vector2.Distance(m_FirstPosition, pointer.Position);
                        if (distance <= m_MaxDistance)
                        {
                            m_State = MatchingState.WaitingSecondDown;
                        }
                        else
                        {
                            Reset();
                        }
                    }
                    else if (pointer.State == PointerState.Expired && m_State == MatchingState.WaitingSecondUp && m_WaitedTime <= m_MaxTime)
                    {
                        var distance = Vector2.Distance(m_FirstPosition, pointer.Position);
                        if (distance <= m_MaxDistance)
                        {
                            m_Count = 2;
                            m_State = MatchingState.Accepted;
                        }
                        else
                        {
                            Reset();
                        }
                    }
                    else if (m_WaitedTime > m_MaxTime)
                    {
                        Reset();
                    }
                }
                else
                {
                    Reset();   
                }
            }
            else if (pointers.Count == 0 && m_State == MatchingState.WaitingSecondDown)
            {
                m_WaitedSpaceTime += deltaTime;

                if (m_WaitedSpaceTime >= m_MaxSpaceTime)
                {
                    m_Count = 1;
                    m_State = MatchingState.Accepted;
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
            WaitingFirstUp,
            WaitingSecondDown,
            WaitingSecondUp,
            Accepted
        }
    }
}

