Shader "Custom/VertexColorMap" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Palette("Palette", 2D) = "white" {}
		_maxValue("Max Value", Float) = 0.5
		_minValue("Min Value", Float) = 0.1
		_psize("Pixel Size", Float) = 0.1
	}
	
    SubShader {
    Pass {
        LOD 200
         
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        uniform float _psize;
		uniform float _minValue;
		uniform float _maxValue;
 
		uniform sampler2D _Palette;

        struct VertexInput
        {
            fixed4 v : POSITION;
        };

        struct VertexOutput
        {
            fixed4 pos : POSITION;
            fixed4 col : COLOR;
            float size : PSIZE;
			fixed i : TEXCOORD;
        };
         
        VertexOutput vert(VertexInput v) 
        {
            VertexOutput o;

			o.i = (v.v.z - _minValue) / (_maxValue - _minValue);
			o.col = fixed4(0, 0, 0, 0);
			o.pos = mul(UNITY_MATRIX_MVP, v.v);
            o.size = _psize;
             
            return o;
        };
         
        fixed4 frag(VertexOutput o) : COLOR
        {
			o.col = tex2D(_Palette, fixed2(o.i, 0.0f));
            return o.col;
        };
 
        ENDCG
        }
    }

	FallBack "Diffuse"
}
