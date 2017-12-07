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
    public interface IPointer
    {
        ushort Id { get; }

        Vector2 Position { get; }

        Vector2 DeltaPosition { get; }

        PointerState State { get; }

        bool IsOverUI { get; }

        void Validate();
    }
}

