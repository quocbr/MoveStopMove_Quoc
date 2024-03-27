using UnityEditor;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Collider other;
    public int time;
    public bool cache;

    public Target GetTarget(Collider other)
    {
        if (cache == false)
        {
            return other.GetComponent<Target>();
        }
        return TargetCache.Get(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            Target2 target2 = other.GetComponent<Target2>();
            Target t1 = TargetCache.Get(target2);
            Target t2 = TargetCache.Get(other);
            // ...
        }
    }
}


[CustomEditor(typeof(Testing))]
public class TestingEditor : Editor
{
    Testing t;
    private void OnEnable()
    {
        t = target as Testing;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Run"))
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            for (int i = 0; i < t.time; i++)
            {
                t.GetTarget(t.other);
            }

            watch.Stop();

            Debug.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}