using System;
using UnityEngine;
using Unity.Netcode;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private int startingHealth = 100;   // The starting health of the object

    [SyncVar]
    private int currentHealth;          // The current health of the object, synced across the network

    // Called when the object is first created
    public override void OnNetworkSpawn()
    {
        currentHealth = startingHealth; // Set the initial health
    }

    // Apply damage to the object
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int amount)
    {
        currentHealth -= amount;        // Reduce the current health by the damage amount

        // Check if the object is dead
        if (currentHealth <= 0)
        {
            DieServerRpc();
        }
    }

    // Called on the client when the object's health changes
    [ClientRpc]
    private void UpdateHealthClientRpc(int newHealth)
    {
        currentHealth = newHealth;
    }

    // Called when the object dies
    [ServerRpc]
    private void DieServerRpc()
    {
        // Implement death behavior here, such as destroying the object or playing an animation
        Debug.Log("Object has died!");

        // If the object is destroyed, notify clients
        NetworkObject.Destroy(gameObject);
    }
}