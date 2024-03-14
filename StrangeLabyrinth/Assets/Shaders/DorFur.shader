Shader "Custom/DorFur"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _displace ("Displace", Float) = 0.0
        _density ("Density", Float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.0;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 1;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 1.0) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.05;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;
        

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 2;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.95) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.1;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 3;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.9) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.15;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 4;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.85) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.2;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 5;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.8) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.25;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 6;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.75) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.3;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 7;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.7) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.35;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 8;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.65) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.4;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 9;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.6) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.45;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 10;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.55) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.5;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 11;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.5) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.55;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 12;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.45) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.6;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 13;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.4) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.65;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 14;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.35) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.7;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 15;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.3) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.75;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 16;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.25) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.8;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 17;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.2) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.85;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 18;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.15) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.9;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 19;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.1) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 0.95;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 20;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.05) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 1;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 21;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.05) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 1;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert
        sampler2D _MainTex;

        float _displace;
        float _density;

        struct Input
        {
            float2 uv_MainTex;
        };
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += normalize(v.normal) * _displace * 22;
        }
        half _Glossiness;
        half _Metallic;
        fixed4 _Color; void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 fractional = (frac(IN.uv_MainTex * _density) - 0.5) * 2;
            float radius = sqrt(fractional.x * fractional.x + fractional.y * fractional.y);
            if (radius > 0.05) discard;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            c.rgb *= 1;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

        }
        ENDCG

        
    }
    FallBack "Diffuse"
}
