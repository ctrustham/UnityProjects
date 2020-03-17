using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask; // the layer targets are on
    public LayerMask obstacleMask; // the layer obstacles are on

    public List<Transform> targetList;

    public float meshResolution; // how many rays the fov casts out
    public int edgeResolution; // how accurately edges are detected (# of iterations for finding a corner/edge in findEddge())
    public float edgeDistThreshold; // 


    public MeshFilter viewMeshFilter;
    Mesh viewMesh;


    private void Start()
    {
        //for creating the mesh that shows what the char can see
        viewMesh = new Mesh();
        viewMesh.name = "view mesh";
        viewMeshFilter.mesh = viewMesh;


       // StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay (float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisableTargets();
        }
    }

    private void LateUpdate() //lateUpdate comes after update, so FoV is drawn after player rotates
    {
        DrawFov();
        FindVisableTargets();
    }

    void FindVisableTargets()
    {
        targetList.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //find all targets within a sphere on the targetMask layer
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            //check if target falls within view angle
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                //check if obstacle is between character and target
                float distToTarget = Vector3.Distance(transform.position, target.position); // directionToTarget

                if (!Physics.Raycast(transform.position, directionToTarget, distToTarget, obstacleMask))
                {
                    targetList.Add(target);
                }
            }
        }
    }

    // to draw the FoV effect
    // cast out rays from character, find collisions and then construct mesh from results
    void DrawFov()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution); // ie. number of rays per degree
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>(); // list of points used to construct mesh
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i; // find the angle that the object (player) is facing, go to 'left' extreme of view angle, then work towards right extreme of view angle one step at a time
            ViewCastInfo newViewCast = ViewCast(angle); // get info on if the raycast hit an object

            if (i > 0)
            {
                if((oldViewCast.hit != newViewCast.hit)
                    || ((oldViewCast.hit == true && newViewCast.hit == true) // if both hit
                        && oldViewCast.obj != newViewCast.obj)) // but did not hit the same object
                {
                    edgeinfo edge = findEdge(oldViewCast, newViewCast);
                    if (edge.ptA != oldViewCast.point)
                    {
                        viewPoints.Add(edge.ptA);
                    }
                    if ( edge.ptB != newViewCast.point)
                    {
                        viewPoints.Add(edge.ptB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);

            oldViewCast = newViewCast;
            
        }
        // create mesh from origin and all viewPoints
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount]; // each vector => the coords of each point of mesh
        int[] triangles = new int[(vertexCount - 2)*3]; // list of triangle vertices
        
        // the view mesh is going to be a child of the character, so all the vertices need to be in 'local space', ie. relative to the character
        vertices[0] = Vector3.zero; // THIS IS INSTEAD OF THE TRANSFORM.POSITION

        
        for (int i = 0; i < vertexCount-1; i++) // already set one
        {
            vertices[i+1] = transform.InverseTransformPoint( viewPoints[i] ); // need to convert 'viewPoints' to local space

            if (i < vertexCount - 2) // stay in bounds of triangle array
            {
                // triangles array will look like -> [0,1,2,0,2,3,0,3,4,..]
                triangles[i * 3] = 0; // the first vertex of each triangle => set to 'origin' (ie. 0)
                triangles[i * 3 + 1] = i + 1; // the second vertex of each triangle
                triangles[i * 3 + 2] = i + 2; // the third vertex of each triangle
            }
        }
        viewMesh.Clear(); // reset viewMesh
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)){
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle, hit.transform);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle, null);
        }

    }

    /* of note: unity uses a unit circle with 0 degrees on the positive y axis, and 90 degrees on the positive x
     * as opposed to the 0 on the positive x and 90 on the positive y.
     * therefore we need to do a conversion of (90-x) to go from the classic trig to unity's scheme
     * since sin(90-x) = cos(x) we will just use cos(x) instead
     */
    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            //make global by addding tranform's own rotation
            angleInDegrees += transform.eulerAngles.y;

        }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), // 
                0,
                Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); // see note
        }
    
    // find the edge of an obstacle for creating a nicer mesh
    public edgeinfo findEdge(ViewCastInfo min, ViewCastInfo max)
    {
        //float minAngle = min.angle;
        //float maxAngle = max.angle;
        //Vector3 minPt = min.point;
        //Vector3 maxPt = max.point;
        
        for (int i = 0; i < edgeResolution; i++)
        {
            float angle = (min.angle + max.angle) / 2;
            ViewCastInfo newVC = ViewCast(angle);
            if ((newVC.hit == min.hit) 
                && min.obj == newVC.obj) // hit the same object
            {
                min = newVC;
                //minAngle = angle;
                //minPt = newVC.point;
            }
            else
            {
                max = newVC;
                //maxAngle = angle;
                //maxPt = newVC.point;
            }
        }
        return new edgeinfo(min.point, max.point);
    }

    // store pair of points where one is on the obj and the other is off, with a minimum of dist between them
    public struct edgeinfo
    {
        public Vector3 ptA, ptB;

        public edgeinfo(Vector3 _ptA, Vector3 _ptB)
        {
            this.ptA = _ptA; // point on obj
            this.ptB = _ptB; // pt off obj
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist, angle;
        public Transform obj;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle, Transform _obj)
        {
            this.hit = _hit;
            this.point = _point;
            this.dist = _dist;
            this.angle = _angle;
            this.obj = _obj;
        }
    }
}
