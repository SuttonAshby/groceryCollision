using UnityEngine;
using DG.Tweening;

public class Cart : MonoBehaviour
{
    public Vector3 CartMoveDist;
    public float CartMoveTime;
    public float ItemShrinkFactor;
    public Transform ingredientMovePoint;

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
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

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
                rigidbody.DOMove(CartMoveDist, CartMoveTime)
                    .SetRelative(true)
                    .OnComplete(() => moveState = MoveState.Outside)
                    .OnRewind(() => moveState = MoveState.Inside);
                break;
            case MoveState.MovingIn:
                moveState = MoveState.MovingOut;
                rigidbody.DOFlip();
                break;
        }
    }

    public void Extend()
    {
        switch (moveState)
        {
            case MoveState.Outside:
                moveState = MoveState.MovingIn;
                rigidbody.DOMove(-CartMoveDist, CartMoveTime)
                    .SetRelative(true)
                    .OnComplete(() => {
                        moveState = MoveState.Inside;
                    })
                    .OnRewind(() => moveState = MoveState.Outside);
                break;
            case MoveState.MovingOut:
                moveState = MoveState.MovingIn;
                rigidbody.DOFlip();
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

    public bool CanCollect()
    {
        return moveState == MoveState.Inside;
    }

    public void AddItem(Ingredient ingredient)
    {
        manager.GotItem(this, ingredient.id);

        // var coll = ingredient.GetComponent<Collider>();
        // if (coll != null) Destroy(coll);
        // Destroy(rb);

        var tr = ingredient.transform;
        var destination = ingredientMovePoint.position;
        tr.DOMove(destination, 0.2f);
        tr.DOScale(tr.localScale / ItemShrinkFactor, 0.2f).OnComplete(() =>
        {
            ingredient.gameObject.SetLayerRecursively("CollectedItems");
        });
    }

}
