using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Werewolf : MonoBehaviour
{
    public enum WerewolfState
    {
        idle,       // 일반, 몬스터 기본 상태
        hungry,     // 공복, 허기 0 이하로 내려간 상태
        weak,       // 약화, 몬스터 공격이 가능한 상태
        dead        // 몬스터 사망
    }

    private GameObject[] wayPoints;
    private int targetPoint;
    private int TargetPoint
    {
        set { if (value >= wayPoints.Length) this.targetPoint = 0; else this.targetPoint = value; }
        get { return this.targetPoint; }
    }
    private Transform targetTransform;

    [Header("Werewolf Speed")]
    public float defaultMoveSpeed;      // 배회 프로세스 시 몬스터 이동 속도
    public float chasingMoveSpeed;      // 추적 프로세스 시 몬스터 이동 속도


    [Header("Werewolf State")]
    public float maxHp;                 // 최대 체력
    public float maxHunger;             // 최대 허기
    public float hp = 100;              // 체력
    public float hunger = 100;          // 허기

    private WerewolfState state;        // 현재 몬스터 상태

    [SerializeField]
    private bool isChasingPlayer;       // 플레이어 추적 프로세스면 true, 배회 프로세스면(default) false
    private bool isFindingMeat;         // 독고기 향해 이동 중이면 true
    

    private GameObject chasingChild;    // 현재 추적중인 플레이어 오브젝트


    [Header("State Durations")]
    public int weakStateDuration;       // 약화 상태 지속시간 (초) 
    public int playerCheckDelay;        // 추적 모드에서 시야 내 플레이어 확인 딜레이 시간(초)


    [Header("Skill Detail Values")]
    public int attackDelay;             // 덮치기 쿨타임
    public int smellDelay;              // 냄새 추적 쿨타임
    public int attackDamage;            // 덮치기 피해량
    public int attackHungerConsumption; // 덮치기 소모 허기
    public int smellHungerConsumption;  // 냄새 추적 소모 허기

    /* 스킬 사용 가능 여부 체크 */
    private bool isReadyToAttack;       // 덮치기 사용 가능 여부
    private bool isReadyToSmell;        // 냄새 추적 사용 가능 여부


    private Animator animator;
    private VisionChecker vision;

    [Header("UI")]
    public Slider hungerSlider;


    /* Coroutines */
    IEnumerator chaseMode;

    private void Start()
    {
        chaseMode = ChasePlayer();

        animator = GetComponent<Animator>();
        vision = GetComponentInChildren<VisionChecker>();

        wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");

        SetTargetTransform(wayPoints[(TargetPoint = 0)].transform);

        isChasingPlayer = false;
        isFindingMeat = false;

        isReadyToAttack = true;
        isReadyToSmell = true;

        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = hunger;
    }

    private void Update()
    {
        // 배회 프로세스, 지정된 경로를 따라 이동
        if (!isChasingPlayer && !Move())
                SetTargetTransform(wayPoints[++TargetPoint].transform);

        // 추적 프로세스는 ChasePlayer 코루틴으로 실행됨

        hungerSlider.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(0, 60, 0);
    }

    private void SetTargetTransform(Transform target)
    {
        targetTransform = target;
        transform.LookAt(targetTransform);
    }

    private bool Move()
    {
        if (targetTransform == null)
        {
            Debug.Log("move Target is NULL");
            return false;
        }

        if (Vector3.SqrMagnitude(transform.position - targetTransform.position) > 0.1f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * defaultMoveSpeed);
            return true;
        }
        else return false;
    }

    // 추적 모드 설정 혹은 해제
    public void SetChasingMode(bool isChasing)
    {
        // 원래 추적 모드가 아니었으나 추적 모드로 들어갈 시에만 코루틴 실행
        if (!isChasingPlayer && isChasing)
        {
            isChasingPlayer = isChasing;
            StartCoroutine(chaseMode);
        }

        isChasingPlayer = isChasing;

        Debug.Log("추적 모드 " + isChasing);

        if (!isChasingPlayer)
        {
            StopCoroutine(chaseMode);

            SetTargetTransform(wayPoints[TargetPoint].transform);
        }
    }

    public void SetChasingChild (GameObject chasingChild)
    {
        this.chasingChild = chasingChild;
    }

    // hunger 값만큼 허기 감소
    public void MakeHungry(int hunger)
    {
        if ((this.hunger -= hunger) <= 0)
        {
            state = WerewolfState.hungry;
            StartCoroutine(FindMeat());
        }

        hungerSlider.value = this.hunger;
    }

    // 허기 모두 회복 및 일반 상태로 변화
    public void MakeFull()
    {
        hunger = maxHunger;
        state = WerewolfState.idle;
        hungerSlider.value = this.hunger;
    }

    private IEnumerator ChasePlayer()
    {
        Debug.Log("추적 모드 시작");

        while (isChasingPlayer && state != WerewolfState.weak && !isFindingMeat)
        {
            // 공복 상태이고, 시야 내에 독고기 있는지 확인
            if (state == WerewolfState.hungry && vision.meat[0] != null)
            {
                StartCoroutine(FindMeat());
                SetChasingMode(false);
            }
            // 시야 내 추적 중인 캐릭터가 있는지 확인
            else if (chasingChild != null)
            {
                SetTargetTransform(chasingChild.transform);
                Move();

                // 덮치기 실행 가능한지 확인
                if (isReadyToAttack)
                {
                    // 덮치기 실행
                    StartCoroutine(Attack());
                }
            }
            // 시야 내 추적 캐릭터 벗어났지만 냄새 추적 사용 가능 시
            else if (isReadyToSmell)
            {
                Debug.Log("Chasing child is null");

                // 냄새 추적 실행
                StartCoroutine(Smell());
            }
            else SetChasingMode(false);

            yield return null;
        }
    }

    private IEnumerator FindMeat()
    {
        isFindingMeat = true;

        // 있으면 독고기 쪽으로 이동
        SetTargetTransform(vision.meat[0].transform);

        while (isFindingMeat && Move())
        {
            yield return null;
        }

        // 도착하면 섭취 --> 독고기 비활성화
        targetTransform.gameObject.SetActive(false);

        state = WerewolfState.weak;

        // 약화 상태 돌입
        StartCoroutine(MakeWeak());
    }

    private IEnumerator MakeWeak()
    {
        float dt = 0;

        while (dt < weakStateDuration && state == WerewolfState.weak)
        {
            dt += Time.deltaTime;
            yield return null;
        }

        MakeFull();
        SetChasingMode(false);
    }


    /* 스킬 메소드 */

    // 덮치기
    private IEnumerator Attack()
    {
        isReadyToAttack = false;

        MakeHungry(attackHungerConsumption);

        // 덮치기 스킬 발동
        Debug.Log("덮치기 발동");

        yield return new WaitForSeconds(attackDelay);

        isReadyToAttack = true;
    }

    // 냄새 추적
    private IEnumerator Smell()
    {
        isReadyToSmell = false;

        MakeHungry(smellHungerConsumption);

        // 냄새 추적 스킬 발동
        Debug.Log("냄새 추적 발동");

        vision.SetVisionExpanded(true);

        yield return new WaitForSeconds(smellDelay);

        vision.SetVisionExpanded(false);

        isReadyToSmell = true;

        Debug.Log("냄새 추적 해제");
    }
}
