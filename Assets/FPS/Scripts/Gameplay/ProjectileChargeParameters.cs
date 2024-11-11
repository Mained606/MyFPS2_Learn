using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 충전용 발사체를 발사할 때 충전량에 발사체의 속성값을 설정
    /// </summary>
    public class ProjectileChargeParameters : MonoBehaviour
    {
        #region Variables
        private ProjectileBase projectileBase;

        public MinMaxFloat Damage;
        public MinMaxFloat Speed;
        public MinMaxFloat GravityDown;
        public MinMaxFloat radius;
        #endregion

        void OnEnable()
        {
            //참조
            projectileBase = GetComponent<ProjectileBase>();
            projectileBase.OnShoot += OnShoot;
        }

        // 발사체 발사시 projectileBased의 OnShoot 델리게이트 함수에서 호출
        // 발사의 속성값을 Charge값에 따라 설정
        void OnShoot()
        {
            //충전량에 따라 발사체의 속성값 설정
            ProjectileStandard projectileStandard = GetComponent<ProjectileStandard>();
            projectileStandard.damage = Damage.getValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.speed = Speed.getValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.gravityDown = GravityDown.getValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.radius = radius.getValueFromRatio(projectileBase.InitialCharge);
        }
    }
}