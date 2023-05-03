using System;
using UnityEngine;

public static class VectorUtils
{


    public static Quaternion GetQuaternionRotationTowards(Transform source, Transform target, float rotSpeed = 1500.0f)
    {
        return GetQuaternionRotationTowards(source, target.position, rotSpeed);
    }

    public static Quaternion GetQuaternionRotationTowards(Transform source, Vector3 target, float rotSpeed = 1500.0f)
    {
        float angle = Mathf.Atan2(target.y - source.position.y, target.x - source.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        return Quaternion.RotateTowards(source.transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    public static Vector2 ClampMagnituteAroundPoint(Vector2 target, Vector2 pivot, float max)
    {
        //consider making it radial?
        target.x = Math.Clamp(target.x, pivot.x - max, pivot.x + max);
        target.y = Math.Clamp(target.y, pivot.y - max, pivot.y + max);

        return target;
    }
}