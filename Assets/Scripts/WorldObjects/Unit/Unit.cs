using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Unit : WorldObject
{
    public GameObject target;
    protected Animator animator;
    protected NavMeshAgent NAM;
    protected override void Awake()
    {
        this.animator = this.GetComponentInChildren<Animator>();
        this.NAM = this.GetComponent<NavMeshAgent>();
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        Animate();
        base.Update();
    }

    protected override void Animate()
    {
        animator.SetFloat("velocity", NAM.velocity.magnitude);
    }

    public override void RightMouseClick(GameObject hitObject, Vector3 hitpoint, Player controller, int index)
    {
        GroupMovement(hitpoint, controller, index);
        base.RightMouseClick(hitObject, hitpoint, controller, index);
    }

    public override void onGUI()
    {
        base.onGUI();
    }

    protected void GroupMovement(Vector3 hitpoint, Player controller, int index)
    {
        int tmp = -1;
        for(int i = 0; i < ResourceManager.UnitPositionPerRing.Length; i++)
        {
            if (tmp == -1 && index < ResourceManager.UnitPositionPerRing[i]) tmp = i;
        }
        int unitInIndexRing = (tmp == 0 ? 1 : (-ResourceManager.UnitPositionPerRing[tmp - 1] + Mathf.Min(ResourceManager.UnitPositionPerRing[tmp],controller.SelectedObjects.Count)) );
        float angle = (ResourceManager.UnitPositionPerRing[tmp] - index) * (360f/ unitInIndexRing);
        Vector3 dir = Quaternion.Euler(0,angle, 0) * Vector3.right; //Quaternion.Ag(0,angle,0)*new Vector3(1,0);
        Vector3 result = hitpoint + (dir * (tmp * 2));
        FindPathTo(result);
    }

    protected void FindPathTo(Vector3 dest)
    {
        NAM.SetDestination(dest);
    }

    public void Stop()
    {
        NAM.isStopped = true;
    }
}
