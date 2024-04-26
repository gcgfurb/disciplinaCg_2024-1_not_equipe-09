using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Circulo : Objeto
    {

        private const int GenerationCirclePointsCounter = 390; 

        public Circulo(Objeto _paiRef, ref char _rotulo, double radius) : base(_paiRef, ref _rotulo)
        {
            double increasingDrawingAngle = 1.0;
            PrimitivaTipo = PrimitiveType.Lines;
            PrimitivaTamanho = 5;

            UpdateObject(0.0, 0.0, radius, increasingDrawingAngle);
            base.ObjetoAtualizar();
        }

        public void UpdateObject(double transformX, double transformY, double radius, double increasingDrawingAngle)
        {
            for (int i = 0; i <= GenerationCirclePointsCounter; i++)
            {
                Ponto4D circlePoint = Matematica.GerarPtosCirculo(increasingDrawingAngle, radius);

                if (transformX == 0.0 && transformY == 0.0)
                {
                    base.PontosAdicionar(new Ponto4D(circlePoint.X, circlePoint.Y));
                }
                else
                {
                    base.PontosAlterar(new Ponto4D(circlePoint.X, circlePoint.Y + transformY), i);       
                }
                increasingDrawingAngle += 1.0;
            }
        }
    }
}
