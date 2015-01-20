Texture2D picture;
SamplerState pictureSampler;
							
struct VS_IN
{
	float2 TexCoord		: TEXCOORD;
	float2 TexCoordSize	: TEXCOORDSIZE;
	float4 Color		: COLOR;	
	float2 TopLeft		: TOPLEFT;
	float2 TopRight		: TOPRIGHT;
	float2 BottomLeft	: BOTTOMLEFT;
	float2 BottomRight	: BOTTOMRIGHT;
};


struct PS_IN
{
	float2 TexCoord : TEXCOORD;
	float4 Color	: COLOR;
	float4 Position : SV_POSITION;
};


cbuffer buffer:register(b0)
{

float Width;
float Height;
float XFrom;
float XTo;
float YFrom;
float YTo;
float r1;
float r2;
float4x4 World;
float4x4 Translate;

}

[maxvertexcount(4)]
void GS( point VS_IN input[1], inout TriangleStream<PS_IN> TriStream )
{
	/*

	0 -- 1
	|  / |
	| /  |
	2 -- 3

	*/

	PS_IN v;
	v.Color = input[0].Color;
	v.Position = float4(input[0].TopLeft, 0, 1);
	v.TexCoord = input[0].TexCoord;
	TriStream.Append(v);

	v.Position = float4(input[0].TopRight, 0, 1);
	v.TexCoord.x += input[0].TexCoordSize.x;
	TriStream.Append(v);

	v.Position = float4(input[0].BottomLeft, 0, 1);
	v.TexCoord.x = input[0].TexCoord.x;
	v.TexCoord.y += input[0].TexCoordSize.y;
	TriStream.Append(v);

	v.Position = float4(input[0].BottomRight, 0, 1);
	v.TexCoord.x += input[0].TexCoordSize.x;
	TriStream.Append(v);

	TriStream.RestartStrip();
}

VS_IN VS(VS_IN vs_in){

	VS_IN vs_out=(VS_IN)0;

	vs_out.TexCoord=vs_in.TexCoord;
	vs_out.Color=vs_in.Color;
	vs_out.TexCoordSize	=vs_in.TexCoordSize;
	
	vs_out.TopLeft= mul(mul(float4(vs_in.TopLeft,0,1),Translate),World).xy;
	vs_out.TopRight	= mul(mul(float4(vs_in.TopRight,0,1),Translate),World).xy;
	vs_out.BottomLeft= mul(mul(float4(vs_in.BottomLeft,0,1),Translate),World).xy;
	vs_out.BottomRight= mul(mul(float4(vs_in.BottomRight,0,1),Translate),World).xy;

	return vs_out;
}			

float4 PS(PS_IN ps_in) : SV_TARGET 
{
if(ps_in.Position.x<XFrom || ps_in.Position.x>XTo 
|| ps_in.Position.y<YFrom || ps_in.Position.y>YTo)
	return float4(0,0,0,0);

	return picture.Sample(pictureSampler, ps_in.TexCoord)* ps_in.Color;
}


technique10 Render 
{
	pass p0 
	{	
		SetVertexShader		( CompileShader( vs_4_0 , VS() ) );
		SetGeometryShader	( CompileShader( gs_4_0 , GS() ) );
		SetPixelShader		( CompileShader( ps_4_0 , PS() ) );
	}
}