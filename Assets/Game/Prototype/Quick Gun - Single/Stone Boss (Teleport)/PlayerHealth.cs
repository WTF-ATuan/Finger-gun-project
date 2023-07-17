using System.Globalization;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class PlayerHealth : MonoBehaviour{
		[SerializeField] private Collider playerHitBox;
		[BoxGroup("UI")] [SerializeField] private Slider healthSlider;
		[BoxGroup("UI")] [SerializeField] private Image healthBackground;
		[BoxGroup("UI")] [SerializeField] private TMP_Text healthText;
		[BoxGroup("UI")] [SerializeField] private Color healthyColor = Color.green;
		[BoxGroup("UI")] [SerializeField] private Color badColor = Color.red;

		[SerializeField] private float playerMaxHp = 100;
		private float _currentHp;

		private void Start(){
			_currentHp = playerMaxHp;
			UpdateHealthUI();
			playerHitBox.OnTriggerEnterAsObservable()
					.Subscribe(OnHitBoxHit);
			EventAggregator.OnEvent<TargetHitEvent>()
					.Subscribe(x => {
						_currentHp += 5;
						UpdateHealthUI();
					});
		}

		private void OnHitBoxHit(Collider obj){
			if(!obj.TryGetComponent(out Projectile projectile)) return;
			_currentHp -= 20;
		}

		private void UpdateHealthUI(){
			// ReSharper disable once PossibleLossOfFraction
			// 20 / 100 0.2f;
			var lerpValue = _currentHp / playerMaxHp;
			var lerpColor = Color.Lerp(badColor, healthyColor, lerpValue);
			healthSlider.value = lerpValue;
			healthBackground.color = lerpColor;
			healthText.text = _currentHp.ToString(CultureInfo.InvariantCulture);
			healthText.color = lerpColor;
		}

		[Button]
		private void TestingButton(){
			_currentHp -= 20;
			UpdateHealthUI();
		}
	}
}