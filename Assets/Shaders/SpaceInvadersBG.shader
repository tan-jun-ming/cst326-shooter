Shader "Custom/SpaceInvadersBG"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
	}

		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			// Properties
			sampler2D _MainTex;
			sampler2D _Mask;

			float4 frag(v2f_img input) : COLOR
			{
				return tex2D(_MainTex, input.uv) * tex2D(_Mask, input.uv);
			}

			ENDCG
		}
	}
}