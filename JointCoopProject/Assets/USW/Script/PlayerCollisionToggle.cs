using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionToggle : MonoBehaviour
{
    public GameObject targetObject;
    public float deactivateDelay = 1f;
    public float activateDelay = 1f;
    
    private bool hasPlayerEntered = false;
    private bool isPlayerInside = false;
    private Coroutine currentCoroutine;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;
        
     
        isPlayerInside = true;
        
        if (!hasPlayerEntered)
        {
            hasPlayerEntered = true;
            StartToggle(false, deactivateDelay); 
        }
        else
        {
            StartToggle(true, activateDelay); 
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;
        
        isPlayerInside = false;
        
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
    
    bool IsPlayer(Collider2D other)
    {
        return other.CompareTag("Player");
    }
    
    
    void StartToggle(bool activate, float delay)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        
        currentCoroutine = StartCoroutine(ToggleCoroutine(activate, delay));
    }
    
   IEnumerator ToggleCoroutine(bool activate, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (!activate || isPlayerInside)
        {
            if (targetObject != null)
            {
                targetObject.SetActive(activate);
            }
        }
        
        currentCoroutine = null;
    }
}