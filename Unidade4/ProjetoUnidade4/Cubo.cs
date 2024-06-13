//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    Ponto4D[] vertices;
    // int[] indices;
    // Vector3[] normals;
    // int[] colors;

    public Cubo(Objeto _paiRef, ref char _rotulo, int primitiveSize) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.TriangleFan;
      PrimitivaTamanho = primitiveSize;

      vertices = new Ponto4D[]
      {
        new Ponto4D(-1.0f, -1.0f,  1.0f),
        new Ponto4D( 1.0f, -1.0f,  1.0f),
        new Ponto4D( 1.0f,  1.0f,  1.0f),
        new Ponto4D(-1.0f,  1.0f,  1.0f),
        new Ponto4D(-1.0f, -1.0f, -1.0f),
        new Ponto4D( 1.0f, -1.0f, -1.0f),
        new Ponto4D( 1.0f,  1.0f, -1.0f),
        new Ponto4D(-1.0f,  1.0f, -1.0f)
      };

      // // 0, 1, 2, 3 Face da frente
      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[1]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[3]);

      // // 3, 2, 6, 7 Face de cima
      base.PontosAdicionar(vertices[3]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[6]);
      base.PontosAdicionar(vertices[7]);

      // Ajuste de rendericazao da parte de cima do Cubo.
      base.PontosAdicionar(vertices[3]);
      base.PontosAdicionar(vertices[2]);
      base.PontosAdicionar(vertices[7]);
      base.PontosAdicionar(vertices[6]);

      // // 4, 7, 6, 5 Face do fundo
      base.PontosAdicionar(vertices[4]);
      base.PontosAdicionar(vertices[7]);
      base.PontosAdicionar(vertices[6]);
      base.PontosAdicionar(vertices[5]);
      
      // // 0, 3, 7, 4 Face direita
      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[3]);
      base.PontosAdicionar(vertices[7]);
      base.PontosAdicionar(vertices[4]);

      // // 0, 4, 5, 1 Face de baixo
      base.PontosAdicionar(vertices[0]);
      base.PontosAdicionar(vertices[4]);
      base.PontosAdicionar(vertices[5]);
      base.PontosAdicionar(vertices[1]);

      // // 1, 5, 6, 2 Face direita
      base.PontosAdicionar(vertices[1]);
      base.PontosAdicionar(vertices[5]);
      base.PontosAdicionar(vertices[6]);
      base.PontosAdicionar(vertices[2]);

      Atualizar();
    }

    private void Atualizar()
    {
      base.ObjetoAtualizar();
    }

    public List<Texture> GetCubeTextures()
    {
      List<Texture> textures = new List<Texture>();
      Texture alexandreTexture = Texture.LoadFromFile("assets/alexandre.jpg");
      Texture brunoTexture = Texture.LoadFromFile("assets/bruno.jpg");
      Texture joshuaTexture = Texture.LoadFromFile("assets/joshua.jpg");
      Texture leonardoTexture = Texture.LoadFromFile("assets/leonardo.jpg");
      Texture lorhanTexture = Texture.LoadFromFile("assets/lorhan.jpg");

      textures.AddRange(new List<Texture> {
        alexandreTexture,
        brunoTexture,
        joshuaTexture,
        leonardoTexture,
        lorhanTexture
      });
      return textures;
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
