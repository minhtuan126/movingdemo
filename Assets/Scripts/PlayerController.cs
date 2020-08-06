using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public Animator animator;

    public float moveSpeed;
    private int desiredLane = 1;
    public float distanceLane = 4;
    public float jumpForce;
    public float gravity = -20;
    public float maxSpeed;
    private bool isSliding = false;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public static int numberOfCoins;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        if (moveSpeed < maxSpeed)
            moveSpeed += 0.1f * Time.deltaTime;

        animator.SetBool("isGameStarted", true);

        direction.z = moveSpeed;

        //isGrounded = Physics.CheckSphere(groundCheck.position, 5.15f, groundLayer);
        isGrounded = true;
        animator.SetBool("isGrounded", isGrounded);

        if (character.isGrounded)
        {
            direction.y = -1;
            if (SwipeManager.swipeUp)
            {
                direction.y = jumpForce;
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
        }

        if (SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * distanceLane;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * distanceLane;
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;

        if(moveDir.sqrMagnitude < diff.sqrMagnitude)
            character.Move(moveDir);
        else
            character.Move(diff);
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        character.Move(direction * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<SoundManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        character.center = new Vector3(0, 0.5f, 0);
        character.height = 1;

        yield return new WaitForSeconds(1.3f);

        animator.SetBool("isSliding", false);
        character.center = new Vector3(0, 0, 0);
        character.height = 2;
        isSliding = false;

    }
}
