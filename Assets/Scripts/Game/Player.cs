using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dev.Krk.MemoryDraw.Game;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Showable))]
public class Player : MonoBehaviour
{
    public delegate void MovedAction();
    public event MovedAction OnMoved;

    public delegate void DiedAction();
    public event DiedAction OnDied;

    enum StateEnum
    {
        Idle = 0,
        Moving
    }

    [SerializeField]
    private float moveDuration = 0.5f;

    private Queue<Vector2> moves;

    private StateEnum state;

    private CircleCollider2D circleCollider;

    [SerializeField]
    private Showable showable;

    private float showInterval;

    private float hideInterval;

    [SerializeField]
    private Animator playerAnimator;

    private Vector3 target;

    private float velocity;

    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float errorMargin;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        moves = new Queue<Vector2>(2);
    }

    public void Init(Vector2 position, float showInterval, float hideInterval)
    {
        state = StateEnum.Idle;

        target = position;
        transform.position = position;
        transform.localScale = Vector3.one;

        this.showInterval = showInterval;
        this.hideInterval = hideInterval;

        showable.ShowDelay = (position.x / Field.SIZE + position.y / Field.SIZE) * showInterval;
        showable.HideDelay = (position.x / Field.SIZE + position.y / Field.SIZE) * hideInterval;

        circleCollider.enabled = true;
    }

    public bool CanMove()
    {
        return state != StateEnum.Moving;
    }

    void Update()
    {
        Vector3 diff = target - transform.position;
        float distance = diff.magnitude;

        if (distance > errorMargin)
        {
            velocity += acceleration;
            velocity = Mathf.Min(velocity, maxVelocity);

            if (distance > velocity)
            {
                transform.position += diff.normalized * velocity;
            }
            else
            {
                transform.position = target;
                state = StateEnum.Idle;
                if (OnMoved != null)
                    OnMoved();
            }
        }
        else
        {
            velocity = 0f;
        }

        playerAnimator.SetFloat("velocity", velocity);
    }

    public void Move(Vector3 vector)
    {
        if (state != StateEnum.Moving)
        {
            state = StateEnum.Moving;

            target = transform.position + vector;
            ChangeDirection(vector);

            showable.ShowDelay = (target.x / Field.SIZE + target.y / Field.SIZE) * showInterval;
            showable.HideDelay = (target.x / Field.SIZE + target.y / Field.SIZE) * hideInterval;
        }
    }

    private void ChangeDirection(Vector2 vector)
    {
        float angle = Mathf.Atan2(vector.y, vector.x);

        while (angle < 0)
        {
            angle += Mathf.PI * 2f;
        }

        while (angle > Mathf.PI * 2f)
        {
            angle -= Mathf.PI * 2f;
        }

        if (Mathf.Abs(angle) < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Mathf.Abs(angle - Mathf.PI * 0.5f) < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Mathf.Abs(angle - Mathf.PI) < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Mathf.Abs(angle - Mathf.PI * 1.5f) < 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    public void Show()
    {
        showable.Show();
    }

    public void Hide()
    {
        showable.Hide();
    }
}
