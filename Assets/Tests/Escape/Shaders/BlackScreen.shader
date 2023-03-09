Shader "Escape/BlackScreen"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Radius("Radius", float) = 1.5
        _CenterX("CenterX", float) = 0.5
        _CenterY("CenterY", float) = 0.5
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _Radius;
            float _CenterX;
            float _CenterY;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float centerX = (i.uv.x - _CenterX) * (_ScreenParams.x / _ScreenParams.y);
                float centerY = i.uv.y - _CenterY;
                float distance = sqrt(pow(centerX, 2) + pow(centerY, 2));
                if(distance > _Radius)
                {
                    return fixed4(0, 0, 0, 0);
                }

                return tex2D(_MainTex, i.uv) * _Color;
            }
            ENDCG
        }
    }
}