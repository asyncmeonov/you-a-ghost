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
}