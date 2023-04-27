using CustomTools.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UISystem
{
	public class SceneUI : SceneService
	{
		// PUBLIC MEMBERS
		public Canvas Canvas   { get; private set; }
		public Camera UICamera { get; private set; }

		[SerializeField]
		private UIPageView[] _defaultViews;

		// SceneUI INTERFACE
		[SerializeField]
		protected List<UIPageView> _views = new List<UIPageView>();

        #region VIRTUAL METHOD
        protected virtual void OnInitializeInternal() { }
		protected virtual void OnDeinitializeInternal() { }
		protected virtual void OnTickInternal() { }
		protected virtual void OnViewOpened(UIPageView view) { }
		protected virtual void OnViewClosed(UIPageView view) { }
        #endregion

		protected override void OnInitialize()
		{
			Canvas   = GetComponent<Canvas>();
			UICamera = Canvas.worldCamera;
			_views = GetComponentsInChildren<UIPageView>(true).ToList();

			for (int i = 0; i < _views.Count; ++i)
			{
				UIPageView view = _views[i];

				view.InitializeWidget(this, null);
				view.ForceClose();
			}

			OnInitializeInternal();
		}

		protected override void OnDeinitialize()
		{
			OnDeinitializeInternal();

			if (_views != null)
			{
				for (int i = 0; i < _views.Count; ++i)
				{
					_views[i].DeinitializeWidget();
				}

				_views = null;
			}
		}

		protected override void OnActivate()
		{
			Canvas.enabled = true;

			for (int i = 0, count = _defaultViews.Length; i < count; i++)
			{
				Open(_defaultViews[i]);
			}
		}

		protected override void OnDeactivate()
		{
			if (Canvas != null)
			{
				Canvas.enabled = false;
			}
		}

		protected override void OnTick()
		{
			if (_views != null)
			{
				for (int i = 0; i < _views.Count; ++i)
				{
					UIPageView view = _views[i];
					if (view.IsOpen)
					{
                        view.Tick();
					}
				}
			}

			OnTickInternal();
		}
		
		// Public method for open and close UI Page
		public T GetView<T>() where T : UIPageView
		{
			for (int i = 0, count = _views.Count; i < count; i++)
			{
				if (_views[i] is T service)
					return service;
			}

			return null;
		}

		public T Open<T>() where T : UIPageView
		{
			if (_views == null)
				return null;

			for (int i = 0; i < _views.Count; ++i)
			{
				T view = _views[i] as T;
				if (view != null)
				{
					OpenView(view);
					return view;
				}
			}

			return null;
		}

		public T Close<T>() where T : UIPageView
		{
			if (_views == null)
				return null;

			for (int i = 0; i < _views.Count; ++i)
			{
				T view = _views[i] as T;
				if (view != null)
				{
					view.Close();
					return view;
				}
			}

			return null;
		}

		public void Open(UIPageView view)
		{
			if (_views == null)
				return;

			int index = _views.IndexOf(view);

			if (index < 0)
			{
				Debug.LogError($"Cannot find view {view.name}");
				return;
			}

			OpenView(view);
		}

		public void Close(UIPageView view)
		{
			if (_views == null)
				return;

			int index = _views.IndexOf(view);

			if (index < 0)
			{
				Debug.LogError($"Cannot find view {view.name}");
				return;
			}

			CloseView(view);
		}

		public T Toggle<T>() where T : UIPageView
		{
			if (_views == null)
				return null;

			for (int i = 0; i < _views.Count; ++i)
			{
				T view = _views[i] as T;
				if (view != null)
				{
					if (view.IsOpen)
					{
						CloseView(view);
					}
					else
					{
						OpenView(view);
					}

					return view;
				}
			}

			return null;
		}

		public bool IsOpen<T>() where T : UIPageView
		{
			if (_views == null)
				return false;

			for (int i = 0; i < _views.Count; ++i)
			{
				T view = _views[i] as T;
				if (view != null)
				{
					return view.IsOpen;
				}
			}

			return false;
		}

		public bool IsTopView(UIPageView view, bool interactableOnly = false)
		{
			if (view.IsOpen == false)
				return false;

			if (_views == null)
				return false;

			int highestPriority = -1;

			for (int i = 0; i < _views.Count; ++i)
			{
				var otherView = _views[i];

				if (otherView == view)
					continue;

				if (otherView.IsOpen == false)
					continue;

				if (interactableOnly == true && otherView.IsInteractable == false)
					continue;

				highestPriority = Math.Max(highestPriority, otherView.Priority);
			}

			return view.Priority > highestPriority;
		}

		public void CloseAll()
		{
			if (_views == null)
				return;

			for (int i = 0; i < _views.Count; ++i)
			{
				CloseView(_views[i]);
			}
		}

		public void GetAll<T>(List<T> list)
		{
			if (_views == null)
				return;

			for (int i = 0; i < _views.Count; ++i)
			{
				if (_views[i] is T element)
				{
					list.Add(element);
				}
			}
		}

		private void OpenView(UIPageView view)
		{
			if (view == null)
				return;

			if (view.IsOpen)
				return;

			view.OpenInternal();

			OnViewOpened(view);
		}

		private void CloseView(UIPageView view)
		{
			if (view == null)
				return;

			if (!view.IsOpen)
				return;

			view.CloseInternal();

			OnViewClosed(view);
		}
	}
}
