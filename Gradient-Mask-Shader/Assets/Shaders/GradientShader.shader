Shader "Unlit/GradientShader"
{
    Properties
    {
		[HideInInspector]_ProjectionRow0("Row0", Vector) = (0,0,0,0)
		[HideInInspector]_ProjectionRow1("Row1", Vector) = (0,0,0,0)
		[HideInInspector]_ProjectionRow2("Row2", Vector) = (0,0,0,0)
		[HideInInspector]_ProjectionRow3("Row3", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float4x4 gradientMatrix: TEXCOORD1;
            };

			float4 _ProjectionRow0, _ProjectionRow1, _ProjectionRow2, _ProjectionRow3;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
				o.gradientMatrix = float4x4(_ProjectionRow0, _ProjectionRow1, _ProjectionRow2, _ProjectionRow3);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 gradient = mul(i.gradientMatrix, float4(i.worldPos.xyz, 1.0));
                return float4(gradient.xxx, 1.0);
            }
            ENDCG
        }
    }
}
