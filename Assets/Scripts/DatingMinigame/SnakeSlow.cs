using System.Collections;
using System.Collections.Generic;
using Misc;
using ScriptTags;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SnakeSlow : MonoBehaviour
{
    [Tooltip("Modifier to move speed when bit by a rattlesnake")]
    public float moveSpeedMult;

    public float multDuration;
    
    public AudioSource bite;

    public Animator anim;

    public SnakeMove move;

    public NavMeshAgent agent;

    public bool isStationary;

    public SnakeRotate rot;

    private Vector3 oldPos = Vector3.zero;

    private bool canStrike = true;

    private static LandMovement playerLM;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && canStrike)
        {
            StopAllCoroutines();
            StartCoroutine(SlowCoroutine());
        }
    }

    private IEnumerator SlowCoroutine()
    {
        rot.enabled = false;
        canStrike = false;
        anim.SetBool("Strike", true);
        bite.Play();
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Strike", false);
        if (playerLM == null)
            playerLM = Player.player.GetComponent<LandMovement>();
        playerLM.ChangeMoveSpeedMultForTime(moveSpeedMult, multDuration);
        HUDManager.hudManager.DisplayMessageToHUDForTime("You were bitten by a rattlesnake! Watch out!", 3);
        
        if (!isStationary)
        {
            move.enabled = true;
            move.SetNewDirection();
            yield return new WaitForSeconds(5);
            canStrike = true;
            rot.enabled = true;
        }
        else
        {
            if (oldPos.Equals(Vector3.zero))
                oldPos = transform.position;
            move.enabled = true;
            yield return new WaitForSeconds(10);
            //Debug.Log("Disabling Move");
            move.enabled = false;
            canStrike = true;
            rot.enabled = true;
            yield return new WaitForSeconds(60);
            //Debug.Log("Attempting to return to old position");
            agent.SetDestination(oldPos);
            anim.SetBool("Moving", true);
        }
    }
    
    private void Update()
    {
        if (agent.remainingDistance <= 0.1f)
        {
            anim.SetBool("Moving", false);
        }
    }
}
