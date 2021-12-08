using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerController
{
    public float ActionTimer = 1f;
    private Vector3 direction;
    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _timer = ActionTimer;
    } 

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0f)
        {
            direction = SelectRandomDirection();
            StartCoroutine(MoveToPoint(direction));
            
            _timer = ActionTimer;
        }
    }

    private Vector3 SelectRandomDirection()
    {
        DisableAnimationBools(animator);
        int RandDirection = Random.Range(0, 4);
        switch (RandDirection) {
            case 0:
                animator.SetBool("MoveRight", true);
                direction = transform.right + RightMoveVector;
                break;
            case 1:
                animator.SetBool("MoveLeft", true);
                direction = (transform.right + RightMoveVector) * -1;
                break;
            case 2:
                animator.SetBool("MoveUp", true);
                direction = Vector3.up + UpMoveVector;
                break;
            case 3:
                animator.SetBool("MoveDown", true);
                direction = Vector3.down - UpMoveVector;
                break;
            default:
                break;
        }
        return direction;
    }




}
