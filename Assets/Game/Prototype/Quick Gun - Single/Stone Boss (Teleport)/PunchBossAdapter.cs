using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PunchBossAdapter : MonoBehaviour{
		[Required] public Transform player;
		[Required] public Collider detectTrigger;

		[BoxGroup("Weakness")] public BoxCollider weaknessFront;
		[BoxGroup("Weakness")] public BoxCollider weaknessBack;

		public int hitCount;


		public void LightAttack(){
			hitCount += 1;
		}
		public void HardAttack(){
			hitCount = 0;
		}
	}
}