using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float TimeToMove = .2f;
    public float SaveTime = 1f;
    public float attackTimer = 1f;
    public LayerMask Obstacle;
    public GameObject bomb;
    public float HealthPoints;

    private SpriteRenderer sprite;
    private float _saveTimer = 0f;
    private bool isMoving = false;
    private bool isSaved = false;
    private bool canAttack = true;
    private float moveUp, moveRight;
    private float _attackTimer;
    private Vector3 origPos, destPos;

    protected Animator animator;

    protected Vector3 UpMoveVector = new Vector3(0.15f, 0f, 0f);

    protected Vector3 RightMoveVector = new Vector3(0.1f, 0f, 0f);


    private void Start()
    {
        animator = GetComponent<Animator>();
        _saveTimer = SaveTime;
        _attackTimer = attackTimer;
        sprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        ChangeAttackState();
        moveUp = CrossPlatformInputManager.GetAxis("Vertical");
        moveRight = CrossPlatformInputManager.GetAxis("Horizontal");
        ChangeSaveState();
        ChangeMovement();
        if (CrossPlatformInputManager.GetButton("Fire") && canAttack){
            PlantBomb();
            _attackTimer = attackTimer;
            canAttack = false;
        }
    }

    private void ChangeMovement()
    {
        if (!isMoving)
        {
            DisableAnimationBools(animator);
            if (moveRight != 0f && Mathf.Abs(moveRight) > Mathf.Abs(moveUp))
            {
                if (moveRight > 0f)
                {
                    moveRight = 1f;
                    animator.SetBool("MoveRight", true);
                }
                else
                {
                    moveRight = -1f;
                    animator.SetBool("MoveLeft", true);
                }
                StartCoroutine(MoveToPoint(Vector3.right * moveRight + RightMoveVector * moveRight));
            }
            else if (moveUp != 0 && Mathf.Abs(moveRight) < Mathf.Abs(moveUp)) // Set Animation bools
            {
                if (moveUp > 0f)
                {
                    moveUp = 1f;
                    animator.SetBool("MoveUp", true);
                }
                else
                {
                    moveUp = -1f;
                    animator.SetBool("MoveDown", true);
                }
                StartCoroutine(MoveToPoint(Vector3.up * moveUp + UpMoveVector * moveUp));
            }
        }
    }

    private void ChangeSaveState()
    {
        if (isSaved)
        {
            _saveTimer -= Time.deltaTime;
        }
        if (_saveTimer < 0f)
        {
            isSaved = false;
            _saveTimer = SaveTime;
        }
    }

    private void PlantBomb()
    {
        Instantiate(bomb, transform.position,transform.rotation, transform.parent);
    }
    protected bool isAvaliableDirection(Vector3 direction)
    {
        bool isAvaliable = true;
        if (!Physics2D.OverlapCircle(destPos, .3f, Obstacle))
            isAvaliable = false;
        return isAvaliable;
    }
    protected IEnumerator MoveToPoint(Vector3 destination)
    {
       
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        destPos = origPos + destination;
        if (!Physics2D.OverlapCircle(destPos, .25f, Obstacle))
        {
            while (elapsedTime < TimeToMove)
            {
                transform.position = Vector3.Lerp(origPos, destPos, (elapsedTime / TimeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = destPos;
        }

        isMoving = false;
    }
    protected void DisableAnimationBools(Animator animator)
    {
        animator.SetBool("MoveLeft", false);
        animator.SetBool("MoveRight", false);
        animator.SetBool("MoveUp", false);
        animator.SetBool("MoveDown", false);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GetDamage();
        }
    }

    public void GetDamage()
    {
        if (!isSaved)
        {
            HealthPoints -= 1f;
            if (HealthPoints < 1f)
            {
                Destroy(gameObject);
            }
            isSaved = true;
            StartCoroutine(ChangeSpriteColor());
        }
    }
    private IEnumerator ChangeSpriteColor()
    {
        sprite.color = Color.clear;
        yield return new WaitForSeconds(.1f);
        sprite.color = Color.white;
    }
    private void ChangeAttackState()
    {
        if (!canAttack)
            _attackTimer -= Time.deltaTime;
        if (_attackTimer < 0.1f)
        {
            canAttack = true;
        }
    }
}
