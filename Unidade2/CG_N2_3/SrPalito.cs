using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class SrPalito : Objeto
    {
        private const int FinalPointPositionAngle = 45;
        private const double FinalPointPositionRadius = 0.5;

        public SrPalito(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Lines;
            PrimitivaTamanho = 1;

            Ponto4D initialPointPosition = new Ponto4D(0.0, 0.0);
            Ponto4D finalPointPosition = CG_Biblioteca.Matematica.GerarPtosCirculo(FinalPointPositionAngle, FinalPointPositionRadius);

            base.PontosAdicionar(initialPointPosition);
            base.PontosAdicionar(finalPointPosition);
            base.ObjetoAtualizar();
        }
    }
}
