using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
public class BuildingDrop : MonoBehaviour
{
    public MeshRenderer MR;
    private bool placementPhase;
    private int contacts;

    private void Start()
    {
        MR.material.color = Color.green;

        contacts = 0;
        placementPhase = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (placementPhase)
        {
            Vector3 mousePosition = GetMouseWorldPos();
            if (mousePosition != ResourceManager.InvalidPosition)
                transform.position = mousePosition;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hit,Mathf.Infinity,mask)) return hit.point;
        return ResourceManager.InvalidPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 9 && placementPhase) // =GROUND
        {
            MR.material.color = Color.red;
            contacts ++;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 9  && placementPhase)
        {
            contacts--;
            if (contacts == 0)
            {
                MR.material.color = Color.green;
            }
        }
    }

    public bool Drop()
    {

        if (contacts == 0)
        {
            MR.material.color = Color.white;
            placementPhase = false;
            return true ;
        }
        else return false;
    }
    public bool GetPlacementPhase()
    {
        return placementPhase;
    }
}
