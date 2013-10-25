Shader "Custom/Color Light And Shadow"
{
    Properties
	{
        _MainTex ("Atlas Texture", 2D) = "white" {}
		_Color1Light ("Color 1 Light", Color) = (1,1,1,1)		//001
		_Color1Shadow ("Color 1 Shadow", Color) = (1,1,1,1)		//010
		_Color2Light ("Color 2 Light", Color) = (1,1,1,1)		//011
		_Color2Shadow ("Color 2 Shadow", Color) = (1,1,1,1)		//100
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
        ZWrite Off
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

			float4 _Color1Light;
			float4 _Color1Shadow;
			float4 _Color2Light;
			float4 _Color2Shadow;

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
				fixed4 texColor = tex2D (_MainTex, i.uv);
				
				if (texColor.r == 0.0 && texColor.g == 0.0 && texColor.b == 1.0 && texColor.a == 1.0)
				{
					texColor = _Color1Light;
				}
				else if (texColor.r == 0.0 && texColor.g == 1.0 && texColor.b == 0.0 && texColor.a == 1.0)
				{
					texColor = _Color1Shadow;
				}
				else if (texColor.r == 0.0 && texColor.g == 1.0 && texColor.b == 1.0)
				{
					texColor = _Color2Light;
				}
				else if (texColor.r == 1.0 && texColor.g == 0.0 && texColor.b == 0.0)
				{
					texColor = _Color2Shadow;
				}

				return texColor;
			}

			ENDCG
        }
    }
}
