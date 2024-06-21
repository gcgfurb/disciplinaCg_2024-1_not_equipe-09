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

    private readonly float[] _frontFaceVertices = {
      // Position         Texture coordinates
      -0.3005f, -0.3005f, 0.3005f, 0.0f, 0.0f,
      0.3005f, -0.3005f, 0.3005f, 1.0f, 0.0f,
      0.3005f, 0.3005f, 0.3005f, 1.0f, 1.0f,
      -0.3005f, 0.3005f, 0.3005f, 0.0f, 1.0f,
    };
    
    private readonly float[] _backFaceVertices = {
      // Position         Texture coordinates
      -0.3005f, -0.3005f, -0.3005f, 0.0f, 0.0f,
      0.3005f, -0.3005f, -0.3005f, 1.0f, 0.0f,
      0.3005f, 0.3005f, -0.3005f, 1.0f, 1.0f,
      -0.3005f, 0.3005f, -0.3005f, 0.0f, 1.0f
    };

    private readonly float[] _topFaceVertices = {
      // Position         Texture coordinates
      -0.3005f, 0.3005f, 0.3005f, 0.0f, 0.0f,
      0.3005f, 0.3005f, -0.3005f, 1.0f, 1.0f,
      0.3005f, 0.3005f, 0.3005f, 1.0f, 0.0f,
      -0.3005f, 0.3005f, -0.3005f, 0.0f, 1.0f
    };

    private readonly float[] _bottomFaceVertices = {
      // Position         Texture coordinates
      0.3005f, -0.3005f, -0.3005f, 0.0f, 0.0f,
      -0.3005f, -0.3005f, 0.3005f, 1.0f, 1.0f,
      -0.3005f, -0.3005f, -0.3005f, 1.0f, 0.0f,
      0.3005f, -0.3005f, 0.3005f, 0.0f, 1.0f
    };

    public readonly float[] _rightFaceVertices = {
      // Position         Texture coordinates
      0.3005f, -0.3005f, -0.3005f, 0.0f, 0.0f,
      0.3005f, 0.3005f, 0.3005f, 1.0f, 1.0f,
      0.3005f, -0.3005f, 0.3005f, 1.0f, 0.0f,
      0.3005f, 0.3005f, -0.3005f, 0.0f, 1.0f
    };

    public readonly float[] _leftFaceVertices = {
      // Position         Texture coordinates
      -0.3005f, -0.3005f, 0.3005f, 0.0f, 0.0f,
      -0.3005f, 0.3005f, -0.3005f, 1.0f, 1.0f,
      -0.3005f, -0.3005f, -0.3005f, 1.0f, 0.0f,
      -0.3005f, 0.3005f, 0.3005f, 0.0f, 1.0f
    };

    private readonly uint[] _frontFaceIndices =
    {
      1, 2, 3,
      0, 1, 3,
    };

    private readonly uint[] _backFaceIndices =
    {
      1, 2, 3,
      0, 1, 3
    };

    private readonly uint[] _topFaceIndices =
    {
      3, 0, 1,
      0, 2, 1
    };

    private readonly uint[] _bottomFaceIndices =
    {
      3, 0, 1,
      0, 2, 1
    };

    private readonly uint[] _rightFaceIndices =
    {
      1, 0, 2,
      3, 0, 1
    };

    private readonly uint[] _leftFaceIndices =
    {
      1, 0, 2,
      3, 0, 1
    };

    public Cubo(Objeto _paiRef, ref char _rotulo, int primitiveSize) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.TriangleFan;
      PrimitivaTamanho = primitiveSize;

      vertices = new Ponto4D[]
      {
        new Ponto4D(-0.3, -0.3,  0.3),
        new Ponto4D( 0.3, -0.3,  0.3),
        new Ponto4D( 0.3,  0.3,  0.3),
        new Ponto4D(-0.3,  0.3,  0.3),
        new Ponto4D(-0.3, -0.3, -0.3),
        new Ponto4D( 0.3, -0.3, -0.3),
        new Ponto4D( 0.3,  0.3, -0.3),
        new Ponto4D(-0.3,  0.3, -0.3)
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

    public Ponto4D[] getVertices()
    {
      return this.vertices;
    }

    public int ColorToRgba32(Color c)
    {
      return (int)((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
    }

    public List<Texture> GetCubeTextures()
    {
      List<Texture> textures = new List<Texture>();
      Texture alexandreTexture = Texture.LoadFromFile("assets/alexandre.png");
      Texture brunoTexture = Texture.LoadFromFile("assets/bruno.png");
      Texture joshuaTexture = Texture.LoadFromFile("assets/joshua.png");
      Texture leonardoTexture = Texture.LoadFromFile("assets/leonardo.png");
      Texture lorhanTexture = Texture.LoadFromFile("assets/lorhan.png");
      Texture containerTexture = Texture.LoadFromFile("assets/container.png");

      textures.AddRange(new List<Texture> {
        alexandreTexture,
        brunoTexture,
        joshuaTexture,
        leonardoTexture,
        lorhanTexture,
        containerTexture
      });
      return textures;
    }

    public List<float[]> GetFaceVertices()
    {
      List<float[]> faceVertices = new List<float[]> {
        _frontFaceVertices,
        _backFaceVertices,
        _topFaceVertices,
        _bottomFaceVertices,
        _rightFaceVertices,
        _leftFaceVertices
      };
      return faceVertices;
    }

    public List<uint[]> GetFaceIndices()
    {
      List<uint[]> faceIndices = new List<uint[]> {
        _frontFaceIndices,
        _backFaceIndices,
        _topFaceIndices,
        _bottomFaceIndices,
        _rightFaceIndices,
        _leftFaceIndices
      };
      return faceIndices;
    }

    public float[] GetFaceVerticesByName(string faceName)
    {
      if (faceName == "front")
      {
        return _frontFaceVertices;
      }
      else if (faceName == "back")
      {
        return _backFaceVertices;
      }
      else if (faceName == "top")
      {
        return _topFaceVertices;
      }
      else if (faceName == "bottom")
      {
        return _bottomFaceVertices;
      }
      else if (faceName == "right")
      {
        return _rightFaceVertices;
      }
      else if (faceName == "left")
      {
        return _leftFaceVertices;
      }
      return null;
    }

    public uint[] GetFaceIndicesByName(string faceName)
    {
      if (faceName == "front")
      {
        return _frontFaceIndices;
      }
      else if (faceName == "back")
      {
        return _backFaceIndices;
      }
      else if (faceName == "top")
      {
        return _topFaceIndices;
      }
      else if (faceName == "bottom")
      {
        return _bottomFaceIndices;
      }
      else if (faceName == "right")
      {
        return _rightFaceIndices;
      }
      else if (faceName == "left")
      {
        return _leftFaceIndices;
      }
      return null;
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
