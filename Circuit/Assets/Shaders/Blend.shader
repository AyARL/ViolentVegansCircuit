Shader "Custom/Blend"
{
	Properties
	{
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_MainTex2 ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Blend ("Blend", Range (0, 1)) = 0
	}
	SubShader
	{
		LOD 300
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _MainTex2;
		float _Blend;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 tex2 = tex2D(_MainTex2, IN.uv_MainTex2);

			fixed4 c = lerp(tex, tex2, _Blend);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}
