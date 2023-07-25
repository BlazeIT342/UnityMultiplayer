using UnityEngine;
using Unity.Netcode;

public class MeatHook : NetworkBehaviour
{
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform hookStart;
    [SerializeField] private float hookSpeed;
    [SerializeField] private float maxHookDistance;
    [SerializeField] private float hookDuration;
    [SerializeField] private float pullSpeed;

    private Transform hookedPlayer;
    private GameObject hook;
    private bool isHooked;
    private float hookDistance;
    private float hookStartTime;
    private Vector3 hookTargetPosition;

    private void MeatHookAction_performed()
    {
        if (!IsOwner) return;
        if (hook == null)
        {
            // Create the hook object
            hook = Instantiate(hookPrefab, hookStart.position, hookStart.rotation);
            hook.GetComponent<Rigidbody>().velocity = hookStart.forward * hookSpeed;

            // Set the start time and distance of the hook
            hookStartTime = Time.time;
            hookDistance = 0f;
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            MeatHookAction_performed();
        }
        if (hook != null)
        {
            if (hookDistance < maxHookDistance)
            {
                // Update the position of the hook
                hookDistance = (Time.time - hookStartTime) * hookSpeed;
                hook.transform.position = hookStart.position + hookStart.forward * hookDistance;

                // Check for collision with other players
                //Collider[] colliders = Physics.OverlapSphere(hook.transform.position, hook.GetComponent<CapsuleCollider>().radius);
                //foreach (Collider collider in colliders)
                //{
                //    if (collider.gameObject.CompareTag("Player"))
                //    {
                //        // Hooked the player, start pulling
                //        isHooked = true;
                //        hookTargetPosition = collider.gameObject.transform.position;
                //        break;
                //    }
                //}
            }
            else
            {
                // Hook expired, destroy it
                Destroy(hook);
            }

            if (isHooked)
            {
                // Move the hooked player towards the hook position
                Vector3 direction = hookStart.position - hookTargetPosition;
                direction.y = 0f;
                direction.Normalize();
                transform.position += direction * pullSpeed * Time.deltaTime;

                // Check for distance between the player and the hook
                float distance = Vector3.Distance(transform.position, hookTargetPosition);
                if (distance < 1f)
                {
                    // Stop pulling and destroy the hook
                    isHooked = false;
                    Destroy(hook);
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hookedPlayer = collision.transform;
            // Hooked the player, start pulling
            isHooked = true;
            hookTargetPosition = collision.gameObject.transform.position;
        }
    }
}
