using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;


public class AgentController : MonoBehaviour
{
    public bool showDebug = true;

    protected void FindPathTo(Vector3 dest)
    {
        GetComponent<NavMeshAgent>().SetDestination(dest);
    }

    public void Stop()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
    }

    public void OnDrawGizmos()
    {
        if (!showDebug) return;

        NavMeshAgent nVA = GetComponent<NavMeshAgent>();
        float height = nVA.height;
        if (nVA.hasPath)
        {
            Vector3[] corners = nVA.path.corners;
            if(corners.Length >= 2)
            {
                Gizmos.color = Color.red;
                for(int i = 1; i < corners.Length; i++)
                {
                    Gizmos.DrawLine(corners[i - 1] + Vector3.up * height / 2, corners[i] + Vector3.up * height / 2);
                }
            }
        }
    }
}
