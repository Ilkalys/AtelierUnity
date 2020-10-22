using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderUnit : Unit
{
    public string[] canCreate;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Animate()
    {
        animator.SetBool("Farm", false);
        animator.SetBool("Build", false);

        if (this.target)
        {
            WorldObject wO = this.target.GetComponent<WorldObject>();
            InConstructionBuilding ICB = this.target.GetComponent<InConstructionBuilding>();
            if (wO && wO.player == this.player)
            {
                if (ICB && !ICB.isConstruct && NAM.remainingDistance < 1)
                {
                    animator.SetBool("Build", true);
                }
                else
                {
                    if (wO is BuildingField)
                    {
                        animator.SetBool("Farm", true);

                    }
                }
            }
        }
        base.Animate();
    }

        public void AskForBuild(int unitId)
    {
        if (player && player.money >= ResourceManager.GetBuilding(canCreate[unitId]).GetComponent<Building>().cost)
        {
            GameObject building = player.AddBuilding(canCreate[unitId]);
        }
    }

    public override void RightMouseClick(GameObject hitObject, Vector3 hitpoint, Player controller, int index)
    {
        if (hitObject)
        {
            InConstructionBuilding hit = hitObject.GetComponent<InConstructionBuilding>();
            if (hit && controller == this.player)
            {
                this.target = hitObject;
                FindPathTo(hitpoint);
            }
            else { 
                base.RightMouseClick(hitObject, hitpoint, controller, index);
                this.target = null;
            }
        }
        
    }

    public override void onGUI()
    {
        GameObject.Find("OrderPanel").GetComponent<OrderPanelScript>().ActualiseUnitDisplay(canCreate, player);
        base.onGUI();
    }


}
