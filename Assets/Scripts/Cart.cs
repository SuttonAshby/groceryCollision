using UnityEngine;
using DG.Tweening;

public class Cart : MonoBehaviour
{
    public Collider shotBlocker;
    public Vector3 CartMoveDist;
    public float CartMoveTime;
    public float ItemShrinkFactor;
    public Vector2 cartSize;

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

    public void AddItem(Rigidbody rb)
    {
        if (moveState != MoveState.Inside) return;
        var obj = rb.gameObject;
        var objectID = FindObjectID(obj);
        if (objectID == null) return;

        manager.GotItem(this, objectID.id);
        var coll = rb.GetComponent<Collider>();
        if (coll != null) Destroy(coll);
        Destroy(rb);
        var tr = objectID.transform;
        tr.SetParent(transform);

        var destination = new Vector3(
            (-0.5f + Random.value) * cartSize.x,
            (-0.5f + Random.value) * cartSize.y,
            0f);
        destination += transform.position;
        tr.DOMove(destination, 0.5f);
        tr.DOScale(tr.localScale / ItemShrinkFactor, 0.5f);
    }

    private ObjectID FindObjectID(GameObject obj)
    {
        var objectID = obj.GetComponent<ObjectID>();
        if (objectID == null)
        {
            var parent = obj.transform.parent;
            if (parent != null) objectID = parent.gameObject.GetComponent<ObjectID>();
        }
        if (objectID == null) objectID = obj.GetComponentInChildren<ObjectID>();
        return objectID;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(cartSize.x, 0f, cartSize.y));
    }

}
