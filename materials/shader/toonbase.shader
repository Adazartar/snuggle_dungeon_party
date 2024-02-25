
HEADER
{
	Description = "";
}

FEATURES
{
	#include "common/features.hlsl"
}

MODES
{
	VrForward();
	Depth(); 
	ToolsVis( S_MODE_TOOLS_VIS );
	ToolsWireframe( "vr_tools_wireframe.shader" );
	ToolsShadingComplexity( "tools_shading_complexity.shader" );
}

COMMON
{
	#ifndef S_ALPHA_TEST
	#define S_ALPHA_TEST 0
	#endif
	#ifndef S_TRANSLUCENT
	#define S_TRANSLUCENT 0
	#endif
	
	#include "common/shared.hlsl"
	#include "procedural.hlsl"

	#define S_UV2 1
	#define CUSTOM_MATERIAL_INPUTS
}

struct VertexInput
{
	#include "common/vertexinput.hlsl"
	float4 vColor : COLOR0 < Semantic( Color ); >;
};

struct PixelInput
{
	#include "common/pixelinput.hlsl"
	float3 vPositionOs : TEXCOORD14;
	float3 vNormalOs : TEXCOORD15;
	float4 vTangentUOs_flTangentVSign : TANGENT	< Semantic( TangentU_SignV ); >;
	float4 vColor : COLOR0;
};

VS
{
	#include "common/vertex.hlsl"

	PixelInput MainVs( VertexInput v )
	{
		PixelInput i = ProcessVertex( v );
		i.vPositionOs = v.vPositionOs.xyz;
		i.vColor = v.vColor;

		VS_DecodeObjectSpaceNormalAndTangent( v, i.vNormalOs, i.vTangentUOs_flTangentVSign );

		return FinalizeVertex( i );
	}
}

PS
{
	#include "common/pixel.hlsl"
	
	SamplerState g_sSampler0 < Filter( ANISO ); AddressU( WRAP ); AddressV( WRAP ); >;
	CreateInputTexture2D( AlbedoMap, Srgb, 8, "None", "_color", ",0/,0/0", Default4( 1.00, 1.00, 1.00, 1.00 ) );
	Texture2D g_tAlbedoMap < Channel( RGBA, Box( AlbedoMap ), Srgb ); OutputFormat( DXT5 ); SrgbRead( True ); >;
	float4 g_vTint < UiType( Color ); UiGroup( ",0/,0/0" ); Default4( 1.00, 1.00, 1.00, 1.00 ); >;
	float g_flF_ShadowSS_Min < UiGroup( ",0/,0/0" ); Default1( 0 ); Range1( 0, 1 ); >;
	float g_flF_ShadowSS_Max < UiGroup( ",0/,0/0" ); Default1( 0.8929459 ); Range1( 0, 1 ); >;
	float g_flF_ShadowPwr < UiGroup( ",0/,0/0" ); Default1( -0.09166849 ); Range1( -1, 2 ); >;
	float4 g_vShadowColour < UiType( Color ); UiGroup( ",0/,0/0" ); Default4( 1.00, 1.00, 1.00, 1.00 ); >;
	float g_flF_HighlightSS_Min < UiGroup( ",0/,0/0" ); Default1( 1 ); Range1( 0, 1 ); >;
	float g_flF_HighlightSS_Max < UiGroup( ",0/,0/0" ); Default1( 1 ); Range1( 0, 1 ); >;
	float g_flF_HighlightPwr < UiGroup( ",0/,0/0" ); Default1( -0.17884111 ); Range1( -1, 2 ); >;
	float4 g_vFresnelColour < UiType( Color ); UiGroup( ",0/,0/0" ); Default4( 1.00, 1.00, 1.00, 1.00 ); >;
	
	float4 MainPs( PixelInput i ) : SV_Target0
	{
		Material m = Material::Init();
		m.Albedo = float3( 1, 1, 1 );
		m.Normal = float3( 0, 0, 1 );
		m.Roughness = 1;
		m.Metalness = 0;
		m.AmbientOcclusion = 1;
		m.TintMask = 1;
		m.Opacity = 1;
		m.Emission = float3( 0, 0, 0 );
		m.Transmission = 0;
		
		float4 l_0 = Tex2DS( g_tAlbedoMap, g_sSampler0, i.vTextureCoords.xy );
		float4 l_1 = g_vTint;
		float4 l_2 = l_0 * l_1;
		float l_3 = g_flF_ShadowSS_Min;
		float l_4 = g_flF_ShadowSS_Max;
		float l_5 = g_flF_ShadowPwr;
		float3 l_6 = CalculatePositionToCameraDirWs( i.vPositionWithOffsetWs.xyz + g_vHighPrecisionLightingOffsetWs.xyz );
		float3 l_7 = float3( 1, 1, 1 ) - l_6;
		float3 l_8 = pow( 1.0 - dot( normalize( i.vNormalWs ), normalize( l_7 ) ), l_5 );
		float3 l_9 = smoothstep( l_3, l_4, l_8 );
		float4 l_10 = g_vShadowColour;
		float4 l_11 = float4( l_9, 0 ) * l_10;
		float4 l_12 = l_2 * l_11;
		float l_13 = g_flF_HighlightSS_Min;
		float l_14 = g_flF_HighlightSS_Max;
		float l_15 = g_flF_HighlightPwr;
		float3 l_16 = pow( 1.0 - dot( normalize( i.vNormalWs ), normalize( CalculatePositionToCameraDirWs( i.vPositionWithOffsetWs.xyz + g_vHighPrecisionLightingOffsetWs.xyz ) ) ), l_15 );
		float4 l_17 = g_vFresnelColour;
		float4 l_18 = float4( l_16, 0 ) * l_17;
		float4 l_19 = smoothstep( l_13, l_14, l_18 );
		
		m.Albedo = l_12.xyz;
		m.Emission = l_19.xyz;
		m.Opacity = 1;
		m.Roughness = 1;
		m.Metalness = 0;
		m.AmbientOcclusion = 1;
		
		m.AmbientOcclusion = saturate( m.AmbientOcclusion );
		m.Roughness = saturate( m.Roughness );
		m.Metalness = saturate( m.Metalness );
		m.Opacity = saturate( m.Opacity );

		// Result node takes normal as tangent space, convert it to world space now
		m.Normal = TransformNormal( m.Normal, i.vNormalWs, i.vTangentUWs, i.vTangentVWs );

		// for some toolvis shit
		m.WorldTangentU = i.vTangentUWs;
		m.WorldTangentV = i.vTangentVWs;
        m.TextureCoords = i.vTextureCoords.xy;
		
		return ShadingModelStandard::Shade( i, m );
	}
}
