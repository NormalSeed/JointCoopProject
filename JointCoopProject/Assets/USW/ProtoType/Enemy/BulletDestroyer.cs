using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f); // 5초 후 제거
    }
}