using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public Shoot parent;
    [SerializeField] float speed = 10f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = rb.transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;
        parent.DestroyShootServerRpc();
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamageServerRpc(10);
        }
    }
}