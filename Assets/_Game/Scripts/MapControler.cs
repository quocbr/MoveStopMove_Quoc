using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapControler : MonoBehaviour
{
    [SerializeField] private List<Transform> l_SpawnPos;
    
    [SerializeField] Transform minPoint, maxPoint;

    public List<Transform> L_SpawnPos { get => l_SpawnPos;}

    public Vector3 RandomPoint()
    {
        Vector3 randPoint = Random.Range(minPoint.position.x, maxPoint.position.x) * Vector3.right + Random.Range(minPoint.position.z, maxPoint.position.z) * Vector3.forward;

        NavMeshHit hit;

        NavMesh.SamplePosition(randPoint, out hit, float.PositiveInfinity, 1);

        return hit.position;
    }


}
