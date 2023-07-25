using UnityEngine;
using Unity.Netcode;

public class CameraSpawner : NetworkBehaviour
{
    public GameObject cameraPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Get the player game object for this client
            GameObject playerObject = gameObject;

            // Create the camera game object and attach it to the player
            GameObject cameraObject = Instantiate(cameraPrefab, playerObject.transform);
            cameraObject.transform.localPosition = new Vector3(0f, 1.5f, -2f);
            cameraObject.transform.localRotation = Quaternion.Euler(new Vector3(15f, 0f, 0f));

            // Set the camera target to the player game object
            //cameraObject.GetComponent<CameraController>().target = playerObject.transform;
        }
    }
}
