using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using System.Linq;

public class BuildingSpawner : Building
{
    public float maxBuildProgress;
    private float currentBuildProgress = 0.0f;

    public float spawnDistToCenter;
    public string[] canCreate;
    protected List<string> buildQueue;
    private Vector3 spawnPoint;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        buildQueue = new List<string>();
        maxBuildProgress = 10;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        ProcessBuildQueue();
    }

    public void AskForUnit(int unitId)
    {
        int cost = ResourceManager.GetUnit(canCreate[unitId]).GetComponent<Unit>().cost;
        if (canCreate.Length > unitId && player.money >= cost)
        {
            buildQueue.Add( canCreate[unitId]);
            player.AddMoney(-cost);
        }
    }

    public void AbortUnitCreation(int queueindex)
    {
        if (buildQueue.Count > queueindex)
        {
            int cost = ResourceManager.GetUnit(buildQueue[queueindex]).GetComponent<Unit>().cost;
            buildQueue.RemoveAt(queueindex);
            player.AddMoney(cost);
            if (queueindex == 0) currentBuildProgress = 0;
        }
    }

    protected void ProcessBuildQueue()
    {
        if (buildQueue.Count > 0)
        {
            currentBuildProgress += Time.deltaTime * ResourceManager.BuildSpeed;
            if (currentBuildProgress > maxBuildProgress)
            {
                float angle =  Random.Range(0,20) * (360f / 20);
                Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.right;
                spawnPoint = transform.position + (dir * (spawnDistToCenter));
                if (player) player.AddUnit(buildQueue[0], spawnPoint, transform.rotation);
                buildQueue.RemoveAt(0);
                currentBuildProgress = 0.0f;
            }
        }
    }

    public string[] getBuildQueueValues()
    {
        string[] values = new string[buildQueue.Count];
        int pos = 0;
        foreach (string unit in buildQueue) values[pos++] = unit;
        return values;
    }

    public float getBuildPercentage()
    {
        return currentBuildProgress / maxBuildProgress;
    }

    public override void onGUI()
    {
        GameObject.Find("OrderPanel").GetComponent<OrderPanelScript>().ActualiseBuildDisplay(canCreate, buildQueue.ToArray(), getBuildPercentage(), player) ;
        base.onGUI();
    }

}
