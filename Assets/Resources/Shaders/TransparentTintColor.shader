Shader "Custom/Transparent Tint Color"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (0, 0, 0, 0)
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" } 
        // draw after all opaque geometry has been drawn
      
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
 
			GLSLPROGRAM
 
			uniform sampler2D _MainTex;
			uniform vec4 _TintColor;

			varying vec4 textureCoordinates;
			
			#ifdef VERTEX
			
			void main()
			{
				textureCoordinates = gl_MultiTexCoord0;
				gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
			}
 
			#endif
 
			#ifdef FRAGMENT
 
			void main()
			{
				vec4 texColor = texture2D(_MainTex, vec2(textureCoordinates));
				gl_FragColor = texColor * 0.5 + _TintColor * 0.5;
			}
 
			#endif
 
			ENDGLSL
		}
	}

	//Fallback "Diffuse"
}
