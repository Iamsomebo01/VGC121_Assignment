
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(LineRenderer), typeof(DistanceJoint2D))]
public class GrappleHook : MonoBehaviour
{
    Rigidbody2D rb;
    LineRenderer lr;
    DistanceJoint2D dj;

    public LayerMask grappleLayer;
    public bool isGrappling = false;
    public bool hasGrapple = false;

    public Transform grapplePoint;

    [Header("Grapple Settings")]
    public float grappleSpeed = 8f;
    public float minDistance = 1f;

    Vector3 point;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        dj = GetComponent<DistanceJoint2D>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        lr.positionCount = 2;
        lr.enabled = false;

        dj.enabled = false;
        dj.autoConfigureDistance = false;
        dj.autoConfigureConnectedAnchor = false;

        dj.anchor = transform.InverseTransformPoint(grapplePoint.position);
    }

    void Update()
    {
        if (!hasGrapple)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;

            point = Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D hit = Physics2D.OverlapPoint(point, grappleLayer);

            if (hit != null)
            {
                dj.connectedAnchor = point;

                dj.distance = Mathf.Max(
                    Vector2.Distance(grapplePoint.position, point),
                    minDistance
                );

                dj.enabled = true;
                lr.enabled = true;

                isGrappling = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }

        if (isGrappling)
        {
            lr.SetPosition(0, grapplePoint.position);
            lr.SetPosition(1, point);
        }
    }

    void StopGrapple()
    {
        dj.enabled = false;
        lr.enabled = false;

        isGrappling = false;
    }

    public void UnlockGrapple()
    {
        hasGrapple = true;
        Debug.Log("Grapple unlocked!");
    }
}
