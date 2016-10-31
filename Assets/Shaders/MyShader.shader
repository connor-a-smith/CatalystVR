// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "test/MyShader"
{
	Properties
	{
		_sphereRadius("Sphere Radius", Range(0, 10.0)) = 5.0
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma geometry geom

#include "UnityCG.cginc"

	float _sphereRadius;
	//float halfRadius = _sphereRadius / 2.0;

		struct appdata
	{
		float4 vertex : POSITION;
		float4 color : COLOR;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float4 color : COLOR;
		float4 worldPos : FLOAT;

	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = v.color;
		o.worldPos = v.vertex;
		return o;
	}

	[maxvertexcount(50)]
	void geom(point v2f input[1], inout TriangleStream<v2f> OutputStream)
	{

		v2f test = (v2f)0;

		for (int i = 0; i < 1; i++)
		{
			//OutputStream.

			//float3 normal = normalize(cross(input[1].worldPosition.xyz - input[0].worldPosition.xyz, input[2].worldPosition.xyz - input[0].worldPosition.xyz));

				//test.normal = normal;
			test.vertex = input[i].worldPos += float4(-_sphereRadius,-_sphereRadius, 0,0);
			test.color = input[i].color;
			test.vertex = mul(UNITY_MATRIX_MVP, test.vertex);
			OutputStream.Append(test);

			test.vertex = input[i].worldPos += float4(-_sphereRadius, _sphereRadius, 0, 0);
			test.color = input[i].color;
			test.vertex = mul(UNITY_MATRIX_MVP, test.vertex);
			OutputStream.Append(test);

			test.vertex = input[i].worldPos += float4(_sphereRadius, -_sphereRadius, 0, 0);
			test.color = input[i].color;
			test.vertex = mul(UNITY_MATRIX_MVP, test.vertex);
			OutputStream.Append(test);

			test.vertex = input[i].worldPos += float4(_sphereRadius, _sphereRadius, 0, 0);
			test.color = input[i].color;
			test.vertex = mul(UNITY_MATRIX_MVP, test.vertex);
			OutputStream.Append(test);

			//test.uv = input[i].uv;
		}
	}

	fixed4 frag(v2f i) : SV_Target
	{

		return i.color;// *ndotl;
	}
		ENDCG
	}
	}
}