#if !defined(LOOKING_THROUGH_WATER)
#define LOOKING_THROUGH_WATER

sampler2D _CameraDepthTexture, _WaterBackground;
float4 _CameraDepthTexture_TexelSize;
float3 _WaterFogColour;
float _WaterFogDensity, _RefractionStrength;

float2 AlignWithGrabTexel (float2 uv)
{
    #if UNITY_UV_STARTS_AT_TOP
        if (_CameraDepthTexture_TexelSize.y < 0)
        {
            uv.y = 1 - uv.y;
        }
    #endif
    return (floor(uv * _CameraDepthTexture_TexelSize.zw) + 0.5) 
        * abs(_CameraDepthTexture_TexelSize.xy);
}

float3 ColourBelowWater (float4 screenPos, float3 tangentSpaceNormal)
{
    float2 uvOffset = tangentSpaceNormal.xy * _RefractionStrength;
    uvOffset.y *= _CameraDepthTexture_TexelSize.z * abs(_CameraDepthTexture_TexelSize.y);
    float2 uv = AlignWithGrabTexel((screenPos.xy + uvOffset) / screenPos.w);
    float backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
    float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z);
    float depthDifference = backgroundDepth - surfaceDepth;
    
    uvOffset *= saturate(depthDifference);
    uv = AlignWithGrabTexel((screenPos.xy + uvOffset) / screenPos.w);
    backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
    depthDifference = backgroundDepth - surfaceDepth;
    //if (depthDifference > 0.01 && depthDifference < 0.05 ) depthDifference = 2.0;
    depthDifference = 2.0;
    
    float3 backgroundColour = tex2D(_WaterBackground, uv).rgb;
    float fogFactor = exp2(-_WaterFogDensity * depthDifference);
    return lerp(_WaterFogColour, backgroundColour, fogFactor);
}

#endif