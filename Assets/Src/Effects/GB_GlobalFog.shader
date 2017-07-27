Shader "Hidden/GB_GlobalFog" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "black" {}
}

CGINCLUDE

	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform sampler2D_float _CameraDepthTexture;
	
	uniform float4x4 _FrustumCornersWS;
	uniform float4 _LocalFogParams; //height, heightDenisty, minDistance, globalDensity
	uniform float4 _SceneFogParams; //fogMode, fogDensity, fogStart, fogEnd
	uniform float4 _CameraWS;

	#ifndef UNITY_APPLY_FOG
	half4 unity_FogColor;
	half4 unity_FogDensity;
	#endif	

	uniform float4 _MainTex_TexelSize;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv_depth : TEXCOORD1;
		float4 interpolatedRay : TEXCOORD2;
	};
	
	v2f vert (appdata_img v)
	{
		v2f o;
		half index = v.vertex.z;
		v.vertex.z = 0.1;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		o.uv_depth = v.texcoord.xy;
		
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1-o.uv.y;
		#endif				
		
		o.interpolatedRay = _FrustumCornersWS[(int)index];
		o.interpolatedRay.w = index;
		
		return o;
	}
	
	// Applies one of standard fog formulas, given fog coordinate (i.e. distance)
	half ComputeFogFactor (float coord)
	{
		float fogFac = 0.0;
		if (_SceneFogParams.x == 1) // linear
		{
			// factor = (end-z)/(end-start) = z * (-1/(end-start)) + (end/(end-start))
			fogFac = coord * _SceneFogParams.y * _SceneFogParams.z + _SceneFogParams.w;
		}
		if (_SceneFogParams.x == 2) // exp
		{
			// factor = exp(-density*z)
			fogFac = coord * 1.4426950408f * _SceneFogParams.y;
			fogFac = exp2(-fogFac);
		}
		if (_SceneFogParams.x == 3) // exp2
		{
			// factor = exp(-(density*z)^2)
			fogFac = coord * 1.2011224087f * _SceneFogParams.y;
			fogFac = exp2(-fogFac * fogFac);
		}
		return saturate(fogFac);
	}

	// Distance-based fog
	float ComputeDistance (float zdepth)
	{
		zdepth -= _ProjectionParams.y;
		return zdepth;
	}

	float ComputeHalfSpace (float3 cpos, float3 wsDir, float3 wVec)
	{
		if (_LocalFogParams.y == 0)
		{
			return 0;
		}

		float wdepth = length(wVec);
		float3 wpos = cpos + wVec;
		float wdrown = _LocalFogParams.x - wpos.y;
		float cdrown = _LocalFogParams.x - cpos.y;

		if (wsDir.y == 0)
		{
			//ignore
		}
		else if (cdrown < 0 && wdrown < 0) 
		{
			return 0;
		}
		else if (cdrown <= wdrown)
		{
			wdepth = min(wdepth, length(wsDir * wdrown / wsDir.y));
		}
		else
		{
			wdepth = min(wdepth, length(wsDir * cdrown / wsDir.y));
		}

		float dens = (2 - exp2(min(0, -cdrown) * _LocalFogParams.y) - exp2(min(0, -wdrown) * _LocalFogParams.y));
		return (wdepth - _LocalFogParams.z) * dens * _LocalFogParams.y;
	}

	half4 ComputeFog (v2f i, bool distance, bool height) : SV_Target
	{
		half4 sceneColor = tex2D(_MainTex, i.uv);
		
		// Reconstruct world space position & direction
		// towards this screen pixel.
		float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth);
		float dpth = Linear01Depth(rawDepth);
		float4 wsDir = i.interpolatedRay;

		// Compute fog distance
		float g = _SceneFogParams.z;
		if (distance)
		{
			g += ComputeDistance(dpth);
		}

		if (height)
		{
			g += ComputeHalfSpace(_CameraWS, wsDir, wsDir * dpth);
		}

		// Compute fog amount
		half fogFac = ComputeFogFactor(max(0.0, g));
		
		// Lerp between fog color & original scene color
		// by fog amount
		return lerp(unity_FogColor, sceneColor, fogFac);
	}

ENDCG

SubShader
{
	ZTest Less
	Cull Off
	ZWrite Off 
	Fog { Mode Off }
	Tags { "Queue" = "Overlay" }

	// 0: distance + height
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		half4 frag(v2f i) : SV_Target { return ComputeFog(i, true, true); }
		ENDCG
	}
	// 1: distance
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		half4 frag(v2f i) : SV_Target { return ComputeFog(i, true, false); }
		ENDCG
	}
	// 2: height
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		half4 frag(v2f i) : SV_Target { return ComputeFog(i, false, true); }
		ENDCG
	}
}

Fallback off

}
