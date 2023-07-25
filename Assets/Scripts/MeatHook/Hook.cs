using Unity.Netcode;
using UnityEngine;

public class Hook : NetworkBehaviour
{
    public HookSpell parent;
    bool active;
    float speed = 20f;
    public GameObject target;
    public LineRenderer lineRenderer;
    public Vector3 startPosition;
    public Vector3 endDirection;
    public Vector3 endPosition;
    bool end;
    private RaycastHit raycastHit;

    public override void OnNetworkSpawn()
    {
        //lineRenderer = GetComponentInParent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        startPosition = transform.position + new Vector3(0, 0, 2);
        endDirection = (new Vector3(raycastHit.point.x, 0f, raycastHit.point.z) - startPosition).normalized;
        endPosition = startPosition + (endDirection * 20);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        transform.LookAt(endPosition);
        active = true;
    }

    public void Initialize(float range, Vector3 startPosition, Vector3 endPoint)
    {
        this.startPosition = startPosition;

        endDirection = (endPoint - startPosition).normalized;
        endPosition = startPosition + (endDirection * range);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        transform.LookAt(endPosition);
        active = true;
    }

    private void Update()
    {
        if (!IsOwner) return;
        MoveHook();
    }

    private void MoveHook()
    {
        if (active)
        {
            if (Vector3.Distance(transform.position, endPosition) <= 1f)
            {
                if (end)
                {
                    //GetComponentInParent<DestroyHook>().DestroyObjectServerRpc();
                    parent.DestroyHookServerRpc();
                }

                endPosition = startPosition;
                endDirection = (startPosition - transform.position).normalized;
                end = true;
            }
            else
            {
                transform.position += endDirection * speed * Time.deltaTime;

                if (end && target != null)
                {
                    target.transform.position = transform.position;
                }
                if (!end)
                {
                    transform.Rotate(new Vector3(0, 0, 15f));
                }
            }
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!IsOwner) return;
        if(collision.gameObject.CompareTag("Player") && !end)
        {
            target = collision.gameObject;
            endPosition = startPosition;
            endDirection = (startPosition - transform.position).normalized; 
            end = true;
        }
    }
}