
struct VS_IN
{
	float4 Position0 : POSITIONA;
	float4 Position1 : POSITIONB;
};

struct PS_IN
{
	float4 Position : SV_Position;

	// The parameters are constant across the line so use the nointerpolation attribute.
	// This is not necessarily required, but using this we can make the vertex shader slightly shorter.
	nointerpolation float4 KMF : KMF;
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

PS_IN VS(VS_IN In)
{
	PS_IN Out=(PS_IN)0;

	float2 dir = In.Position1;

	// Select between mostly horizontal or vertical
	bool x_gt_y = (abs(dir.x) > abs(dir.y));

	float kx=(dir.x>0?1:-1)*(x_gt_y?0:0.5);
	float ky=(dir.y>0?1:-1)*(x_gt_y?0.5:0);
	// Pass down the screen-space line equation
	if (x_gt_y)
	{
		float k = dir.y / dir.x;
		Out.KMF.xy = float2(k, -1);
	}
	else
	{
		float k = dir.x / dir.y;
		Out.KMF.xy = float2(-1, k);
	}
		
	Out.KMF.z = -dot(In.Position0.xy+float2(kx,ky), Out.KMF.xy);
	Out.KMF.w = asfloat(x_gt_y);

	Out.Position = mul(In.Position0+float4(kx,ky,0,0), World);

	return Out;
}

Texture2D BackBuffer;
SamplerState Filter;

float4 PS(PS_IN In) : SV_Target
{

	// Compute the difference between geometric line and sample position
	float diff = dot(In.KMF.xy, In.Position.xy) + In.KMF.z;

	// Compute the coverage of the neighboring surface
	float coverage = 0.5f - abs(diff);
	float2 offset = 0;
	
	if (coverage > 0)
	{
		// Select direction to sample a neighbor pixel
		float off = (diff >= 0)? 1 : -1;
		if (asuint(In.KMF.w))
			offset.y = off;
		else
			offset.x = off;
	}

	
	// Blend pixel with neighbor pixel using texture filtering and shifting the coordinate appropriately.
	return BackBuffer.Sample(Filter, (In.Position.xy + coverage * offset.xy) * float2(1/Width,1/Height));
}

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}
