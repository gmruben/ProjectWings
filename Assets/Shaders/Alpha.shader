Shader "Custom/Alpha"
{
    Properties
	{
        _MainTex ("Atlas Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (1,1,1,1)
    }

    SubShader
	{
        Tags
		{ 
            "Queue"="Transparent"
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
        }

        Cull Off 
        Lighting Off 
		//ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        BindChannels
		{
            Bind "Color", color
            Bind "Vertex", vertex
            Bind "TexCoord", texcoord
        }

        Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _TintColor;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _MainTex_ST;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 texColor = tex2D (_MainTex, i.uv);				
				return float4(texColor.rgb, 0.5);
			}

			ENDCG
        }
    }
}
