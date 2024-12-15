using System;
using UnityEngine;

using System;
using UnityEngine;
using System.Globalization;

namespace Helper.Math
{
    public class MathHelper
    {
        public static bool Chance(float percent)
        {
            return Random(0, 100) <= percent;
        }

        public static float CalculateAngleOf2Vector(Vector3 v1, Vector3 v2)
        {
            return Vector3.SignedAngle(v1, v2, Vector3.up);
        }

        public static Vector2 ConvertAngleToVector(float angleInDegree)
        {
            return new Vector2(Mathf.Cos(angleInDegree * Mathf.Deg2Rad), Mathf.Sin(angleInDegree * Mathf.Deg2Rad));
        }

        public static Quaternion ConvertDirectionToQuaternion(Vector3 direction)
        {
            return Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg, 0);
        }

        public static Quaternion ConvertDirectionToQuaternionOXZ(Vector3 direction)
        {
            return Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
        }

        public static float GetOxzAngleFromDirection(Vector3 direction)
        {
            return Mathf.Atan2(direction.x, direction.y);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static Vector3 RotateVector(Vector3 shootDirection, float currentRecoil)
        {
            return Quaternion.Euler(0, currentRecoil, 0) * shootDirection;
        }

        public static string ConvertNumberToText(long number)
        {
            if (number >= 100000 && number < 1000000)
            {
                float value = number / 100000f;
                return string.Format("{0:0.0}{1}", value, "K");
            }
            if (number >= 1000000 && number < 1000000000)
            {
                float value = number / 1000000f;
                return string.Format("{0:0.0}{1}", value, "M");
            }
            if (number >= 1000000000)
            {
                float value = number / 1000000000f;
                return string.Format("{0:0.0}{1}", value, "B");
            }
            return number.ToString();
        }

        public static double ParseDouble(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        public static int ConvertFloat(float value)
        {
            int convertedValue = (int)(value * 1000f);
            float newValue = convertedValue / 1000f;
            int differentValue = Mathf.RoundToInt((value - newValue) * 1000f);
            return convertedValue + differentValue;
            // switch (value)
            // {
            //     case Mathf.Infinity: return int.MaxValue;
            //     case 0.7f: return 700;
            //     case 0.04f: return 40;
            //     default: return (int)(value * 1000f);
            // }
        }

        public static decimal ConvertDecimal(float value)
        {
            return (decimal)(value * 1000);
        }
    }
}