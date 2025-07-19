using UnityEngine;
using System.Collections;

public class CowAI : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 1.5f;
    public float waitTime = 5f;
    public float walkRadius = 10f;

    private Vector3 targetPosition;
    private bool isWalking = false;

    void Start()
    {
        StartCoroutine(StateLoop());
    }

    IEnumerator StateLoop()
    {
        while (true)
        {
            // Walk state
            isWalking = true;
            Debug.Log("Cow is Walking");
            animator.SetBool("isWalking", true);
            targetPosition = GetRandomPosition();

            while (Vector3.Distance(transform.position, targetPosition) > 0.5f)
            {
                MoveToward(targetPosition);
                yield return null;
            }

            // Graze state
            isWalking = false;
            animator.SetBool("isWalking", false);
            yield return new WaitForSeconds(waitTime + Random.Range(0f, 3f));
        }
    }

    void MoveToward(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += -direction * walkSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction), Time.deltaTime * 2f);
    }

    Vector3 GetRandomPosition()
    {
        Vector2 circle = Random.insideUnitCircle * walkRadius;
        Vector3 randomTarget = new Vector3(circle.x, 0, circle.y) + transform.position;
        randomTarget.y = transform.position.y; // keep ground level
        return randomTarget;
    }
}
