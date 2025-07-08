using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeDamage : MonoBehaviour
{
    private float activatedTime = 0f;
    
    [SerializeField]
    private Tilemap spikeTilemap;
    
    [SerializeField]
    private float damageCooldown = 3f;
    private int frameCount = 3; 
    private float speed = 4f;   

    private float lastDamageTime = -999f;
    
    void Awake() {
        spikeTilemap = GetComponent<Tilemap>();
    }

    void OnEnable()
    {
        activatedTime = Time.time;
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        
        if (!other.CompareTag("Player")) return;
        
        Vector3 playerPos = other.transform.position;
        Vector3Int cellPos = spikeTilemap.WorldToCell(playerPos);
        
        int currentFrame = (int)((Time.time -activatedTime)* speed) % frameCount;
    
       
        if ((currentFrame == 1 || currentFrame == 2) && Time.time - lastDamageTime > damageCooldown)
        {
            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(1, transform.position);
                lastDamageTime = Time.time;
                Debug.Log("Damaged 1");
            }
        }
    }
}