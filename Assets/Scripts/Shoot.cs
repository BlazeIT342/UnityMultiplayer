using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shoot : NetworkBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPosition;

    [SerializeField] List<GameObject> shootList = new List<GameObject>();

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
        shootList.Add(bullet);
        bullet.GetComponent<Bullet>().parent = this;
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyShootServerRpc()
    {
        GameObject toDestroy = shootList[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        shootList.Remove(toDestroy);
        Destroy(toDestroy);
    }
}