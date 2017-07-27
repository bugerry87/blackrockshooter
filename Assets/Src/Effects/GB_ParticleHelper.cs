using UnityEngine;
using GB.Utils;
using System;

namespace GB.Effects
{
	[RequireComponent(typeof(ParticleSystem))]
	public class GB_ParticleHelper : GB_AUpdateMode {

		public enum ApplyType
		{
			None,
			Local,
			World
		}
		
		public bool updateParams;

		[Header("Orientation")]
		[SerializeField] private Transform m_Root;
		[SerializeField] private ApplyType applyPos;
		[SerializeField] private ApplyType applyRot;
		[SerializeField] private ApplyType applyScale;

		[Header("Size")]
		[SerializeField] private float m_StartSize;
		[SerializeField] private bool m_ApplySize;

		protected ParticleSystem.EmitParams emitParams;

		public ParticleSystem ps { get; protected set; }
		public Transform root { get { return m_Root; } set { m_Root = value; } }
		public bool applySize { get { return m_ApplySize; } set { m_ApplySize = value; } }
		public float startSize { get { return m_StartSize; } set { m_StartSize = value; } }

		void Start()
		{
			ps = GetComponent<ParticleSystem>();
			UpdateParams();
		}

		protected override void DoUpdate(float deltaTime)
		{
			ApplyParams();
		}

		public void UpdateParams()
		{
			if (root)
			{
				if (applyPos == ApplyType.World)
					emitParams.position = root.position;
				else if (applyPos == ApplyType.Local)
					emitParams.position = root.localPosition;

				if (applyRot == ApplyType.World)
					emitParams.rotation3D = root.rotation.eulerAngles;
				else if (applyRot == ApplyType.Local)
					emitParams.rotation3D = root.localRotation.eulerAngles;

				if (applyScale == ApplyType.World)
					emitParams.startSize3D = root.lossyScale;
				else if (applyScale == ApplyType.Local)
					emitParams.startSize3D = root.localScale;
			}

			if (applySize)
				emitParams.startSize = startSize;
		}

		public void ApplyParams()
		{
			if (!isActiveAndEnabled) return;
			var mm = ps.main;

			if (root)
			{
				if (applyRot == ApplyType.World)
				{
					var tmp = root.eulerAngles * Mathf.PI / 180;
					mm.startRotation = tmp.x;
					mm.startRotationX = tmp.x;
					mm.startRotationY = tmp.y;
					mm.startRotationZ = tmp.z;
				}
				else if (applyRot == ApplyType.Local)
				{
					var tmp = root.localEulerAngles * Mathf.PI / 180;
					mm.startRotation = tmp.x;
					mm.startRotationX = tmp.x;
					mm.startRotationY = tmp.y;
					mm.startRotationZ = tmp.z;
				}

				if (applyScale == ApplyType.World)
				{
					var tmp = root.lossyScale;
					mm.startSizeX = tmp.x;
					mm.startSizeY = tmp.y;
					mm.startSizeZ = tmp.z;
				}
				else if (applyScale == ApplyType.Local)
				{
					var tmp = root.localScale;
					mm.startSizeX = tmp.x;
					mm.startSizeY = tmp.y;
					mm.startSizeZ = tmp.z;
				}
			}

			if (applySize)
				mm.startSize = startSize;
		}

		public void Emit(int amount)
		{
			if (!isActiveAndEnabled) return;
			if (updateParams) UpdateParams();
			ps.Emit(emitParams, amount);
		}

		public void ResetParams()
		{
			emitParams.ResetAngularVelocity();
			emitParams.ResetAxisOfRotation();
			emitParams.ResetPosition();
			emitParams.ResetRandomSeed();
			emitParams.ResetRotation();
			emitParams.ResetStartColor();
			emitParams.ResetStartLifetime();
			emitParams.ResetStartSize();
			emitParams.ResetVelocity();
		}
	}
}
