using Unity.Netcode;
using UnityEngine;
using TMPro;
using Unity.Collections;

public class PlayerSettings : NetworkBehaviour
{
    public static PlayerSettings Instance { get; private set; }

    [SerializeField] TextMeshProUGUI playerName;

    NetworkVariable<FixedString128Bytes> networkPlayerName = new NetworkVariable<FixedString128Bytes>
        ("Player: null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {    
        networkPlayerName.Value = "Player: " + (OwnerClientId + 1);
        //networkPlayerName.Value = EditPlayerName.Instance.GetPlayerName();
        playerName.text = networkPlayerName.Value.ToString();
    }
}