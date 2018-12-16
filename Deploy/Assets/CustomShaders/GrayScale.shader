Shader "Custom/GrayScale" {
	 Properties {
		 _MainTex ("Base (RGB)", 2D) = "white" {}
		 _bwBlend ("Black & White blend", Range (0, 1)) = 0		 
		 _skyColor ("Sky Color", Color) = (0,0,0,0)
		 _sunColor ("Sun Color", Color) = (0,0,0,0)
		 _minDistEffect ("Min Dist", Float) = 0.5
		 _distEffectMaximized ("Dist Maximized", Float) = 1
		 _intensityEndObscureBlend ("Intensity above sky intensity where color blending ends", Float) = 0
	 }
	 SubShader {
		 Pass {
			 CGPROGRAM
			 #pragma vertex vert_img
			 #pragma fragment frag
 
			 #include "UnityCG.cginc"
 
			 uniform sampler2D _MainTex;
			 uniform sampler2D _CameraDepthTexture;
			 uniform float _bwBlend;

			 half4 _skyColor;
			 half4 _sunColor;
			 const float _minDistEffect;
			 const float _distEffectMaximized;
			 const float _intensityEndObscureBlend;
			 


			
			 //@TODO improve efficiency of the fragment shader. Depth Texture sampling in the frag shader might be making things too slow.
			 float4 frag(v2f_img i) : COLOR {

				 //get color and depth values
				 float4 col = tex2D(_MainTex, i.uv);				 
				 float depthValue = Linear01Depth(tex2D(_CameraDepthTexture, i.uv).r);
				 
				 //do bunch of calcs for atmospheric obscuring

				 //the intensity of the pixel that we're looking at.
				 float pixelIntensity = 0.2126 * col.rgb.r + 0.7152 * col.rgb.g + 0.0722 * col.rgb.b;
				 //the base sky intensity
				 float skyIntensity = 0.2126 * _skyColor.r + 0.7152 * _skyColor.g + 0.0722 * _skyColor.b;
				 
				 //if the depth is less than the min dist, this is 0, so we don't obscure. 
				 float lessThanMinDist = max(0,sign(depthValue - _minDistEffect));
				 
				 //if the pixel intensity is greater than the sky intensity, this is 0, so we don't obscure.
				 float greaterThanMaxBlendIntensity = max(0,sign(_intensityEndObscureBlend + skyIntensity - (pixelIntensity))) * abs(sign(abs(col.rgb.r -_sunColor.r) + abs(col.rgb.g -_sunColor.g) * 10 + 100 * abs(col.rgb.b -_sunColor.b))) ;

				 //
				 float intensityInterpVal =  1 - max(0, ((pixelIntensity - skyIntensity)/(_intensityEndObscureBlend)));

				 //
				 float distInterpVal = min(1, (depthValue - _minDistEffect)/(_distEffectMaximized - _minDistEffect));

				 float interpVal = intensityInterpVal * distInterpVal * lessThanMinDist * greaterThanMaxBlendIntensity;
				 
				 //lerp in the pixels
				 return lerp(col, _skyColor, interpVal);
			}


			ENDCG
		 }
	 }
}