Shader "X-Ray" {  
    Properties {  
        _MainTex ("Base (RGB)", 2D) = "white" {}  
        _RimColor("Rim Color",color) = (1,1,1,1)  
        _RimPower("Rim Power",float) = 0.5  
        _RimBright("Rim Bright",float) = 1  
    }  
    SubShader {  
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}  
        Cull back ZWrite Off Fog { Color (0,0,0,0) }
        ZTest off
        Blend SrcAlpha OneMinusSrcAlpha 
          
        CGPROGRAM  
        #pragma surface surf Rim
  
        sampler2D _MainTex;  
        half4 _RimColor;  
        half _RimPower;  
        half _RimBright;
          
        struct Input {  
            float2 uv_MainTex;  
        };  
          
        inline fixed4 LightingRim(SurfaceOutput s,half3 lightDir,half3 viewDir,half atten){  
              
            fixed NdotE = max(0,dot(s.Normal,viewDir));  

            fixed rimLight = 1 - NdotE;  
            rimLight = pow(rimLight,_RimPower);  
              
            fixed4 c;  
            c.rgb = (rimLight.xxx * _RimColor);  
            c.a = s.Alpha * rimLight.x;  
            return c;  
        }  
  
        void surf (Input IN, inout SurfaceOutput o) { 

            half4 c = tex2D (_MainTex, IN.uv_MainTex);            
            o.Albedo = _RimColor * _RimBright;;    
            o.Alpha = c.a;  
        }  
        ENDCG  
    }   
}  
