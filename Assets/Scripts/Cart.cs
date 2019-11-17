using UnityEngine;
using DG.Tweening;

public class Cart : MonoBehaviour
{
    private Manager manager;
    private bool Moving;
    private enum MoveState
    {
        Outside,
        MovingIn,
        MovingOut,
        Inside
    }
    private MoveState moveState;
    private readonly Vector3 CartMoveDist = new Vector3(0, 0, 7);
    void Start()
    {
        manager = FindObjectOfType<Manager>();
        if (!manager)
        {
            Debug.LogError("no Manager");
        }
        moveState = MoveState.Outside;
    }

    // Behavior 1
    public void Retract()
    {
        switch (moveState)
        {
            case MoveState.Inside:
                moveState = MoveState.MovingOut;
                transform.DOMove(CartMoveDist, 1)
                    .SetRelative(true)
                    .OnComplete(() => moveState = MoveState.Outside)
                    .OnRewind(() => moveState = MoveState.Inside) ;
                break;
            case MoveState.MovingIn:
                moveState = MoveState.MovingOut;
                transform.DOFlip();
                break;
        }
    }

    public void Extend()
    {
        switch (moveState)
        {
            case MoveState.Outside:
                moveState = MoveState.MovingIn;
                transform.DOMove(-CartMoveDist, 1)
                    .SetRelative(true)
                    .OnComplete(() => moveState = MoveState.Inside)
                    .OnRewind(() => moveState = MoveState.Outside);
                break;
            case MoveState.MovingOut:
                moveState = MoveState.MovingIn;
                transform.DOFlip();
                break;
        }
    }

    // Behavior 2
    public void Lockout()
    {
        if (!Moving)
        {
            Moving = true;
            transform.DOMove(-CartMoveDist, 1)
                .SetLoops(2, LoopType.Yoyo)
                .SetRelative(true)
                .OnComplete(() => Moving = false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name;
        manager.GotItem(this, otherName);
        Destroy(other.gameObject);
    }
}
