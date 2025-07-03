using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTestPlayerController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            TempManager.inventory.UseBomb(transform.GetChild(0).transform);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            collision.transform.GetComponent<IPickable>().PickUp(transform);
        }
    }
}
