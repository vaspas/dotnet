
struct PsIn
{
	float4 Position : SV_Position;
	float2 TexCoord : TexCoord;
};


PsIn VS(uint VertexID : SV_VertexID)
{
	// Produce a fullscreen triangle
	PsIn Out;
	Out.Position.x = (VertexID == 0)? 3.0f : -1.0f;
	Out.Position.y = (VertexID == 2)? 3.0f : -1.0f;
	Out.Position.z = 0.5f;
	Out.Position.w = 1.0f;
	Out.TexCoord = Out.Position.xy * float2(0.5f, -0.5f) + 0.5f;

	return Out;
}

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

Texture2D BackBuffer:register(t0);
Texture2D GeometryBuffer:register(t1);


SamplerState Linear;

float4 PS(PsIn In) : SV_Target
{
	float4 offset = BackBuffer.Sample(Linear, In.TexCoord);
	float4 offset1 = BackBuffer.Sample(Linear, In.TexCoord,float2(-1,  0));
	float4 offset2 = BackBuffer.Sample(Linear, In.TexCoord,float2(1,  0));
	//float4 offset3 = BackBuffer.Sample(Linear, In.TexCoord,float2(0,  -1));
	//float4 offset4 = BackBuffer.Sample(Linear, In.TexCoord,float2(0,  1));

	return float4(((offset+offset1+offset2)/3).xyz,1);
}

/*float4 PS(PsIn In) : SV_Target
{

	float2 offset = GeometryBuffer.Sample(Linear, In.TexCoord);

	return float4(offset,0,1);

	// Check geometry buffer for an edge cutting through the pixel.
	[flatten]
	if (min(abs(offset.x), abs(offset.y)) >= 0.5f)
	{
		// If no edge was found we look in neighboring pixels' geometry information. This is necessary because
		// relevant geometry information may only be available on one side of an edge, such as on silhouette edges,
		// where a background pixel adjacent to the edge will have the background's geometry information, and not
		// the foreground's geometric edge that we need to antialias against. Doing this step covers up gaps in the
		// geometry information.

		offset = 0.5f;

		// We only need to check the component on neighbor samples that point towards us
		float offset_x0 = GeometryBuffer.Sample(Linear, In.TexCoord, int2(-1,  0)).x;
		float offset_x1 = GeometryBuffer.Sample(Linear, In.TexCoord, int2( 1,  0)).x;
		float offset_y0 = GeometryBuffer.Sample(Linear, In.TexCoord, int2( 0, -1)).y;
		float offset_y1 = GeometryBuffer.Sample(Linear, In.TexCoord, int2( 0,  1)).y;
		
		// Check range of neighbor pixels' distance and use if edge cuts this pixel.
		if (abs(offset_x0 - 0.75f) < 0.25f) offset = float2(offset_x0 - 1.0f, 0.5f); // Left  x-offset [ 0.5 ..  1.0] cuts this pixel
		if (abs(offset_x1 + 0.75f) < 0.25f) offset = float2(offset_x1 + 1.0f, 0.5f); // Right x-offset [-1.0 .. -0.5] cuts this pixel
		if (abs(offset_y0 - 0.75f) < 0.25f) offset = float2(0.5f, offset_y0 - 1.0f); // Up    y-offset [ 0.5 ..  1.0] cuts this pixel
		if (abs(offset_y1 + 0.75f) < 0.25f) offset = float2(0.5f, offset_y1 + 1.0f); // Down  y-offset [-1.0 .. -0.5] cuts this pixel

	//return float4(0,1,0,1);	
	}
	
	//return float4(1,1,0,1);

	// Convert distance to texture coordinate shift
	float2 off = (offset >= float2(0, 0))? float2(0.5f, 0.5f) : float2(-0.5f, -0.5f);
	offset = off - offset;

	// Blend pixel with neighbor pixel using texture filtering and shifting the coordinate appropriately.
	return BackBuffer.Sample(Linear, In.TexCoord + offset * float2(1/Width,1/Height));
}*/

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}
