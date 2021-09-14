using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [Header("Ground Detection")]
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundLayerMask;

    [Header("Component References")]
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D mainCollider;
    [SerializeField] SpriteRenderer spriteRenderer;

    // INTERNAL
    List<GameObject> groundObjects;
    int groundRaycasts = 3;
    bool grounded;
    bool jumpFlag = false;

    void Update()
    {
        // Updating ground detection
        groundObjects = GetGroundObjects();
        grounded = IsGrounded();

        // Animation parameters
        animator.SetBool("Walking", Input.GetAxisRaw("Horizontal") != 0);
        animator.SetBool("Grounded", grounded);
        spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") < 0;

        // Jump flag
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            jumpFlag = true;
        }
    }

    void FixedUpdate()
    {
        // Applying jumping
        if (jumpFlag)
        {
            jumpFlag = false;
            Jump();
        }

        // Applying velocity
        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rigidbody.velocity.y);

        void Jump()
        {
            grounded = false;
            rigidbody.AddForce(Vector2.up * jumpForce);
        }
    }

    // This method returns true or false, depending on wether or not the player
    // is on top of any GameObject.
    bool IsGrounded ()
    {
        return (groundObjects.Count > 0);
    }

    // This method returns a list of GameObjects that are detected below the player.
    // The amount of raycasts used is determined by "groundRaycasts".
    List<GameObject> GetGroundObjects ()
    {
        // Calculating each raycast's position (X axis)
        List<float> positions = new List<float>();
        for (int i=0; i < groundRaycasts; i++)
        {
            positions.Add ((-mainCollider.bounds.size.x * 0.95f) / 2 +
                          ((mainCollider.bounds.size.x * 0.95f) / (groundRaycasts-1) * i));
        }

        // Detecting objects
        List<GameObject> objects = new List<GameObject>();
        for (int i=0; i < positions.Count; i++)
        {
            // Debugging
            Debug.DrawRay (transform.position + new Vector3(positions[i], (-mainCollider.bounds.size.y * 0.6f) / 2, 0), Vector2.down * groundDistance, Color.blue, 0.1f);
            // Getting current ray (in the loop)
            RaycastHit2D currentRay = Physics2D.Raycast(mainCollider.bounds.center + new Vector3(positions[i], (-mainCollider.bounds.size.y * 0.6f) / 2, 0), Vector2.down, groundDistance, groundLayerMask);
            if (currentRay.collider != null) //Hits an object
            {
                // The list doesn't contain the current object
                if (!objects.Contains(currentRay.collider.gameObject))
                {
                    // Add it to the list
                    objects.Add (currentRay.collider.gameObject);
                }
            }
        }
        return objects;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Checkpoint>())
        {
            // TODO PlayFab Event

            // Disabling checkpoint
            other.gameObject.SetActive(false);
        }
    }
}
