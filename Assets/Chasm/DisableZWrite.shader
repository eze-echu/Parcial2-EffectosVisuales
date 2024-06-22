Shader "Custom/DiableZWrite"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        
        Pass
        {
            ZWrite Off
        }
    }
}
