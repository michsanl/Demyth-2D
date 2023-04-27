using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomCode.Utils
{
    public static class Utility
    {
        public static float GetAngleFromVectorFloat(Vector3 direction)
        {
            direction = direction.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            if (angle <= 0) angle += 360;
            return angle;
        }

        public static bool IsLayerEqual(LayerMask layer_1, LayerMask layer_2)
        {
            return ((1 << layer_1) & layer_2) != 0;
        }

        /// <summary>
        /// Untuk mendapatkan random dari 2 distinct number float misalnya 0 atau 1 gitu
        /// </summary>
        /// <param name="firstNum"></param>
        /// <param name="secondNum"></param>
        /// <returns></returns>
        public static float RandomBetweenTwoNumbers(float firstNum, float secondNum)
        {
            float chance = Random.Range(0, 10);
            if (chance < 5)
            {
                return firstNum;
            }
            else
            {
                return secondNum;
            }
        }

        /// <summary>
        /// Sama seperti diatas namun return int
        /// </summary>
        /// <param name="firstNum"></param>
        /// <param name="secondNum"></param>
        /// <returns></returns>
        public static int RandomBetweenTwoNumbers(int firstNum, int secondNum)
        {
            float chance = Random.Range(0, 10);
            if (chance < 5)
            {
                return firstNum;
            }
            else
            {
                return secondNum;
            }
        }

        public static Vector2 GetScreenHalfSizeWorldUnit()
        {
            if (Camera.main.orthographic)
                return new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
            else
            {
                Debug.LogError("Camera not orthographic");
                return Vector2.zero;
            }
        }

        public static float Remap(float value, float a1, float a2, float b1, float b2)
        {
            return b1 + (value - a1) * (b2 - b1) / (a2 - a1);
        }

        #region GET WORLD POSITION FROM MOUSE

        public static Vector3 GetWorldPosition2D()
        {
            Vector3 mousePos = GetWorldPosition(Input.mousePosition, Camera.main);
            mousePos.z = 0;
            return mousePos;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetWorldPosition(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera cam)
        {
            return GetWorldPosition(Input.mousePosition, cam);
        }

        public static Vector3 GetWorldPosition(Vector3 input, Camera cam)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(input);
            return mousePos;
        }
        #endregion

        public static IEnumerator TimedAction(Action callBack, float time)
        {
            yield return new WaitForSeconds(time);
            callBack?.Invoke();
        }
    }
}
