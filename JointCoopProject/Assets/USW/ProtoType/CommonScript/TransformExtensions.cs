using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// 2D환경에서 타겟을 바라보게 회전시킵니다. (확장 메서드)
    /// </summary>
    public static void LookAt2D(this Transform transform, Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}