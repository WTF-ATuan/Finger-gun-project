using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Prototype.SoundEffect{
	[RequireComponent(typeof(AudioSource))]
	public class SoundEffectHandler : MonoBehaviour{
		private AudioSource _sfxSource;
		[SerializeField] private List<SFXData> sfxDataList;


		private void Start(){
			_sfxSource = GetComponent<AudioSource>();
			EventAggregator.OnEvent<SFXEvent>().Subscribe(PlaySFXByEvent);
			DontDestroyOnLoad(gameObject);
		}

		private void PlaySFXByEvent(SFXEvent obj){
			var foundSFX = sfxDataList.Find(x => x.sfxName.Equals(obj.SFXName));
			if(foundSFX == null) throw new NullReferenceException($"{obj.SFXName} is not found");
			_sfxSource.transform.position = obj.SFXPosition;
			_sfxSource.PlayOneShot(foundSFX.sfxClip);
		}
	}

	public class SFXEvent{
		public string SFXName;
		public Vector3 SFXPosition;

		public SFXEvent(string sfxName, Vector3 sfxPosition){
			SFXName = sfxName;
			SFXPosition = sfxPosition;
		}
	}

	[Serializable]
	public class SFXData{
		public string sfxName;
		public AudioClip sfxClip;
	}
}