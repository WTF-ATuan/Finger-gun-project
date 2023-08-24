using System.Collections.Generic;
using Game.Prototype.Pistol;
using Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_;
using UniRx;
using UnityEngine;

namespace Game.Prototype.SubstituteFeature{
	public class DodgeAbility : MonoBehaviour{
		[SerializeField] private List<ShotGunBinder> effectComponent;
		[SerializeField] private ParticleSystem abilityVfx;
		[SerializeField] private AudioClip soundEffect;

		private AudioSource _audioSource;


		private void Start(){
			EventAggregator.OnEvent<DodgeAction>().Subscribe(OnDodge);
			_audioSource = gameObject.AddComponent<AudioSource>();
		}

		private void OnDodge(DodgeAction obj){
			effectComponent.ForEach(x => x.ModifyCurrentAmmo(x.ammoMax));
			abilityVfx.Play();
			_audioSource.PlayOneShot(soundEffect);
		}
	}
}