using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private const int GenerationCirclePointsCounter = 500; 

        public Circulo(Objeto _paiRef, ref char _rotulo, double radius) : base(_paiRef, ref _rotulo)
        {
            double increasingDrawingAngle = 1.0;
            PrimitivaTipo = PrimitiveType.Lines;
            PrimitivaTamanho = 5;

            for (int i = 0; i <= GenerationCirclePointsCounter; i++)
            {
                base.PontosAdicionar(Matematica.GerarPtosCirculo(increasingDrawingAngle, radius));
                base.ObjetoAtualizar();
                increasingDrawingAngle += 1.0;
            }
        }
    }
}
