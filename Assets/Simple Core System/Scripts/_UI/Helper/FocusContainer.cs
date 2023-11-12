using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    [DefaultExecutionOrder(-2)]
    public class FocusContainer : MonoBehaviour
    {
        private Transform Container
        {
            get
            {
                if (transform == transform.root)
                {
                    Debug.LogError("Dont work in root objects !");
                    return null;
                }

                return transform;
            }
        }

        [Header("ENABLE ALL CONTAINERS")]
        public bool OnSelectNothing = true;
        public bool OnSelectOnAnotherRoot = false;
        public bool OnSelectItself = false;
        public bool OnSelectOwnRoot = true;
        public bool OnSelectOnSibling = false;

        private void Awake()
        {
            ToggleAll(true);
        }

        public void SelectPage(Transform transform)
        {
            Transform selectedTransform = transform;
            if (ShouldReturn(selectedTransform)) return;

            int stepsFromContainer = CalculateStepsFromContainer(selectedTransform);
            ActivateSelectedContainer(stepsFromContainer, selectedTransform);
        }

        private bool ShouldReturn(Transform t)
        {
            if (t == null)
            {
                if (OnSelectNothing)
                    ToggleAll(true);

                return true;
            }

            if (t.root != Container.root)
            {
                if (OnSelectOnAnotherRoot)
                    ToggleAll(true);

                return true;
            }

            if (t == Container)
            {
                if (OnSelectItself)
                    ToggleAll(true);

                return true;
            }

            if (t == Container.root)
            {
                if (OnSelectOwnRoot)
                    ToggleAll(true);

                return true;
            }

            if (!IsInsideOfContainer(t))
            {
                if (OnSelectOnSibling)
                    ToggleAll(true);

                return true;
            }

            return false;
        }

        private bool IsInsideOfContainer(Transform t)
        {
            bool isInside = false;

            while (t.parent != null)
            {
                if (t == Container)
                {
                    isInside = true;
                    break;
                }
                t = t.parent;
            }

            return isInside;
        }

        private int CalculateStepsFromContainer(Transform t)
        {
            int _stepsFromContainer = 0;
            while (t != Container)
            {
                _stepsFromContainer++;
                t = t.parent;

                //Lets say that Im little paranoic about 'while' loops
                if (_stepsFromContainer > 100)
                    break;
            }

            return _stepsFromContainer;
        }

        private void ToggleAll(bool option)
        {
            for (int i = 0; i < Container.childCount; i++)
            {
                Container.GetChild(i).gameObject.SetActive(option);
            }
        }

        private void ActivateSelectedContainer(int levels, Transform t)
        {
            Transform selectedContainer = t;
            for (int i = 0; i < levels - 1; i++)
            {
                selectedContainer = selectedContainer.parent;
            }

            ToggleAll(false);
            selectedContainer.gameObject.SetActive(true);
        }
    }
}