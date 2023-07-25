using System;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossAdapter : MonoBehaviour{
		[Required] [BoxGroup("Basic")] public Transform player;
		[Required] [BoxGroup("Basic")] public Collider detectTrigger;
		[Required] [BoxGroup("Basic")] public float bossStartHp = 100;
		[Required] [BoxGroup("Basic")] public float bossTurningDuration = 5;

		[BoxGroup("Weakness")] public BoxCollider weaknessFront;
		[BoxGroup("Weakness")] public BoxCollider weaknessBack;
		[BoxGroup("Weakness")] public GameObject weaknessHitVFX;


		[ReadOnly] public int hitCount;
		[ReadOnly] public float bossHp;
		[ReadOnly] public int bossCurrentStage = 1;

		public UnityEvent enableSecondStage;
		public UnityEvent enableThirdStage;
		public UnityEvent enableBossDead;

		private void Start(){
			weaknessFront.OnCollisionEnterAsObservable().Subscribe(OnWeaknessHit);
			weaknessBack.OnCollisionEnterAsObservable().Subscribe(OnWeaknessHit);
			bossCurrentStage = 1;
			bossHp = bossStartHp;
		}

		[Button]
		private void TestHit(float amount = 10){
			DamageBoss(amount);
		}

		private void OnWeaknessHit(Collision obj){
			if(!obj.gameObject.TryGetComponent<Projectile>(out var projectile)){
				return;
			}

			var vfxClone = Instantiate(weaknessHitVFX, obj.GetContact(0).point,
				Quaternion.Euler(obj.GetContact(0).normal));
			Destroy(vfxClone, 0.35f);
			DamageBoss();
		}

		private void DamageBoss(float damageAmount = 7.5f){
			bossHp -= damageAmount;
			if(bossHp > 0) return;
			bossCurrentStage += 1;
			switch(bossCurrentStage){
				case 2:
					enableSecondStage?.Invoke();
					break;
				case 3:
					enableThirdStage?.Invoke();
					break;
				case 4:
					enableBossDead?.Invoke();
					return;
			}

			bossHp = 100;
		}

		public void ModifyTurnDuration(float duration){
			bossTurningDuration = duration;
		}

		#region Trigger_By_Animation

		public void LightAttack(){
			hitCount += 1;
		}

		public void HardAttack(){
			hitCount = 0;
		}

		#endregion
	}
}