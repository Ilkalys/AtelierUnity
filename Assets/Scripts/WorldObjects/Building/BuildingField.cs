using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingField : Building
{
    public float speedCultivate;
    public int gainPerCultivation;
    public float timer, maxtimer;

    protected override void Start()
    {
        timer = 0;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void onGUI()
    {
        base.onGUI();
    }

    private void OnTriggerStay(Collider other)
    {
            Unit col = other.gameObject.transform.root.GetComponent<Unit>();
            if (col && col is BuilderUnit && col.player == this.player && col.target == this.gameObject)
            {
                timer += speedCultivate * Time.deltaTime;
                if (timer >= maxtimer)
                {
                    timer = 0;
                    col.player.AddMoney(gainPerCultivation);
                }
            }
    }

}
