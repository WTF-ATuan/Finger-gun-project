using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossAdapter : MonoBehaviour{
		[Required] public Transform player;
		[Required] public Collider detectTrigger;

		[BoxGroup("Weakness")] public BoxCollider weaknessFront;
		[BoxGroup("Weakness")] public BoxCollider weaknessBack;

		public Collider lightAttackHitbox;
		public int hitCount;


		public void LightAttackStart(){
			lightAttackHitbox.gameObject.SetActive(true);
			hitCount += 1;
		}

		public void LightAttackOver(){
			lightAttackHitbox.gameObject.SetActive(false);
		}

		public void HardAttack(){
			hitCount = 0;
		}
	}
}