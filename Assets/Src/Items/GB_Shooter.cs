using UnityEngine;

namespace GBAssets.Items
{
	[RequireComponent(typeof(Animator))]
	public class GB_Shooter : MonoBehaviour
	{
		[System.Serializable]
		class BoolControl
		{
			[SerializeField]
			public string button = "Fire1";
			[SerializeField]
			public string param = "Shoot";
		}

		[SerializeField]
		GameObject prefab = null;

		[SerializeField]
		float spread = 0;

		[SerializeField]
		Vector3 offset = Vector3.zero;

		[SerializeField]
		BoolControl[] controls = { new BoolControl() };

		protected Animator animator {get; private set;}

		void Start()
		{
			animator = GetComponent<Animator>();
		}

		void FixedUpdate()
		{
			foreach (BoolControl c in controls)
			{
                if (Input.GetButton(c.button))
				{
					animator.SetBool(c.param, true);
				}
				else if (Input.GetAxis(c.button) != 0)
				{
					animator.SetBool(c.param, true);
				}
				else
				{
					animator.SetBool(c.param, false);
				}
			}
		}

		public void ApplyShoot()
		{
			if (prefab != null)
			{
				if (spread == 0)
				{
					Instantiate(prefab, transform.position + transform.TransformVector(offset), transform.rotation);
				}
				else
				{
					Vector3 random = transform.forward + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
					Instantiate(prefab, transform.position + transform.TransformVector(offset), Quaternion.LookRotation(random));
				}
			}
		}

        public void SpawnShooter()
        {
            animator.SetBool("Spawn", true);
        }

        public void DespawnShooter()
        {
            animator.SetBool("Spawn", false);
        }
	}
}
