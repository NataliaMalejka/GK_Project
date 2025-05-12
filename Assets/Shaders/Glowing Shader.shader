Shader "Custom/FadeFromBottomMaskSmooth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _PlayerDistance ("Player Distance", Float) = 10.0
        _NearDistance ("Near Distance", Float) = 5.0
        _FarDistance ("Far Distance", Float) = 20.0
        _Smoothness ("Smoothness", Float) = 12.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _PlayerDistance;
            float _NearDistance;
            float _FarDistance;
            float _Smoothness;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Obliczamy fade na podstawie odległości, teraz z krzywą przejścia
                float fade = saturate((_FarDistance - _PlayerDistance) / (_FarDistance - _NearDistance));
                fade = pow(fade, _Smoothness); // Dodanie płynności przejścia

                // Obliczamy procent widoczności na podstawie odległości gracza
                // 1.0 (100%) gdy gracz jest na _NearDistance lub bliżej
                // 0.0 (0%) gdy gracz jest na _FarDistance lub dalej
                // Liniowa interpolacja pomiędzy
                float visibilityPercent = 1.0 - saturate((_PlayerDistance - _NearDistance) / (_FarDistance - _NearDistance));
                
                // Jeśli współrzędna UV.y jest większa niż procent widoczności, odrzucamy fragment
                if (i.uv.y > visibilityPercent)
                    discard; // Ukrycie fragmentu powyżej linii

                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color * i.color;
                // Nie modyfikujemy alpha, ponieważ używamy discard do kontroli widoczności
                
                return texColor;
            }
            ENDCG
        }
    }
}