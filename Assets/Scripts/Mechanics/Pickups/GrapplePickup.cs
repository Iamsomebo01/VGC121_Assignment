using UnityEngine;
public class GrapplePickup : Pickup
{
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    public override void OnPlayerPickup(PlayerController interactor)
    {
        Debug.Log("Player picked up Grapple Hook!");

        interactor.UnlockGrapple();

        // Remove pickup after collecting
        Destroy(gameObject);
    }

    void Update()
    {
        // Floating animation
        transform.position = new Vector3(
            transform.position.x,Mathf.Sin(Time.time * 2f) * 0.5f + startPosition.y,transform.position.z);
    }
}
