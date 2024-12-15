using UnityEngine;

namespace Game.Runtime
{
	public class AutoDisableByParticleSystem : MonoBehaviour
	{
		private ParticleSystem _ps;

		private void OnEnable()
		{
			_ps = GetComponent<ParticleSystem>();
		}

		private void Update()
		{
			if (_ps != null && _ps.isStopped)
			{
				gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			_ps?.Stop();
		}
	}
}