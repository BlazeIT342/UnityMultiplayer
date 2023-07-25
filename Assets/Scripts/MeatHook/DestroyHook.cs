using Unity.Netcode;

public class DestroyHook : NetworkBehaviour
{
    [ServerRpc]
    public void DestroyObjectServerRpc()
    {
        Destroy(gameObject);
    }
}