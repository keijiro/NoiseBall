Shader "NoiseBall/Line"
{
    Properties
    {
        _Color ("", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0

        #include "Common.cginc"

        struct Input { float dummy; };

        fixed4 _Color;

        void vert(inout appdata_full v)
        {
            v.vertex.xyz = displace(v.vertex.xyz);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Emission = _Color.rgb;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
