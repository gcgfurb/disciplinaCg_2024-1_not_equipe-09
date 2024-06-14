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
    private Objeto point;
    private Objeto biggerCube;
    private Objeto smallerCube;
    private List<Texture> textures;
    private readonly float[] _sruEixos =
    {
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */,
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */,
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f, /* Z+ */
    };

    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;

    private Texture _texture;

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
      GL.Enable(EnableCap.CullFace);     // Desenha os dois lados da face
      //GL.FrontFace(FrontFaceDirection.Cw);
      //GL.CullFace(CullFaceMode.FrontAndBack);

      //GL.Enable(EnableCap.Dither);
      //GL.Enable(EnableCap.Texture2D);

      #region Cores
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      _shaderWithTextures = new Shader("Shaders/shader.vert", "Shaders/shaderWithTextures.frag");

      _shaderWithTextures.Use();
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
      point = new Ponto(mundo, ref rotuloNovo, new Ponto4D(2.0, 0.0));
      point.PrimitivaTipo = PrimitiveType.Points;
      point.PrimitivaTamanho = 5;
      #endregion

      var vertexLocation = _shaderWithTextures.GetAttribLocation("aPosition");
      GL.EnableVertexAttribArray(vertexLocation);
      GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var textureCoordLocation = _shaderWithTextures.GetAttribLocation("aTextureCoord");
      GL.EnableVertexAttribArray(textureCoordLocation);
      GL.VertexAttribPointer(textureCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      #region Object: Bigger Cube
      biggerCube = new Cubo(mundo, ref rotuloNovo, 10);

      _texture = Texture.LoadFromFile("assets/alexandre.png");
      _texture.Use(TextureUnit.Texture0);

      biggerCube.shaderCor = _shaderWithTextures;
      //textures = ((Cubo) biggerCube).GetCubeTextures();
      //UseTextures();
      //OnLoadSetTexturesOnShader();
      #endregion

      /*#region Object: Smaller Cube
      smallerCube = new Cubo(mundo, ref rotuloNovo, 2);
      #endregion*/
      // objetoSelecionado.MatrizEscalaXYZ(0.2, 0.2, 0.2);

      _camera = new Camera(Vector3.UnitZ * 5, ClientSize.X / (float) ClientSize.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      //UseTextures();
      
      _texture.Use(TextureUnit.Texture0);
      _shaderWithTextures.Use();

      mundo.Desenhar(new Transformacao4D(), _camera);

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
        if (biggerCube == null)
          biggerCube = mundo;
        biggerCube.shaderCor = _shaderBranca;
        biggerCube = mundo.GrafocenaBuscaProximo(biggerCube);
        biggerCube.shaderCor = _shaderAmarela;
      }
      if (estadoTeclado.IsKeyPressed(Keys.G))
        mundo.GrafocenaImprimir("");
      if (estadoTeclado.IsKeyPressed(Keys.P) && biggerCube != null)
        Console.WriteLine(biggerCube.ToString());
      if (estadoTeclado.IsKeyPressed(Keys.M) && biggerCube != null)
        biggerCube.MatrizImprimir();
      if (estadoTeclado.IsKeyPressed(Keys.I) && biggerCube != null)
        biggerCube.MatrizAtribuirIdentidade();
      if (estadoTeclado.IsKeyPressed(Keys.Left) && biggerCube != null)
        biggerCube.MatrizTranslacaoXYZ(-0.05, 0, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Right) && biggerCube != null)
        biggerCube.MatrizTranslacaoXYZ(0.05, 0, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Up) && biggerCube != null)
        biggerCube.MatrizTranslacaoXYZ(0, 0.05, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Down) && biggerCube != null)
        biggerCube.MatrizTranslacaoXYZ(0, -0.05, 0);
      if (estadoTeclado.IsKeyPressed(Keys.O) && biggerCube != null)
        biggerCube.MatrizTranslacaoXYZ(0, 0, 0.05);
      if (estadoTeclado.IsKeyPressed(Keys.L) && biggerCube != null)
        biggerCube.MatrizTranslacaoXYZ(0, 0, -0.05);
      if (estadoTeclado.IsKeyPressed(Keys.PageUp) && biggerCube != null)
        biggerCube.MatrizEscalaXYZ(2, 2, 2);
      if (estadoTeclado.IsKeyPressed(Keys.PageDown) && biggerCube != null)
        biggerCube.MatrizEscalaXYZ(0.5, 0.5, 0.5);
      if (estadoTeclado.IsKeyPressed(Keys.Home) && biggerCube != null)
        biggerCube.MatrizEscalaXYZBBox(0.5, 0.5, 0.5);
      if (estadoTeclado.IsKeyPressed(Keys.End) && biggerCube != null)
        biggerCube.MatrizEscalaXYZBBox(2, 2, 2);
      if (estadoTeclado.IsKeyPressed(Keys.D1) && biggerCube != null)
        biggerCube.MatrizRotacao(10);
      if (estadoTeclado.IsKeyPressed(Keys.D2) && biggerCube != null)
        biggerCube.MatrizRotacao(-10);
      if (estadoTeclado.IsKeyPressed(Keys.D3) && biggerCube != null)
        biggerCube.MatrizRotacaoZBBox(10);
      if (estadoTeclado.IsKeyPressed(Keys.D4) && biggerCube != null)
        biggerCube.MatrizRotacaoZBBox(-10);

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
      if (MouseState.IsButtonDown(MouseButton.Right) && biggerCube != null)
      {
        Console.WriteLine("MouseState.IsButtonDown(MouseButton.Right)");

        int janelaLargura = ClientSize.X;
        int janelaAltura = ClientSize.Y;
        Ponto4D mousePonto = new Ponto4D(MousePosition.X, MousePosition.Y);
        Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

        biggerCube.PontosAlterar(sruPonto, 0);
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

    protected void UseTextures()
    {
      for (int i = 0; i < textures.Count; i++)
      {
        if (i == 0)
        {
          textures[i].Use(TextureUnit.Texture0);
        }
        else if (i == 1)
        {
          textures[i].Use(TextureUnit.Texture1);
        }
        else if (i == 2)
        {
          textures[i].Use(TextureUnit.Texture2);
        }
        else if (i == 3)
        {
          textures[i].Use(TextureUnit.Texture3);
        }
        else if (i == 4)
        {
          textures[i].Use(TextureUnit.Texture4);
        }
      }
    }

    protected void OnLoadSetTexturesOnShader()
    {
      string textureName = "";

      for (int i = 0; i < textures.Count; i++)
      {
        textureName = "texture" + i;
        _shaderWithTextures.SetInt(textureName, i);
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
