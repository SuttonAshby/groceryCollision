using UnityEngine;
using DG.Tweening;

public class Cart : MonoBehaviour
{
    public Vector3 CartMoveDist;
    public float CartMoveTime;
    public float ItemShrinkFactor;
    public Transform ingredientMovePoint;
    public float rejectForce;
    public Vector3 rejectVector;

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

    public bool AddItem(Ingredient ingredient)
    {
        if (manager.GotItem(this, ingredient.id))
        {
            var tr = ingredient.transform;
            var destination = ingredientMovePoint.position;
            tr.DOMove(destination, 0.2f);
            tr.DOScale(ingredient.DefaultScale / ItemShrinkFactor, 0.2f).OnComplete(() =>
            {
                ingredient.gameObject.SetLayerRecursively("CollectedItems");
            });
            return true;
        }
        else
        {
            var rb = ingredient.GetComponent<Rigidbody>();
            var dir = rejectVector;
            var lateralDir = Vector3.Cross(dir, Vector3.back);
            dir = Quaternion.AngleAxis(Random.Range(-45f, 45f), lateralDir) * dir;
            var verticalDir = Vector3.Cross(dir, lateralDir);
            dir = Quaternion.AngleAxis(Random.Range(-20f, 20f), verticalDir) * dir;
            rb.AddForce(dir * rejectForce, ForceMode.Impulse);
            return false;
        }
    }

}
