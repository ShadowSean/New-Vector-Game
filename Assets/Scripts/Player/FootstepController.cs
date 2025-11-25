using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepController : MonoBehaviour
{
    [Header("Footstep Settings")]
    public float footstepDistance = 1.5f;     // Distance between steps
    public AudioSource footstepSource;        // Audio Source on the player
    public AudioClip footstepClip;            // Footstep sound

    private float distanceTravelled = 0f;
    private Vector3 lastPos;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (footstepSource == null)
            footstepSource = gameObject.AddComponent<AudioSource>();

        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;

        // How far we moved this frame (ignoring vertical motion)
        Vector3 horizontalMove = new Vector3(currentPos.x - lastPos.x, 0, currentPos.z - lastPos.z);
        float distanceCurrentFrame = horizontalMove.magnitude;

        // Only add distance while grounded and moving
        if (controller.isGrounded)
            Debug.Log("Grounded: " + controller.isGrounded);

            distanceTravelled += distanceCurrentFrame;

        // Check if it's time to play a footstep
        if (distanceTravelled >= footstepDistance && IsMoving() && controller.isGrounded)
        {
            Debug.Log("Footstep Triggered");
            footstepSource.PlayOneShot(footstepClip);
            Debug.Log("Audio playing: " + footstepSource.isPlaying);
            distanceTravelled = 0f;
        }

        lastPos = currentPos;
    }

    // Check if the player is actually moving by velocity, not input
    bool IsMoving()
    {
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        return horizontalVelocity.magnitude > 0.1f;
    }
}
