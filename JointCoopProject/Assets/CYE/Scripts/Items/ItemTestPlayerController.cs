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
}
