using System.Collections;
using System.Collections.Generic;
using Audio;
using Misc;
using ScriptTags;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SnakeSlow : MonoBehaviour
{
    [Tooltip("Modifier to move speed when bit by a rattlesnake")]
    public float moveSpeedMult;

    [Tooltip("Duration that the move speed modifier lasts")]
    public float multDuration;
    
    public AudioSource bite;

    public Animator anim;

    public SnakeMove move;

    public NavMeshAgent agent;

    [Tooltip("Whether this is a snake that defaults to stationary")]
    public bool isStationary;

    public SnakeRotate rot;

    [Tooltip("Overlay sprite to pass to the hud for display when bit")]
    public Sprite screenOverlay;

    [Tooltip("Degree by which to quiet background music while playing sound")] public float BGMAttenuation;

    //the snakes last stationary position
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
        //disable rotating and striking
        rot.enabled = false;
        canStrike = false;
        //play the strike
        anim.SetBool("Strike", true);
        bite.Play();
        if (!SoundManager.soundManager.IsMuted(2))
        {
            StartCoroutine(SoundManager.soundManager.QuietBGMUntilDone(bite, BGMAttenuation));
        }
        yield return new WaitForSeconds(0.2f);
        //stop strike animation
        anim.SetBool("Strike", false);
        //reduce player movement speed
        if (playerLM == null)
            playerLM = Player.player.GetComponent<LandMovement>();
        playerLM.ChangeMoveSpeedMultForTime(moveSpeedMult, multDuration);
        //display result to the hud, play sound, unlock achievement
        HUDManager.hudManager.DisplayMessageToHUDForTime("You were bitten by a rattlesnake! Watch out!", 3);
        HUDManager.hudManager.CreateFadingOverlay(screenOverlay, multDuration);
        Player.playerA.PlayAHHH();
        SteamAPIManager.UnlockAch("SnakeBiteAchievement");
        //if this is a moving snake, it runs away
        if (!isStationary)
        {
            move.enabled = true;
            move.SetNewDirection();
            yield return new WaitForSeconds(5);
            canStrike = true;
            rot.enabled = true;
        }
        //if this is a stationary snake, run away for a while, then return to old position
        else
        {
            if (oldPos.Equals(Vector3.zero))
                oldPos = transform.position;
            move.enabled = true;
            yield return new WaitForSeconds(10);
            move.enabled = false;
            canStrike = true;
            rot.enabled = true;
            yield return new WaitForSeconds(60);
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
