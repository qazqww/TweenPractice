Shader "Custom/DissolveRamp" {

	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalTex("Bump", 2D) = "bump" {}

		// 디졸브 효과에 사용할 텍스처를 받아 올 수 있도록 합니다.
		// 이미지는 노이즈 이미지 입니다.
		_DissolveTex("DissolveTex", 2D) = "white" {}
		_DissolveRamp("DissolveRamp", 2D) = "white" {}

		// 디졸브 쉐이더에 적용할 색상 이미지의 범위 값을 나타냅니다.
		_DissolveRampSize("DissolveRampSize", Float) = 0.2

		// 녹이는 효과의 강도 값입니다.
		_DissolveAmount("Dissolove Amount", Range(0, 1)) = 0
		
		_EmissiveTex("Emissive", 2D) = "white" {}
		_EmissiveStrength("EmissiveStrength", Range(1, 15)) = 1
		_EmissiveColor("EmissiveColor", Color) = (1, 1, 1, 1)
		_Strength("Nomal Strength", Range(0, 5)) = 1
		_SmoothnessStrength("SmoothnessStrength", Range(0, 1)) = 0



	}
		
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
			// 
#pragma surface surf Standard addshadow
#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _EmissiveTex;

		sampler2D _DissolveTex;
		// 디졸브 쉐이더에 적용할 컬러 텍스쳐입니다.
		sampler2D _DissolveRamp;

		fixed _Strength;
		fixed _SmoothnessStrength;
		
		fixed _EmissiveStrength;
		fixed4 _EmissiveColor;

		fixed _DissolveAmount;

		// 디졸브 쉐이더에 적용할 컬러의 범위를 설정하기 위해 추가하였습니다.
		float _DissolveRampSize;


		struct Input {
			// 첫번째 텍스쳐 좌표계 입니다. 
			float2 uv_MainTex : TEXCOORD0;
			// 두번째 텍스쳐 좌표계 입니다.
			float2 uv_DissolveTex : TEXCOORD1;
		};

		fixed4 _Color;

		// 핵심을 간단히 정의내리자면,
		// 지정된 영역에 포함되는 색상이라면 비율의 역 색상값을 출력한다.
		// 지정된 범위보다 값이 작다면 ramp 텍스쳐 색상의 값을 그대로 사용한다.
		// 지정된 범위보다 값이 크다면 ramp 텍스쳐의 색상을 사용하지 않는다.
		void surf(Input IN, inout SurfaceOutputStandard o) {



			// 디졸브 텍스쳐로부터 r 값만을 얻어 올 수 있도록 합니다.
			float dissolveColor = tex2D(_DissolveTex, IN.uv_DissolveTex).r;
			// dissolveColor 칼라 값으로 해당 픽셀을 삭제 할 수 있도록 합니다.
			// clip 함수는 값으로 들어온 요소중 어느 하나라도 0보다 작다면 그릴 영역에서
			// 제외 시켜주는 함수입니다. 기억해 두세요.
			clip(dissolveColor - _DissolveAmount);

			// smoothstep 함수는 
			// 1) 첫번째 매개변수로 시점을 받고,
			// 2) 두번째 매개변수로 종점을 받으며,
			// 3) 세번째 매개변수로 현재값을 받습니다.
			// 하는 역할은 간단합니다. 현재 값이 시점(min) 값보다 작다면 0을 되돌려주고,
			// 현재 값이 종점(max) 보다 크다면 1을 리턴해주는 함수입니다.

			// 아래의 함수는 시점과 종점을 받고, 디졸브 간격을 조정하기 위해 작성한 코드입니다. 
			float ramp = smoothstep(_DissolveAmount, _DissolveAmount + _DissolveRampSize, dissolveColor);
			float3 rampColor = tex2D(_DissolveRamp, float2(ramp, 0));

			// 아래의 코드는 램프 텍스쳐의 색상값으로 출력하는 코드입니다.
			//float3 rampColor = tex2D(_DissolveRamp, IN.uv_DissolveTex );
			//o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			//o.Albedo += rampColor;

			float3 rampContribution = rampColor * (1-ramp);
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo += rampContribution;
			

			fixed4 norPixel = tex2D(_NormalTex, IN.uv_MainTex);
			fixed3 normal = UnpackNormal(norPixel);
			normal.xy *= _Strength;
			o.Normal = normalize(normal);

			//o.Emission = tex2D(_EmissiveTex, IN.uv_MainTex) * _EmissiveStrength * _EmissiveColor;
			//o.Emission += rampContribution * _EmissiveStrength;

			o.Smoothness = _SmoothnessStrength;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
