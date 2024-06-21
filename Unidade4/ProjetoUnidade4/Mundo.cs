#define CG_DEBUG
#define CG_Gizmo      
#define CG_OpenGL      
// #define CG_OpenTK
// #define CG_DirectX      
// #define CG_Privado      

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using System.Collections.Generic;

//FIXME: padrão Singleton

namespace gcgcg
{
  public class Mundo : GameWindow
  {
    private static Objeto mundo = null;
    private char rotuloNovo = '?';
    private Objeto _point;
    private Objeto _biggerCube;
    //private Objeto _smallerCube;
    private readonly float[] _sruEixos =
    [
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f  /* Z+ */
    ];

    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;

    private List<Texture> _textures;
    private List<float[]> _faceVertices;
    private List<uint[]> _faceIndices;
    private readonly List<Ponto4D> _rectangleFirstPoints = new List<Ponto4D> {
      new Ponto4D(-0.3, -0.3, 0.3),
      new Ponto4D(-0.3, -0.3, -0.3),
      new Ponto4D(-0.3, 0.3, -0.3),
      new Ponto4D(-0.3, -0.3, -0.3),
      new Ponto4D(0.3, -0.3, -0.3),
      new Ponto4D(-0.3, -0.3, -0.3)
    };
    private readonly List<Ponto4D> _rectangleSecondPoints = new List<Ponto4D> {
      new Ponto4D(0.3, 0.3, 0.3),
      new Ponto4D(0.3, 0.3, -0.3),
      new Ponto4D(0.3, 0.3, 0.3),
      new Ponto4D(0.3, -0.3, 0.3),
      new Ponto4D(0.3, 0.3, 0.3),
      new Ponto4D(-0.3, 0.3, 0.3)
    };

    private int _vertexBufferObject_texture;
    private int _vertexArrayObject_texture;
    private int _elementBufferObject_texture;
    //private Texture _texture;

    private Shader _shaderBranca;
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderCiano;
    private Shader _shaderMagenta;
    private Shader _shaderAmarela;
    private Shader _shaderWithTextures;
    private Camera _camera;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloNovo); //padrão Singleton
    }

    protected override void OnLoad()
    {
      base.OnLoad();

      //Utilitario.Diretivas();
#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

      GL.Enable(EnableCap.DepthTest);       // Ativar teste de profundidade
      //GL.Enable(EnableCap.CullFace);     // Desenha os dois lados da face
      //GL.FrontFace(FrontFaceDirection.Cw);
      //GL.CullFace(CullFaceMode.FrontAndBack);

      #region Cores
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      #endregion

      #region Eixos: SRU  
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);
      #endregion

      #region Object: Point  
      _point = new Ponto(mundo, ref rotuloNovo, new Ponto4D(2.0, 0.0));
      _point.PrimitivaTipo = PrimitiveType.Points;
      _point.PrimitivaTamanho = 5;
      #endregion

      #region Object: Bigger Cube
      _biggerCube = new Cubo(mundo, ref rotuloNovo, 10);
      _biggerCube.shaderCor = _shaderAmarela;
      _textures = ((Cubo) _biggerCube).GetCubeTextures();
      _faceVertices = ((Cubo) _biggerCube).GetFaceVertices();
      _faceIndices = ((Cubo) _biggerCube).GetFaceIndices();

      OnLoadUseTextures();
      #endregion

      /*#region Object: Smaller Cube
      _smallerCube = new Cubo(mundo, ref rotuloNovo, 2);
      #endregion*/
      // objetoSelecionado.MatrizEscalaXYZ(0.2, 0.2, 0.2);

      _camera = new Camera(Vector3.UnitZ * 1.5f, ClientSize.X / (float) ClientSize.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      mundo.Desenhar(new Transformacao4D(), _camera);

      OnRenderFrameUseTextures();

#if CG_Gizmo      
      Gizmo_Sru3D();
#endif
      SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      // ☞ 396c2670-8ce0-4aff-86da-0f58cd8dcfdc   TODO: forma otimizada para teclado.
      #region Teclado
      var estadoTeclado = KeyboardState;
      if (estadoTeclado.IsKeyDown(Keys.Escape))
        Close();
      if (estadoTeclado.IsKeyPressed(Keys.Space))
      {
        if (_biggerCube == null)
          _biggerCube = mundo;
        _biggerCube.shaderCor = _shaderBranca;
        _biggerCube = mundo.GrafocenaBuscaProximo(_biggerCube);
        _biggerCube.shaderCor = _shaderAmarela;
      }
      if (estadoTeclado.IsKeyPressed(Keys.G))
        mundo.GrafocenaImprimir("");
      if (estadoTeclado.IsKeyPressed(Keys.P) && _biggerCube != null)
        Console.WriteLine(_biggerCube.ToString());
      if (estadoTeclado.IsKeyPressed(Keys.M) && _biggerCube != null)
        _biggerCube.MatrizImprimir();
      if (estadoTeclado.IsKeyPressed(Keys.I) && _biggerCube != null)
        _biggerCube.MatrizAtribuirIdentidade();
      if (estadoTeclado.IsKeyPressed(Keys.Left) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(-0.05, 0, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Right) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0.05, 0, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Up) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, 0.05, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Down) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, -0.05, 0);
      if (estadoTeclado.IsKeyPressed(Keys.O) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, 0, 0.05);
      if (estadoTeclado.IsKeyPressed(Keys.L) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, 0, -0.05);
      if (estadoTeclado.IsKeyPressed(Keys.PageUp) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZ(2, 2, 2);
      if (estadoTeclado.IsKeyPressed(Keys.PageDown) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZ(0.5, 0.5, 0.5);
      if (estadoTeclado.IsKeyPressed(Keys.Home) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZBBox(0.5, 0.5, 0.5);
      if (estadoTeclado.IsKeyPressed(Keys.End) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZBBox(2, 2, 2);
      if (estadoTeclado.IsKeyPressed(Keys.D1) && _biggerCube != null)
        _biggerCube.MatrizRotacao(10);
      if (estadoTeclado.IsKeyPressed(Keys.D2) && _biggerCube != null)
        _biggerCube.MatrizRotacao(-10);
      if (estadoTeclado.IsKeyPressed(Keys.D3) && _biggerCube != null)
        _biggerCube.MatrizRotacaoZBBox(10);
      if (estadoTeclado.IsKeyPressed(Keys.D4) && _biggerCube != null)
        _biggerCube.MatrizRotacaoZBBox(-10);

      const float cameraSpeed = 1.5f;
      if (estadoTeclado.IsKeyDown(Keys.Z))
        _camera.Position = Vector3.UnitZ * 5;
      if (estadoTeclado.IsKeyDown(Keys.W))
        _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
      if (estadoTeclado.IsKeyDown(Keys.S))
        _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
      if (estadoTeclado.IsKeyDown(Keys.A))
        _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
      if (estadoTeclado.IsKeyDown(Keys.D))
        _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
      if (estadoTeclado.IsKeyDown(Keys.RightShift))
        _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
      if (estadoTeclado.IsKeyDown(Keys.LeftShift))
        _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
      if (estadoTeclado.IsKeyDown(Keys.D9))
        _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
      if (estadoTeclado.IsKeyDown(Keys.D0))
        _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down

      #endregion

      #region  Mouse

      if (MouseState.IsButtonPressed(MouseButton.Left))
      {
        Console.WriteLine("MouseState.IsButtonPressed(MouseButton.Left)");
        Console.WriteLine("__ Valores do Espaço de Tela");
        Console.WriteLine("Vector2 mousePosition: " + MousePosition);
        Console.WriteLine("Vector2i windowSize: " + ClientSize);
      }
      if (MouseState.IsButtonDown(MouseButton.Right) && _biggerCube != null)
      {
        Console.WriteLine("MouseState.IsButtonDown(MouseButton.Right)");

        int janelaLargura = ClientSize.X;
        int janelaAltura = ClientSize.Y;
        Ponto4D mousePonto = new Ponto4D(MousePosition.X, MousePosition.Y);
        Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

        _biggerCube.PontosAlterar(sruPonto, 0);
      }
      if (MouseState.IsButtonReleased(MouseButton.Right))
      {
        Console.WriteLine("MouseState.IsButtonReleased(MouseButton.Right)");
      }

      #endregion

    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);

#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif
      GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    }

    protected override void OnUnload()
    {
      mundo.OnUnload();

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

      GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

      GL.DeleteProgram(_shaderBranca.Handle);
      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      GL.DeleteProgram(_shaderCiano.Handle);
      GL.DeleteProgram(_shaderMagenta.Handle);
      GL.DeleteProgram(_shaderAmarela.Handle);
      GL.DeleteProgram(_shaderWithTextures.Handle);

      base.OnUnload();
    }

    protected void OnLoadUseTextures()
    {
      for (int i = 0; i < _textures.Count; i++)
      {
        GL.Enable(EnableCap.Texture2D);
        _vertexArrayObject_texture = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject_texture);

        _vertexBufferObject_texture = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_texture);
        GL.BufferData(BufferTarget.ArrayBuffer, _faceVertices[i].Length * sizeof(float), _faceVertices[i], BufferUsageHint.StaticDraw);

        _elementBufferObject_texture = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject_texture);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _faceIndices[i].Length * sizeof(uint), _faceIndices[i], BufferUsageHint.StaticDraw);

        _shaderWithTextures = new Shader("Shaders/shaderWithTextures.vert", "Shaders/shaderWithTextures.frag");
        _shaderWithTextures.Use();

        var vertexLocation = _shaderWithTextures.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        var texCoordLocation = _shaderWithTextures.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _textures[i].Use(TextureUnit.Texture0);

        Retangulo faceRectangle = new Retangulo(_biggerCube, ref rotuloNovo, _rectangleFirstPoints[i], _rectangleSecondPoints[i], true);
        faceRectangle.shaderCor = _shaderWithTextures;
      }
    }

    protected void OnRenderFrameUseTextures()
    {
      for (int i = 0; i < _textures.Count; i++)
      {
        GL.BindVertexArray(_vertexArrayObject_texture);
        _textures[i].Use(TextureUnit.Texture0);
        _shaderWithTextures.Use();

        GL.DrawElements(PrimitiveType.Triangles, _faceIndices[i].Length, DrawElementsType.UnsignedInt, 0);
      }
    }

#if CG_Gizmo
    private void Gizmo_Sru3D()
    {
#if CG_OpenGL && !CG_DirectX
      var model = Matrix4.Identity;
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      _shaderVermelha.SetMatrix4("model", model);
      _shaderVermelha.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVermelha.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.SetMatrix4("model", model);
      _shaderVerde.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVerde.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.SetMatrix4("model", model);
      _shaderAzul.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderAzul.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif    

  }
}
