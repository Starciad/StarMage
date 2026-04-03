#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

// Palette with exactly 4 colors (NES-style)
float4 Palette[4];

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// Convert grayscale value to exact index (0–3)
int GetPaletteIndex(float value)
{
    // Expected encoded values:
    // 0.0, 0.3333, 0.6666, 1.0
    // Multiply and round safely
    int index = (int)round(saturate(value) * 3.0);

    // Safety clamp
    return clamp(index, 0, 3);
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 texColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);

    // Transparency check
    if (texColor.a < 0.1)
    {
        discard;
    }

    // Use RED channel as index source
    int index = GetPaletteIndex(texColor.r);

    float4 finalColor = Palette[index];

    // Apply vertex color (for tinting if needed)
    return finalColor * input.Color;
}

technique NesPaletteIndexed
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};