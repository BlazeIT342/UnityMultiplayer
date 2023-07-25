using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CastSystem : NetworkBehaviour
{
    [Header("Status")]
    public bool Cast = false;
    public int spellID = 0;

    [Header("Reference")]
    public List<Spell> Spells;
    public GameObject selectCircle = null;


    private Ray p_ray;
    private RaycastHit p_hit;
    private Camera p_camera = null;

    private void Start()
    {
        p_camera = Camera.main;
        selectCircle.SetActive(false);
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKey(KeyCode.Q))
        {
            spellID = 0;
            Cast = true;
        }
        if (Cast)
        {
            if (RayMousePosition("Ground"))
            {
                selectCircle.transform.position = new Vector3(p_hit.point.x, p_hit.point.y + 0.5f, p_hit.point.z);
                selectCircle.SetActive(true);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Cast = false;
                selectCircle.SetActive(false);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Spells[spellID].ActivateSpellServerRpc(p_hit.point);
                Cast = false;
            }
        }
        else
        {
            selectCircle.SetActive(false);
        }

    }

    private bool RayMousePosition(string tag)
    {
        p_ray = p_camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(p_ray, out p_hit) && p_hit.collider.CompareTag(tag))
        {
            return true;
        }
        return false;
    }
}
