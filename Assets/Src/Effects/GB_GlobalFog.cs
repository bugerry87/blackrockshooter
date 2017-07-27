using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace GB.Effects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    class GB_GlobalFog : PostEffectsBase
	{
		[Tooltip("Apply distance-based fog?")]
        public bool  distanceFog = true;
		[Tooltip("Apply height-based fog?")]
		public bool  heightFog = true;
		[Tooltip("Fog start Y coordinate")]
        public float height = 1.0f;
		[Tooltip("Push fog away from camera")]
        public float minDistance = 1.0f;
        [Range(0.0f, 2.0f)]
        public float heightDensity = 1.0f;
		[Range(0.0f, 2.0f)]
        public float globalDensity = 1.0f;

        public Shader fogShader = null;
        private Material fogMaterial = null;

        public override bool CheckResources()
		{
            CheckSupport(true);
            fogMaterial = CheckShaderAndCreateMaterial(fogShader, fogMaterial);
            if (!isSupported) ReportAutoDisable();
            return isSupported;
        }

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
            if (CheckResources() == false || (!distanceFog && !heightFog))
            {
                Graphics.Blit (source, destination);
                return;
            }
			
			Camera cam = GetComponent<Camera>();
			Transform camtr = cam.transform;
			float camNear = cam.nearClipPlane;
			float camFar = cam.farClipPlane;
			float camFov = cam.fieldOfView;
			float camAspect = cam.aspect;

            Matrix4x4 frustumCorners = Matrix4x4.identity;

			float fovWHalf = camFov * 0.5f;

			Vector3 toRight = camtr.right * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * camAspect;
			Vector3 toTop = camtr.up * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);

			Vector3 topLeft = (camtr.forward * camNear - toRight + toTop);
			float camScale = topLeft.magnitude * camFar/camNear;

            topLeft.Normalize();
			topLeft *= camScale;

			Vector3 topRight = (camtr.forward * camNear + toRight + toTop);
            topRight.Normalize();
			topRight *= camScale;

			Vector3 bottomRight = (camtr.forward * camNear + toRight - toTop);
            bottomRight.Normalize();
			bottomRight *= camScale;

			Vector3 bottomLeft = (camtr.forward * camNear - toRight - toTop);
            bottomLeft.Normalize();
			bottomLeft *= camScale;

            frustumCorners.SetRow (0, topLeft);
            frustumCorners.SetRow (1, topRight);
            frustumCorners.SetRow (2, bottomRight);
            frustumCorners.SetRow (3, bottomLeft);

			var camPos = camtr.position;
            fogMaterial.SetMatrix("_FrustumCornersWS", frustumCorners);
            fogMaterial.SetVector("_CameraWS", camPos);
            fogMaterial.SetVector("_LocalFogParams", new Vector4(height, heightDensity, minDistance, globalDensity));
			fogMaterial.SetVector("_SceneFogParams", new Vector4((float)RenderSettings.fogMode, RenderSettings.fogDensity, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance));

            int pass = 0;
            if (distanceFog && heightFog)
                pass = 0; // distance + height
            else if (distanceFog)
                pass = 1; // distance only
            else
                pass = 2; // height only
            CustomGraphicsBlit (source, destination, fogMaterial, pass);
        }

        static void CustomGraphicsBlit (RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
		{
            RenderTexture.active = dest;

            fxMaterial.SetTexture ("_MainTex", source);

            GL.PushMatrix ();
            GL.LoadOrtho ();

            fxMaterial.SetPass (passNr);

            GL.Begin (GL.QUADS);

            GL.MultiTexCoord2 (0, 0.0f, 0.0f);
            GL.Vertex3 (0.0f, 0.0f, 3.0f); // BL

            GL.MultiTexCoord2 (0, 1.0f, 0.0f);
            GL.Vertex3 (1.0f, 0.0f, 2.0f); // BR

            GL.MultiTexCoord2 (0, 1.0f, 1.0f);
            GL.Vertex3 (1.0f, 1.0f, 1.0f); // TR

            GL.MultiTexCoord2 (0, 0.0f, 1.0f);
            GL.Vertex3 (0.0f, 1.0f, 0.0f); // TL

            GL.End ();
            GL.PopMatrix ();
        }
    }
}
