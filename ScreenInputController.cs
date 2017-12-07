//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using System;
using UnityEngine;
using System.Collections.Generic;

namespace LGK.ScreenInput
{
    public class ScreenInputController
    {
        readonly IList<IPointer> m_Pointers;

        private ushort m_IdCounter;

        public IList<IPointer> Pointers
        {
            get { return m_Pointers; }
        }

        public ScreenInputController(IList<IPointer> pointersCollector)
        {
            m_Pointers = pointersCollector;
        }

        private ushort GenerateNewId()
        {
            m_IdCounter++;
            return m_IdCounter;
        }

        public void Update(float deltaTime)
        {
            ClearExpiredPointer();
            ValidatePointer();

            #if UNITY_STANDALONE || UNITY_EDITOR
            ReadMouseInput();
            #else
            ReadTouchInput();
            #endif
        }

        public void Reset()
        {
            m_Pointers.Clear();
        }

        private void ClearExpiredPointer()
        {
            var pointers = m_Pointers;

            int index = 0;
            while (index < pointers.Count)
            {
                var pointer = pointers[index];
                if (pointer.State == PointerState.Expired)
                {
                    pointers.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        private void ValidatePointer()
        {
            var pointers = m_Pointers;
            var pointerLength = pointers.Count;
            for (int i = 0; i < pointerLength; i++)
            {
                pointers[i].Validate();
            }
        }

        private void ReadMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pointer = new MousePointer(GenerateNewId(), 0);
                m_Pointers.Add(pointer);
            }
            if (Input.GetMouseButtonDown(1))
            {
                var pointer = new MousePointer(GenerateNewId(), 1);
                m_Pointers.Add(pointer);
            }
            if (Input.GetMouseButtonDown(2))
            {
                var pointer = new MousePointer(GenerateNewId(), 2);
                m_Pointers.Add(pointer);
            }
        }

        private void ReadTouchInput()
        {
            var touches = Input.touches;
            var touchCount = Input.touchCount;
            for (int i = 0; i < touchCount; i++)
            {
                var touch = touches[i];
                if (touch.phase == TouchPhase.Began)
                {
                    var pointer = new TouchPointer(GenerateNewId(), touch);
                    m_Pointers.Add(pointer);
                }
            }
        }
    }
}

