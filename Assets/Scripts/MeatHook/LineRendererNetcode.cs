using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LineRendererNetcode : NetworkBehaviour
{

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private List<Vector3> pointsList = new List<Vector3>();

    [SerializeField] private Color lineColor = Color.white;

    [ServerRpc]
    private void ServerAddPointToListServerRpc(Vector3 point)
    {
        pointsList.Add(point);
        UpdateClientLineRenderer();
    }

    private void UpdateClientLineRenderer()
    {
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.positionCount = pointsList.Count;
        lineRenderer.SetPositions(pointsList.ToArray());
    }

    private void Update()
    {
        if (IsLocalPlayer && Input.GetMouseButton(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ServerAddPointToListServerRpc(worldPos);
        }
    }
}
