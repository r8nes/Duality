using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftReallity : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private float _maskChecker;
    [SerializeField] private Collider2D[] _invisibleBlockColliders;
    [SerializeField] private LayerMask _groundLayer;
    private SpriteMask mask;
    private bool isOpen = false;
    private bool _isCollidersAppers;

    private void Start()
    {
        _animator = GetComponent<Animator>(); 
    }
    private void Update()
    {
        _maskChecker =transform.GetComponent<CircleCollider2D>().radius;
        _invisibleBlockColliders = Physics2D.OverlapCircleAll(transform.position, _maskChecker * 10f, _groundLayer);

        if (_invisibleBlockColliders.Length != 0)
        {
            _isCollidersAppers = true;  
        }

        HashSet<Collider2D> hashCollider = new HashSet<Collider2D>();

        for (int i = 0; i < _invisibleBlockColliders.Length; i++)
        {
            hashCollider.Add(_invisibleBlockColliders[i]);
        }

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
        if(_isCollidersAppers)
        {
            for (int i = 0; i < _invisibleBlockColliders.Length; i++)
            {
                Detect(_invisibleBlockColliders[i]);
            }
        }

        if (collision.gameObject.GetComponent<SpriteRenderer>().maskInteraction == SpriteMaskInteraction.VisibleOutsideMask)
        {
            collision.isTrigger = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Undetect(collision);
    }

    private void Detect(Collider2D visible)
    {      
        visible.isTrigger = false; 
    }

    private void Undetect(Collider2D block) 
    {
        switch (block.tag)
        {
            case "InvisibleBlock":
                block.isTrigger = true;
                break;
            case "ReverseBlock":
                block.isTrigger = false;
                break;
        }
        _isCollidersAppers = false;
    }
}
