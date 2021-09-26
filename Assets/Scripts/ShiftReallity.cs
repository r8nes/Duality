using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftReallity : MonoBehaviour
{
    private Animator _animator;
    private bool isOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isOpen == false && Input.GetKeyDown(KeyCode.E))
        {
            _animator.SetBool("isOn", true);
            isOpen = true;
        }
        else if (isOpen == true && Input.GetKeyDown(KeyCode.E))
        {
            _animator.SetBool("isOn", false);

            isOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Detect(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Undetect(collision);
    }

    private void Detect(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.VisibleOutsideMask)
        {
            collision.isTrigger = true;
        }
        else if (collision.gameObject.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
        {
            collision.isTrigger = false;
        }
    }

    private void Undetect(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.VisibleOutsideMask)
        {
            collision.isTrigger = false;
        }
        else if (collision.gameObject.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
        {
            collision.isTrigger = true;
        }
    }
}
