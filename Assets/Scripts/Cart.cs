using UnityEngine;
using DG.Tweening;

public class Cart : MonoBehaviour
{
    public Collider shotBlocker;
    public Vector3 CartMoveDist;
    public float CartMoveTime;
    public float ItemShrinkFactor;

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
                transform.DOMove(CartMoveDist, CartMoveTime)
                    .SetRelative(true)
                    .OnStart(() =>
                    {
                        shotBlocker.enabled = true;
                    })
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
                transform.DOMove(-CartMoveDist, CartMoveTime)
                    .SetRelative(true)
                    .OnComplete(() => {
                        shotBlocker.enabled = false;
                        moveState = MoveState.Inside;
                    })
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
            transform.DOMove(-CartMoveDist, CartMoveTime)
                .SetLoops(2, LoopType.Yoyo)
                .SetRelative(true)
                .OnComplete(() => Moving = false);
        }
    }

    public void AddItem(GameObject go)
    {
        if (moveState == MoveState.Inside)
        {
            manager.GotItem(this, go.name);
            go.transform.localScale = go.transform.localScale / ItemShrinkFactor;
        }
    }
}
