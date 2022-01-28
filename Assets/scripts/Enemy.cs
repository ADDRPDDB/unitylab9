using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 PointA;
    public Vector3 PointB;
    public GameObject PlayerObject;
    public CircleCollider2D TriggerArea;
    public float MovementSpeed = 1.0f;
    private int CurrentPoint = 0;
    private bool bFollow = false;
    private Animator EnemyAnimator;
    private float CurrentSpeed;

    private void MoveToCurrentPoint()
    {
        if(transform.position == (CurrentPoint == 0 ? PointA:PointB))
        {
            CurrentPoint = CurrentPoint == 0 ? 1 : 0;
        }
        Vector3 NewPosition = Vector3.MoveTowards(transform.position, CurrentPoint == 0 ? PointA : PointB, MovementSpeed * Time.deltaTime);
        Vector3 NewDirection = transform.position - NewPosition;
        transform.localScale = new Vector3(NewDirection.x > 0 ? -.3f : .3f, .3f, .3f);
        CurrentSpeed = (transform.position - NewPosition).magnitude;
        transform.position = NewPosition;
    }

    private void MoveToPlayer()
    {
        if(PlayerObject)
        {
            Vector3 NewPosition = Vector3.MoveTowards(transform.position, PlayerObject.transform.position, MovementSpeed * Time.deltaTime);
            Vector3 NewDirection = transform.position - NewPosition;
            transform.localScale = new Vector3(NewDirection.x > 0 ? -.3f : .3f, .3f, .3f);
            CurrentSpeed = (transform.position - NewPosition).magnitude;
            transform.position = NewPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentSpeed = 0;
        if(TriggerArea)
        {
            List<Collider2D> Result = new List<Collider2D>();
            ContactFilter2D contactFilter = new ContactFilter2D();
            TriggerArea.OverlapCollider(contactFilter, Result);
            if (Result.Find(x => x.gameObject.GetComponent<Player>()))
            {
                bFollow = true;
            }
            else
                bFollow = false;
        }

        if (bFollow)
            MoveToPlayer();
        else
            MoveToCurrentPoint();

        if(EnemyAnimator)
        {
            EnemyAnimator.SetFloat("Speed", CurrentSpeed);
        }    
    }
}
