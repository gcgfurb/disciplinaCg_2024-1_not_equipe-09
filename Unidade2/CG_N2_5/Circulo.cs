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

            UpdateObject(null, 0.0, 0.0, radius, increasingDrawingAngle);
        }

        public void UpdateObject(Circulo circulo, double transformX, double transformY, double radius, double increasingDrawingAngle)
        {
            for (int i = 0; i <= GenerationCirclePointsCounter; i++)
            {
                Ponto4D circlePoint = Matematica.GerarPtosCirculo(increasingDrawingAngle, radius);

                if (circulo == null)
                {
                    base.PontosAdicionar(new Ponto4D(circlePoint.X, circlePoint.Y));
                }
                else
                {
                    if (i != 390)
                    {
                        circulo.PontosAlterar(new Ponto4D(circlePoint.X + transformX, circlePoint.Y + transformY), i);
                    }
                }
                increasingDrawingAngle += 1.0;
            }
        }
    }
}
