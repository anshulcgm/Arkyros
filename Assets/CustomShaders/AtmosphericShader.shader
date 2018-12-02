Shader "Custom/AtmosphericShader" {
	Properties {
		 _MainTex ("Base (RGB)", 2D) = "white" {}
		 _MaskTex ("Mask texture", 2D) = "white" {}
		 _maskBlend ("Mask blending", Float) = 0.5
		 _maskSize ("Mask Size", Float) = 1
		 _skyColor ("Sky Color", Color) = (0,0,0,0)
		 _minDistEffect ("Min Dist", Float) = 0.5
		 _distEffectMaximized ("Dist Maximized", Float) = 1
	}
	SubShader {
		Tags{ "RenderType" = "Opaque" }
		Pass {
			 CGPROGRAM
			 #pragma vertex vert_img
			 #pragma fragment frag
			 #include "UnityCG.cginc"
 
			 fixed _maskSize;
			 uniform sampler2D _MainTex;
			 uniform sampler2D _MaskTex;
			 uniform sampler2D _CameraDepthTexture;

			 half4 _skyColor;
			 const float _minDistEffect;
			 const float _distEffectMaximized;

			 
 
			 fixed _maskBlend;
		     struct v2f {
				float4 pos : SV_POSITION;
				float4 scrPos: TEXCOORD1;
				float2 uv : TEXCOORD0;
			 };

			 //Vertex Shader
			 v2f vert (appdata_base v){
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.scrPos=ComputeScreenPos(o.pos);
				o.uv = v.texcoord;
				return o;
			 }


			 float4 frag (v2f i) : COLOR {
				float4 col = tex2D(_CameraDepthTexture, i.uv);

				//get depth value
				float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);

				//do bunch of calcs for atmospheric obscuring
				float interpValMinDist = max(0,sign(depthValue - _minDistEffect));
				float interpValDist = min(interpValMinDist * (depthValue - _minDistEffect)/(_distEffectMaximized - _minDistEffect), 1);
				float pixelIntensity = 0.2126 * col.rgb.r + 0.7152 * col.rgb.b + 0.0722 * col.rgb.g;
				float skyIntensity = 0.2126 * _skyColor.r + 0.7152 * _skyColor.b + 0.0722 * _skyColor.g;
				float interpValIntensity01 = max(0,sign(skyIntensity - pixelIntensity));
				float interpValIntensity = max((pixelIntensity - skyIntensity)/(1 - skyIntensity), interpValIntensity01);
				float interpVal = interpValIntensity * interpValDist * 0.001;

				
				return col;
			 }
			 ENDCG
		 }
	}
}