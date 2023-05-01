using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
	public abstract class UIWidget : MonoBehaviour
	{
		// PUBLIC MEMBERS
		public bool IsVisible { get; private set; }

		[Header("General UI WIdget")]
		[SerializeField]
		[Tooltip("Visible Method On Initialized (AWAKE)")]
		private bool visibleOnInitialize = false;
		[SerializeField]
		[Tooltip("Visible Method On Enable Monobehaviour")]
		private bool visibleOnEnable = false;
		[SerializeField]
		[Tooltip("Hidden Method On Disable Monobehaviour")]
		private bool hiddenOnDisable = false;

		// PROTECTED MEMBERS
		protected bool IsInitalized { get; private set; }
		protected SceneUI SceneUI { get; private set; }		
		protected UIWidget Owner { get; private set; }

		[SerializeField]
		protected List<UIWidget> _childrens = new List<UIWidget>();

		// INTERNAL METHOD (CALLED FROM PARENT)
		internal void InitializeWidget(SceneUI sceneUI, UIWidget owner)
		{
			if (IsInitalized)
				return;

			SceneUI = sceneUI;
			Owner = owner;

			_childrens.Clear();
			GetChildWidgets(transform, _childrens);

			for (int i = 0; i < _childrens.Count; i++)
			{
				_childrens[i].InitializeWidget(sceneUI, this);
			}

			OnInitialize();

			IsInitalized = true;

			if (gameObject.activeInHierarchy || visibleOnInitialize)
				Visible();
		}

		internal void DeinitializeWidget()
		{
			if (!IsInitalized)
				return;

			Hidden();

			OnDeinitialize();

			for (int i = 0; i < _childrens.Count; i++)
			{
				_childrens[i].DeinitializeWidget();
			}

			_childrens.Clear();

			IsInitalized = false;

			SceneUI = null;
			Owner = null;
		}

		internal void Visible()
		{
			if (!IsInitalized)
				return;

			if (IsVisible)
				return;

			if (!gameObject.activeSelf)
				return;

			IsVisible = true;

			for (int i = 0; i < _childrens.Count; i++)
			{
				_childrens[i].Visible();
			}

			OnVisible();
		}

		internal void Hidden()
		{
			if (!IsVisible)
				return;

			IsVisible = false;

			OnHidden();

			for (int i = 0; i < _childrens.Count; i++)
			{
				_childrens[i].Hidden();
			}
		}

		internal void Tick()
		{
			if (!IsInitalized)
				return;

			if (!IsVisible)
				return;

			OnTick();

			for (int i = 0; i < _childrens.Count; i++)
			{
				_childrens[i].Tick();
			}
		}

		// AND AND REMOVE CHILD
		public void AddChild(UIWidget widget)
		{
			if (widget == null || widget == this)
				return;

			if (_childrens.Contains(widget))
			{
				Debug.LogError($"Widget {widget.name} is already added as child of {name}");
				return;
			}

			_childrens.Add(widget);

			widget.InitializeWidget(SceneUI, this);
		}

		public void RemoveChild(UIWidget widget)
		{
			int childIndex = _childrens.IndexOf(widget);

			if (childIndex < 0)
			{
				Debug.LogError($"Widget {widget.name} is not child of {name} and cannot be removed");
				return;
			}

			widget.DeinitializeWidget();

			_childrens.RemoveAt(childIndex);
		}

		// MONOBEHAVIOR METHOD
		protected virtual void OnEnable()
		{
			if(visibleOnEnable)
				Visible();
		}

		protected virtual void OnDisable()
		{
			if(hiddenOnDisable)
				Hidden();
		}

		// VIRTUAL METHOD
		public virtual bool IsActive() { return true; }
		protected virtual void OnInitialize() { }
		protected virtual void OnDeinitialize() { }
		protected virtual void OnVisible() { }
		protected virtual void OnHidden() { }
		protected virtual void OnTick() { }

		// PRIVATE METHOD
		private void GetChildWidgets(Transform transform, List<UIWidget> widgets)
		{
			foreach (Transform child in transform)
			{
				var childWidget = child.GetComponent<UIWidget>();

				if (childWidget != null)
				{
					widgets.Add(childWidget);
				}
				else
				{
					// Continue searching deeper in hierarchy
					GetChildWidgets(child, widgets);
				}
			}
		}
	}
}
