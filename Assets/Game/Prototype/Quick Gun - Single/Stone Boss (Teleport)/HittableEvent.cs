using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class HittableEvent : MonoBehaviour{
		public UnityEvent onHit;

		private void Start(){
			gameObject.OnCollisionEnterAsObservable().Subscribe(OnHit);
		}

		private void OnHit(Collision obj){
			if(!obj.gameObject.TryGetComponent<Projectile>(out var projectile)){
				return;
			}

			onHit?.Invoke();
		}

		//Event PlaceHolder
		public void SceneReload(){
			SceneManager.LoadScene(0);
		}
	}
}