
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
	float g_flNoiseAngle < UiGroup( ",0/,0/0" ); Default1( 1 ); Range1( -5, 1000 ); >;
	float g_flCellDensity < UiGroup( ",0/,0/0" ); Default1( 0 ); Range1( -5, 1000 ); >;
	float g_flNoiseOpacity < UiGroup( ",0/,0/0" ); Default1( 1 ); Range1( 0, 1 ); >;
	float4 g_vTint < UiType( Color ); UiGroup( ",0/,0/0" ); Default4( 1.00, 1.00, 1.00, 1.00 ); >;
	float g_flShadowDark < UiGroup( ",0/,0/0" ); Default1( 0 ); Range1( 0, 1 ); >;
	float g_flF_ShadowSS_Min < UiGroup( ",0/,0/0" ); Default1( 0 ); Range1( 0, 1 ); >;
	float g_flF_ShadowSS_Max < UiGroup( ",0/,0/0" ); Default1( 0.8929459 ); Range1( 0, 1 ); >;
	float g_flF_ShadowPwr < UiGroup( ",0/,0/0" ); Default1( 0.43853116 ); Range1( -1, 5 ); >;
	float3 g_vDirectionMulti < UiGroup( ",0/,0/0" ); Default3( 0,0,0 ); >;
	float g_flShadowHighlightSS_Min < UiGroup( ",0/,0/0" ); Default1( 0 ); Range1( 0, 1 ); >;
	float g_flShadowHighlightSS_Max < UiGroup( ",0/,0/0" ); Default1( 0.24161862 ); Range1( 0, 1 ); >;
	float g_flShadowHighlightPwr < UiGroup( ",0/,0/0" ); Default1( 0.8659146 ); Range1( -1, 2 ); >;
	float g_flF_HighlightSS_Min < UiGroup( ",0/,0/0" ); Default1( 0 ); Range1( 0, 5 ); >;
	float g_flF_HighlightSS_Max < UiGroup( ",0/,0/0" ); Default1( 1 ); Range1( 0, 5 ); >;
	float g_flF_HighlightPwr < UiGroup( ",0/,0/0" ); Default1( 1.7116337 ); Range1( -1, 5 ); >;
	float4 g_vFresnelColour < UiType( Color ); UiGroup( ",0/,0/0" ); Default4( 1.00, 1.00, 1.00, 1.00 ); >;
	float g_flEmmissionPower < UiGroup( ",0/,0/0" ); Default1( 0.5503927 ); Range1( 0, 1 ); >;
	
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
		
		float l_0 = g_flNoiseAngle;
		float l_1 = 0.5 * l_0;
		float l_2 = g_flCellDensity;
		float l_3 = 0.5 * l_2;
		float l_4 = VoronoiNoise( i.vTextureCoords.xy, l_1, l_3 );
		float l_5 = VoronoiNoise( i.vTextureCoords.xy, l_0, l_2 );
		float l_6 = l_4 * l_5;
		float l_7 = g_flNoiseOpacity;
		float l_8 = l_6 * l_7;
		float l_9 = 1 - l_8;
		float4 l_10 = Tex2DS( g_tAlbedoMap, g_sSampler0, i.vTextureCoords.xy );
		float4 l_11 = g_vTint;
		float4 l_12 = l_10 * l_11;
		float4 l_13 = float4( l_9, l_9, l_9, l_9 ) * l_12;
		float l_14 = g_flShadowDark;
		float l_15 = g_flF_ShadowSS_Min;
		float l_16 = g_flF_ShadowSS_Max;
		float l_17 = g_flF_ShadowPwr;
		float3 l_18 = CalculatePositionToCameraDirWs( i.vPositionWithOffsetWs.xyz + g_vHighPrecisionLightingOffsetWs.xyz );
		float l_19 = dot( i.vNormalWs, l_18 );
		float3 l_20 = g_vDirectionMulti;
		float3 l_21 = float3( l_19, l_19, l_19 ) * l_20;
		float3 l_22 = float3( 1, 1, 1 ) - l_21;
		float3 l_23 = pow( 1.0 - dot( normalize( i.vNormalWs ), normalize( l_22 ) ), l_17 );
		float3 l_24 = smoothstep( l_15, l_16, l_23 );
		float3 l_25 = float3( l_14, l_14, l_14 ) * l_24;
		float l_26 = g_flShadowDark;
		float l_27 = g_flShadowHighlightSS_Min;
		float l_28 = g_flShadowHighlightSS_Max;
		float l_29 = g_flShadowHighlightPwr;
		float3 l_30 = pow( 1.0 - dot( normalize( i.vNormalWs ), normalize( l_22 ) ), l_29 );
		float3 l_31 = smoothstep( l_27, l_28, l_30 );
		float3 l_32 = float3( l_26, l_26, l_26 ) * l_31;
		float3 l_33 = l_25 - l_32;
		float4 l_34 = l_13 * float4( l_33, 0 );
		float l_35 = g_flF_HighlightSS_Min;
		float l_36 = g_flF_HighlightSS_Max;
		float l_37 = g_flF_HighlightPwr;
		float3 l_38 = pow( 1.0 - dot( normalize( i.vNormalWs ), normalize( l_21 ) ), l_37 );
		float3 l_39 = smoothstep( l_35, l_36, l_38 );
		float4 l_40 = g_vFresnelColour;
		float4 l_41 = float4( l_39, 0 ) * l_40;
		float4 l_42 = l_34 + l_41;
		float l_43 = g_flEmmissionPower;
		float4 l_44 = l_42 * float4( l_43, l_43, l_43, l_43 );
		
		m.Albedo = l_13.xyz;
		m.Emission = l_44.xyz;
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
