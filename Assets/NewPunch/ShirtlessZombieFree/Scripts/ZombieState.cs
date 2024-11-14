using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ZombieState : MonoBehaviour
{
    public enum ActorState
    {
        Idle,
        Eat,
        Attack,
        Walk,
        Climb,
        Die
    }

    public ActorState actorState = ActorState.Idle;
    public int health = 3;
    public Transform player;
    public GameObject ZombieCount;

    public Material litMatierial;
    public Material unlitMatierial;

    private Animator animator;
    private float aniOffset;
    private NavMeshAgent navAgent;

    private int zombieCount = 31;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        AniUpdate();
    }

    public void StateUpdate()
    {
        switch (health)
        {
            case 0:
                actorState = ActorState.Die;
                zombieCount--;
                ZombieCount.GetComponent<TMP_Text>().text = zombieCount.ToString();
                AniUpdate();
                break;
            case 1:
                EyeGlowChange();
                break;
            case 2:
                if (actorState == ActorState.Idle)
                    actorState = ActorState.Walk;
                else if (actorState == ActorState.Eat) actorState = ActorState.Climb;
                NavToActor();
                AniUpdate();
                break;
        }
    }

    private void AniUpdate()
    {
        aniOffset = Random.Range(0f, 1f);

        switch (actorState)
        {
            case ActorState.Idle:
                var idleState = Random.Range(1, 4);
                animator.SetInteger("RandomIdle", idleState);
                break;
            case ActorState.Eat:
                animator.Play("Eat", 0, aniOffset);
                break;
            case ActorState.Walk:
                animator.Play("Walking", 0, aniOffset);
                break;
            case ActorState.Climb:
                animator.Play("Climb", 0, aniOffset);
                navAgent.speed = 0.5f;
                break;
            case ActorState.Attack:
                animator.Play("Attack", 0, aniOffset);
                break;
            case ActorState.Die:
                Destroy(gameObject);
                break;
        }
    }

    private void EyeGlowChange()
    {
        var targetChild = transform.Find("V3/Body_LODs/LOD_0/Body_LOD0");
        var renderer = targetChild.GetComponent<Renderer>();
        renderer.material = litMatierial;
    }

    private void NavToActor()
    {
        if (!player) return;
        navAgent.SetDestination(player.position);
    }
}