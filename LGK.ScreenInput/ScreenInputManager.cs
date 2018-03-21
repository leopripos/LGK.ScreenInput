//
// Author: Leo Pripos Marbun <leopripos@gmail.com>
// Ask author for more information
//
// Copyright (c) 2017 NED Studio
// See the LICENSE file in the project root directory for more information.
//
using System;
using System.Collections.Generic;

namespace LGK.ScreenInput
{
	public class ScreenInputManager
	{
		readonly IList<IPointer> m_Pointers = new List<IPointer> ();

		readonly ScreenInputController m_InputController;
		readonly ShortTapGestureMatcher m_ShortTapGestureMatcher;
		readonly LongTapGestureMatcher m_LongTapGestureMatcher;
		readonly SwipeGestureMatcher m_SwipeGestureMatcher;
		readonly DragGestureMatcher m_DragGestureMatcher;
		readonly PinchZoomGesture m_PinchGestureMatcher;

		public PointerEvent OnPointerDown;
		public PointerEvent OnPointerMoved;
		public PointerEvent OnPointerUp;

		public TapEvent OnTap;
		public TapEvent OnDoubleTap;
		public TapEvent OnLongTap;

		public SwipeEvent OnSwipe;
		public DragEvent OnDrag;
		public PinchEvent OnPinch;

		public IList<IPointer> Pointers {
			get { return m_Pointers; }
		}

		public ScreenInputManager ()
		{
			m_InputController = new ScreenInputController (m_Pointers);

			m_ShortTapGestureMatcher = new ShortTapGestureMatcher (0.25f, 0.1f, 50f);
			m_LongTapGestureMatcher = new LongTapGestureMatcher (0.35f, 10f);
			m_SwipeGestureMatcher = new SwipeGestureMatcher (0.3f, 50f);
			m_DragGestureMatcher = new DragGestureMatcher (0.5f);
			m_PinchGestureMatcher = new PinchZoomGesture ();
		}

		public void Update (float deltaTime)
		{
			UpdateController (deltaTime);
			CheckMatchGesture (deltaTime);
			BroadcastEvent ();
			PostSuccessGesture ();
		}

		public void Reset ()
		{
			ResetController ();
		}

		private void CheckMatchGesture (float deltaTime)
		{
			var pointers = m_Pointers;

			m_ShortTapGestureMatcher.Process (pointers, deltaTime);
			m_LongTapGestureMatcher.Process (pointers, deltaTime);
			m_SwipeGestureMatcher.Process (pointers, deltaTime);
			m_DragGestureMatcher.Process (pointers, deltaTime);
			m_PinchGestureMatcher.Process (pointers, deltaTime);
		}

		private void UpdateController (float deltaTime)
		{
			m_InputController.Update (deltaTime);
		}

		private void ResetController ()
		{
			m_InputController.Reset ();
		}

		private void BroadcastEvent ()
		{
			#region Pointer Event 
			var hasPointerDownEvent = (OnPointerDown != null);
			var hasPointerMovedEvent = (OnPointerMoved != null);
			var hasPointerUpEvent = (OnPointerUp != null);

			var pointers = m_Pointers;
			var pointersLength = pointers.Count;
			for (int i = 0; i < pointersLength; i++) {
				var pointer = pointers [i];
				if (hasPointerDownEvent && pointer.State == PointerState.New) {
					OnPointerDown.Invoke (pointer);
				} else if (hasPointerMovedEvent && pointer.State == PointerState.Moved) {
					OnPointerMoved.Invoke (pointer);
				} else if (hasPointerUpEvent && pointer.State == PointerState.Expired) {
					OnPointerUp.Invoke (pointer);
				}
			}
			#endregion

			#region Tap Event

			if (m_ShortTapGestureMatcher.Success) {
				if (m_ShortTapGestureMatcher.Count == 1 && OnTap != null) {
					OnTap.Invoke (m_ShortTapGestureMatcher.Position, m_ShortTapGestureMatcher.TotalTime);   
				} else if (OnDoubleTap != null) {
					OnDoubleTap.Invoke (m_ShortTapGestureMatcher.Position, m_ShortTapGestureMatcher.TotalTime);   
				}
			}

			if (m_LongTapGestureMatcher.Success && OnLongTap != null) {
				OnLongTap.Invoke (m_LongTapGestureMatcher.Position, m_LongTapGestureMatcher.TotalTime);
			}
			#endregion

			#region Move Event
			if (m_SwipeGestureMatcher.Success && OnSwipe != null) {
				OnSwipe.Invoke (m_SwipeGestureMatcher.Direction, m_SwipeGestureMatcher.TotalTime);
			}

			if (m_DragGestureMatcher.Success && OnDrag != null) {
				OnDrag.Invoke (m_DragGestureMatcher.Position, m_DragGestureMatcher.DeltaPosition);
			}

			if (m_PinchGestureMatcher.Success && OnPinch != null) {
				OnPinch.Invoke (m_PinchGestureMatcher.FocusPosition, m_PinchGestureMatcher.DeltaScale);
			}
			#endregion
		}

		private void PostSuccessGesture ()
		{
			if (m_ShortTapGestureMatcher.Success)
				m_ShortTapGestureMatcher.PostSuccess ();

			if (m_LongTapGestureMatcher.Success)
				m_LongTapGestureMatcher.PostSuccess ();

			if (m_SwipeGestureMatcher.Success)
				m_SwipeGestureMatcher.PostSuccess ();

			if (m_DragGestureMatcher.Success)
				m_DragGestureMatcher.PostSuccess ();

			if (m_PinchGestureMatcher.Success)
				m_PinchGestureMatcher.PostSuccess ();
		}
	}
}

