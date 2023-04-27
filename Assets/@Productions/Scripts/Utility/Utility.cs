using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCode.Utils
{
    public static class Utils
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

        public static Vector3 RadianToVector3(float radian)
        {
            return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian));
        }

        public static Vector3 DegreeToVector3(float degree)
        {
            return RadianToVector3(degree * Mathf.Deg2Rad);
        }

        public static float FindAngleFromPoint(float pointY, float pointX)
        {
            var angle = Mathf.Atan2(pointY, pointX) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }

        public static float FindAngleBetweenTwoPoints(Vector2 pointFrom, Vector3 pointTo)
        {
            var angle = Mathf.Atan2(pointFrom.y - pointTo.y, pointFrom.x - pointTo.x) * Mathf.Rad2Deg;
            // if (angle < 0) angle += 360f;

            return angle;
        }

        public static bool CalculateCriticalChance(this float critChance)
        {
            return Random.Range(0, 100) < critChance;
        }

        public static float CalculateCriticalDamage(this float actualDamage, float damagePercentage)
        {
            return actualDamage + (actualDamage * (damagePercentage / 100f));
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

        public static bool GetMouseWorldPosition3D(out Vector3 position, LayerMask layer)
        {
            return GetMouseWorldPosition3D(out position, Camera.main, Mathf.Infinity, layer);
        }

        public static bool GetMouseWorldPosition3D(out Vector3 position, Camera cam, float range, LayerMask layer)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, range, layer))
            {
                position = hit.point;
                return true;
            }
            else
            {
                position = Vector3.zero;
                return false;
            }
        }
        #endregion

    }
}
