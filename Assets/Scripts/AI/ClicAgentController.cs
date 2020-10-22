using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicAgentController : AgentController
{
    private Ray rayPickPos;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rayPickPos = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rh;
            if(Physics.Raycast(rayPickPos, out rh))
            {
                FindPathTo(rh.point);
            }
        }
    }

    public new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayPickPos.origin, rayPickPos.direction * 100);
    }

}
