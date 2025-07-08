using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTestPlayerController : MonoBehaviour
{
    private bool _canItemPickUp = true;
    private Coroutine _delayRoutine = null;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TempManager.inventory.UseBomb(transform);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempManager.inventory.UseActiveSkill(transform);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_canItemPickUp && collision.transform.CompareTag("Item"))
        {
            collision.transform.GetComponent<IPickable>().PickUp(transform);
            if (_delayRoutine == null)
            { 
                _delayRoutine = StartCoroutine(DelayItemPickUp());
            }
        }
    }    

    private IEnumerator DelayItemPickUp()
    {
        _canItemPickUp = false;
        yield return new WaitForSeconds(0.1f);

        _canItemPickUp = true;
        if (_delayRoutine != null)
        {
            StopCoroutine(_delayRoutine);
            _delayRoutine = null;
        }
    }
}
