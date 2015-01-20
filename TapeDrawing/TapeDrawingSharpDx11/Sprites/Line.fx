// -----------------------------------------------------------------------------
// Original code from SlimDX project.
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/


struct VS_IN
{
	float4 pos : POSITION;
	float4 col : COLOR;
};

struct GS_IN
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
	float4 linepoints : POSITION;
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

cbuffer lineparams:register(b1)
{

float LineWidth;
float dash1;
float dash2;
float dash3;
float dash4;
float paramr1;
float paramr2;
float paramr3;

}


GS_IN VS( VS_IN input )
{
	GS_IN output = (GS_IN)0;
	
	output.pos = mul(input.pos, World);
	output.col = input.col;
	
	return output;
}

void buildRect(in float2 p0,in float2 p1,in float4 col,inout TriangleStream<PS_IN> OutputStream)
{
PS_IN output = (PS_IN)0;
	
	float2 normal=(float2)0;

	if(Width>0 || Height>0)
	{
		
	float val=LineWidth;

	float2 crv=(p1-p0)*float2(Width,Height);
	
	normal=normalize(float2(-crv.y,crv.x))*float2(val/Width,val/Height);
	}
	
	float2 a=p0-normal;
	float2 b=p0+normal;
	float2 c=p1-normal;
	float2 d=p1+normal;
    
	output.col = col;
	output.linepoints=float4((p0.x+1)*Width/2,Height*(1-p0.y)/2, (p1.x+1)*Width/2,Height*(1-p1.y)/2);

    output.pos = float4(a.xy,0.5,1);	
    OutputStream.Append( output );

	output.pos = float4(b.xy,0.5,1);
	OutputStream.Append( output );
    
	output.pos = float4(c.xy,0.5,1);
	OutputStream.Append( output );
	
    OutputStream.RestartStrip();


	output.pos = float4(c.xy,0.5,1);
	OutputStream.Append( output );

	output.pos = float4(b.xy,0.5,1);
	OutputStream.Append( output );

	output.pos = float4(d.xy,0.5,1);
	OutputStream.Append( output );
	
	
    OutputStream.RestartStrip();
}


[maxvertexcount(6)]
void GS( line GS_IN input[2], inout TriangleStream<PS_IN> OutputStream )
{			
	buildRect(input[0].pos.xy,input[1].pos.xy,input[0].col,OutputStream);
}


float4 PS( PS_IN input ) : SV_Target
{

if(input.pos.x<XFrom || input.pos.x>XTo 
|| input.pos.y<YFrom || input.pos.y>YTo)
	return float4(0,0,0,0);

float2 p0=input.linepoints.xy;
float2 p1=input.linepoints.zw;

float dsum=dash1+dash2+dash3+dash4;
if(dsum<=0)
	return input.col;


float ab=length(p1-p0);
float c=length(input.pos.xy-p0);
float d=length(p1-input.pos.xy);

float u=0;
if(ab!=0)
	u=(c*c-d*d+ab*ab)/(2*ab);
//float u2=ab-u;
//float v=sqrt(c*c- u*u);


float ldv=(u%(LineWidth*dsum))/(LineWidth*dsum);

if(ldv<dash1/dsum)
	return input.col;
if(ldv<(dash1+dash2)/dsum)
	return float4(0,0,0,0);
if(ldv<(dash1+dash2+dash3)/dsum)
	return input.col;

return float4(0,0,0,0);
}

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetGeometryShader( CompileShader( gs_4_0, GS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}