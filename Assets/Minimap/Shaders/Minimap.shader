Shader "EOE/Minimap"
{
	Properties
	{
		_Rotation ("Rotate (In Rad)", Float) = 0.0

		_MainTex ("Height Map", 2D) = "white" {}
		_HeightStep ("Height Step", Float) = 5.0

		[NoScaleOffset] _ContourTex ("Contour Map", 2D) = "black" {}
		_ContourThickness ("Contour Thickness", Range(0.0, 10.0)) = 1.0
		_ContourStrength ("Contour Strength", Range(0.0, 1.0)) = 0.2

		[NoScaleOffset] _RoadTex ("Road Map", 2D) = "black" {}
		_RoadThickness ("Road Thickness", Range(0.0, 10.0)) = 1.0
		_RoadStrength ("Road Strength", Range(0.0, 1.0)) = 1.0
		_RoadColor ("Road Color", Color) = (0, 0, 0, 1)

		[NoScaleOffset] _RiverTex ("River Map", 2D) = "black" {}
		_RiverThickness ("River Thickness", Range(0.0, 10.0)) = 1.0
		_RiverColor ("River Color", Color) = (0, 0, 0, 1)

		_EdgeBorderScale ("Edge Border Scale", Float) = 1.0
		_EdgeBorderFalloff ("Edge Border Falloff", Range(0.0, 10.0)) = 5.0
		_EdgeBorderColor ("Edge Border Color", Color) = (0, 0, 0, 1)

		[MaterialToggle] _IsCircular ("Is Circular", Float) = 1.0
		_CircularFalloff ("Circle Falloff", Range(0.0, 1.0)) = 0.2

		[HideInInspector]_Stencil ("Stencil ID", Float) = 0
		[HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
		[HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector]_ColorMask ("Color Mask", Float) = 15
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Transparent"
			"UniversalMaterialType" = "Unlit"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp]
			WriteMask [_StencilWriteMask]
			ReadMask [_StencilReadMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			Name "Minimap"
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#include "MinimapHelper.hlsl"

			struct VertexInfo
			{
				float4 position : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct FragmentInfo
			{
				float4 position : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			FragmentInfo vert(VertexInfo v)
			{
				FragmentInfo f;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(f);

				f.position = UnityObjectToClipPos(v.position);
				f.color = v.color;
				f.uv = v.uv;
				f.worldPosition = v.position;
				return f;
			}

			// Properties
			float _Rotation;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _HeightStep;

			sampler2D _ContourTex;
			float _ContourThickness;
			float _ContourStrength;

			sampler2D _RoadTex;
			float _RoadThickness;
			float _RoadStrength;
			float4 _RoadColor;

			sampler2D _RiverTex;
			float _RiverThickness;
			float4 _RiverColor;

			float _EdgeBorderScale;
			float _EdgeBorderFalloff;
			float4 _EdgeBorderColor;

			float _IsCircular;
			float _CircularFalloff;

			#define REPLACE_START
			static float4 _Colors[6] = { float4(0.3886792, 0.1995243, 0.04033459, 1), float4(0.07371303, 0.3283019, 0.07371303, 1), float4(0, 0.6301887, 0.2974742, 1), float4(0.07133492, 0.8792453, 0.3081172, 1), float4(0.8469215, 1, 0.4056603, 1), float4(0.953431, 0.9924528, 0.7920896, 1) };
			#define REPLACE_END


			float4 SampleColorFromColorsArray(float t)
			{
				float4 color = _Colors[0];

				#define REPLACE_START
				int count = 6;
				#define REPLACE_END

				[unroll]
				for (int i = 1; i < count; i++)
				{
					float prevPercent = (i - 1) / (float)count;
					float currPercent = i / (float)count;
					float pos = saturate((t - prevPercent) / (currPercent - prevPercent)) * step(i, count - 1.0);
					color = lerp(color, _Colors[i], lerp(pos, step(0.01, pos), 2.0));
				}
				
				return color;
			}

			float4 frag(FragmentInfo f) : SV_Target
			{
				float len = length(f.uv * 2.0f - 1.0f);
				float smoothAlpha = 1.0f;

				if (_IsCircular > 0.5f)
				{
					smoothAlpha = 1.0f - smoothstep(1.0f - _CircularFalloff, 1.0f, len);
					float alpha = 1.0f - saturate((len - 1.0f) / fwidth(len));
					clip(alpha - 0.5);
				}

				float2 transformedUV = RotateUV_Radian((f.uv - 0.5f) * _MainTex_ST.xy, float2(0.0, 0.0), _Rotation) + _MainTex_ST.zw;
				if (transformedUV.x < 0.0f || transformedUV.x > 1.0f || transformedUV.y < 0.0f || transformedUV.y > 1.0f)
				{
					return float4(_EdgeBorderColor.xyz, f.color.a * smoothAlpha);
				}

				// Height info
				float height = tex2D(_MainTex, transformedUV).r;
				float heightStep = floor(height * _HeightStep) / _HeightStep;
				float4 heightColor = SampleColorFromColorsArray(heightStep);

				// Contour info
				float contour = saturate(tex2D(_ContourTex, transformedUV).r * _ContourThickness);
				float3 contourColor = RGB2HSV(heightColor.xyz);
				contourColor.b *= (1.0 - _ContourStrength);
				contourColor = HSV2RGB(contourColor);

				// Road info
				float road = saturate(tex2D(_RoadTex, transformedUV).r * _RoadThickness);
				float3 roadColor = RGB2HSV(heightColor.xyz);
				roadColor.z *= (1.0 - _RoadStrength);
				roadColor = HSV2RGB(roadColor);

				// River info
				float river = saturate(tex2D(_RiverTex, transformedUV).r * _RiverThickness);

				// Border info
				float2 borderUV = abs(transformedUV * 2.0 - 1.0);
				float2 borderCoord = pow(saturate(abs(borderUV) * _EdgeBorderScale), _EdgeBorderFalloff);
				float border = 1.0 - saturate(max(borderCoord.x, borderCoord.y));

				// Final color
				float4 outColor = lerp(heightColor, float4(contourColor, 1.0), contour);
				outColor = lerp(outColor, float4(roadColor, 1.0), road);
				outColor = lerp(outColor, float4(_RiverColor.xyz, 1.0), river);
				outColor = lerp(_EdgeBorderColor, outColor, border);

				return float4(outColor.xyz, f.color.a * smoothAlpha);
			}

			ENDCG
		}
	}
}