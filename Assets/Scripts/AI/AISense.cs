using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AISense<Stimulus> : MonoBehaviour
{

    public enum Status
    {
        Enter,
        Stay,
        Leave
    };

    public Transform senseTransform;

    public float updateInterval = 0.3f;
    private float updateTime = 0.0f;

    public bool ShowDebug = true;
    protected List<Transform> trackedObjects = new List<Transform>();
    protected List<Transform> sensedObjects = new List<Transform>();

    public delegate void SenseEventHandler(Stimulus sti, Status sta);
   // private event SenseEventHandler CallSenseEvent;

    private Stimulus stimulus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Gestion du sens, si perception => event
    void Update()
    {
        updateTime += Time.deltaTime;
        if(updateTime > updateInterval)
        {
            resetSense();

            foreach (Transform t in trackedObjects)
            {
                stimulus = default(Stimulus);
                if (doSense(t, ref stimulus))
                {
                    if (!sensedObjects.Contains(t))
                    {
                        sensedObjects.Add(t);
                        senseEvent(stimulus);
                    }
                }
                else
                {
                    sensedObjects.Remove(t);
                }
            }
            updateTime = 0;
        }
    }

    protected abstract bool doSense(Transform obj, ref Stimulus sti);

    protected virtual void senseEvent(Stimulus sti)
    {

    }

    protected virtual void resetSense()
    {

    }

    public void AddObjectToTrack(Transform t)
    {
        trackedObjects.Add(t);
    }

    public void OnDrawGizmos()
    {
        if (!ShowDebug) return;

        Gizmos.color = Color.red;
        foreach( Transform t in sensedObjects)
        {
            Gizmos.DrawLine(senseTransform.position, t.position);
        }
    }

}
