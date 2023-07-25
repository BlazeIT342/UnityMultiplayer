using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class HookSpell : NetworkBehaviour
{
    [SerializeField] float distance;
    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject hookPrefab;
    [SerializeField] List<GameObject> hookList = new List<GameObject>();

    private Ray ray;
    private RaycastHit raycastHit;

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Q) && GetMouseRay())
        {
            ActivateHookServerRpc(raycastHit.point);
        }
    }

    [ServerRpc]
    public void ActivateHookServerRpc(Vector3 point)
    {
        GameObject hook = Instantiate(hookPrefab, shootPoint.position, transform.rotation);
        hookList.Add(hook);
        
        //hook.GetComponent<Hook>().Initialize(distance, shootPoint.position, new Vector3(point.x, 0f, point.z));
        hook.GetComponent<Hook>().parent = this;
        hook.GetComponent<NetworkObject>().Spawn();

    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyHookServerRpc()
    {
        GameObject toDestroy = hookList[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        hookList.Remove(toDestroy);
        Destroy(toDestroy);      
    }

    private bool GetMouseRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out raycastHit);
    }
}