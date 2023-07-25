using Unity.Netcode;
using UnityEngine;

public abstract class Spell : NetworkBehaviour
{
    public AttributeSpell attribute;

    [ServerRpc]
    public virtual void ActivateSpellServerRpc(Vector3 point)
    {
        Debug.Log("Activate!");
    }
}