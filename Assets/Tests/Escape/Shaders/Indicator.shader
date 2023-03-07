Shader "Escape/Indicator"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Angle ("Angle", Range(0, 360)) = 60
        _Gradient ("Gradient", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"
        }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _Angle;
            float _Gradient;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXTCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float distance = sqrt(pow(i.uv.x - 0.5, 2) + pow(i.uv.y - 0.5, 2));
                if (distance > 0.5f)
                {
                    discard;
                }

                float gradient = 1 - (distance + 0.5 * _Gradient);
                fixed4 color = tex2D(_MainTex, i.uv) * _Color;
                color.a *= gradient;

                float x = i.uv.x;
                float y = i.uv.y;
                float deg2rad = 0.017453;
                float tangent = abs(x - 0.5) / abs(y - 0.5);
                if (_Angle > 180)
                {
                    if (y > 0.5 && tangent <= tan((180 - _Angle / 2) * deg2rad))
                        discard;
                }
                else
                {
                    if (y > 0.5 || tangent > tan(_Angle / 2 * deg2rad))
                        discard;
                }

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}