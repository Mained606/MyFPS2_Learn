using UnityEngine;
using Unity.FPS.Game;


namespace Unity.FPS.AI
{
    /// <summary>
    /// Enemy 상태
    /// </summary>
    public enum AIState
    {
        Patrol,
        Follow,
        Attack
    }

    /// <summary>
    /// 이동하는 Enemy의 상태들을 구현하는 클래스
    /// </summary>
    public class EnemyMobile : MonoBehaviour
    {
        #region Variables
        public Animator animator;
        private EnemyController enemyController;

        public AIState AiState { get; private set; }

        // 이동
        public AudioClip movementSound;
        public MinMaxFloat pitchMovementSpeed; 

        private AudioSource audioSource;

        // 데미지 - 이펙트
        public ParticleSystem[] randomHitSparks;

        // animation parameter
        const string k_AnimAttackParameter = "Attack";
        const string k_AnimMoveSpeedParameter = "MoveSpeed";
        const string k_AnimAlertedParameter = "Alerted";
        const string k_AnimOnDamagedParameter = "OnDamaged";
        const string k_AnimDeathParameter = "Death";
        #endregion

        private void Start()
        {
            // 참조
            enemyController = GetComponent<EnemyController>();
            enemyController.Damaged += OnDamaged;
            
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = movementSound;
            audioSource.Play();

            // 초기화
            AiState = AIState.Patrol;
        }

        void Update()
        {
            // 상태 구현
            UpdateCurrentAiState();

            //속도에 따른 애니 / 사운드 효과
            float moveSpeed = enemyController.Agent.velocity.magnitude;
            animator.SetFloat(k_AnimMoveSpeedParameter, moveSpeed);
            audioSource.pitch = pitchMovementSpeed.GetValueFromRatio(moveSpeed / enemyController.Agent.speed);
        }

        // 상태에 따른 Enemy 구현
        private void UpdateCurrentAiState()
        {
            switch (AiState)
            {
                case AIState.Patrol:
                    enemyController.UpdatePathDestination(true);
                    enemyController.SetNavDestination(enemyController.GetDestinationOnPath());
                    break;
                case AIState.Follow:
                    break;
                case AIState.Attack:
                    break;
            }
        }

        private void OnDamaged()
        {
            // 스파크 파티클 - 랜덤하게 하나 선택해서 플레이
            if(randomHitSparks.Length > 0)
            {
                int randNum = Random.Range(0, randomHitSparks.Length);
                randomHitSparks[randNum].Play();

                animator.SetTrigger(k_AnimOnDamagedParameter);
            }
        }
    }   
}
