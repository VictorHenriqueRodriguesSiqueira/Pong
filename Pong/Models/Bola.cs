using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pong.Models
{
    /*
     * CLASSE Bola
     *
     * Representa a bola: guarda a posição (X, Y), o tamanho e a velocidade
     * em cada eixo. O sinal da velocidade é a direção: velocidade X negativa
     * vai para a esquerda e positiva vai para a direita; o mesmo vale
     * para o eixo Y.
     *
     * Conceitos de POO aplicados:
     *  - Encapsulamento: posição e velocidades só podem ser alteradas pelos
     *    métodos da própria bola (set privado).
     *  - Construtor: recebe o tamanho da bola.
     *
     * A bola não conhece as raquetes nem as regras: ela só sabe se mover
     * e inverter a direção. Quem decide quando isso acontece é a classe Jogo.
     */
    public class Bola
    {
        public const double VelocidadeInicial = 6.0;
        public const double VelocidadeMaxima = 14.0;
        private const double AumentoPorRebatida = 0.5;
        private const double VelocidadeVerticalMaxima = 7.0;

        private readonly Random _random = new Random();

        public double X { get; private set; }
        public double Y { get; private set; }
        public double VelocidadeX { get; private set; }
        public double VelocidadeY { get; private set; }
        public int Tamanho { get; }

        /*
         * Construtor
         *
         * Cria a bola com o tamanho informado (largura e altura iguais).
         * A posição e a direção são definidas por Reposicionar.
         */
        public Bola(int tamanho)
        {
            Tamanho = tamanho;
        }

        /*
         * Propriedades CentroX e CentroY
         *
         * Posição do centro da bola. Usadas para saber de que lado
         * e em que altura a bola bateu na raquete.
         */
        public double CentroX
        {
            get { return X + Tamanho / 2.0; }
        }

        public double CentroY
        {
            get { return Y + Tamanho / 2.0; }
        }

        /*
         * Propriedade Retangulo
         *
         * Área ocupada pela bola, usada na colisão e no posicionamento
         * da imagem na tela.
         */
        public Rectangle Retangulo
        {
            get { return new Rectangle((int)Math.Round(X), (int)Math.Round(Y), Tamanho, Tamanho); }
        }

        /*
         * Método Mover
         *
         * Avança a bola um passo, somando a velocidade à posição.
         * É chamado uma vez a cada quadro pela thread do jogo, o que dá
         * o movimento contínuo, sem depender de nenhuma ação do usuário.
         */
        public void Mover()
        {
            X += VelocidadeX;
            Y += VelocidadeY;
        }

        /*
         * Métodos InverterDirecaoX e InverterDirecaoY
         *
         * Invertem o sinal da velocidade. InverterDirecaoX é usado quando a bola
         * bate em uma raquete; InverterDirecaoY, quando bate no topo ou
         * no fundo da área de jogo.
         */
        public void InverterDirecaoX()
        {
            VelocidadeX = -VelocidadeX;
        }

        public void InverterDirecaoY()
        {
            VelocidadeY = -VelocidadeY;
        }

        /*
         * Método DefinirPosicao
         *
         * Coloca a bola em uma posição exata. Usado para "encostar" a bola na
         * raquete ou na borda depois de uma colisão, evitando que ela fique
         * presa ou atravesse o obstáculo.
         */
        public void DefinirPosicao(double x, double y)
        {
            X = x;
            Y = y;
        }

        /*
         * Método AjustarVelocidadeVertical
         *
         * Muda a inclinação da bola conforme o ponto da raquete onde ela bateu.
         * O fator vai de -1 (ponta de cima) a 1 (ponta de baixo); no centro,
         * a bola segue mais reta. Há uma velocidade mínima para a bola nunca
         * ficar totalmente horizontal.
         */
        public void AjustarVelocidadeVertical(double fator)
        {
            if (fator < -1.0) fator = -1.0;
            if (fator > 1.0) fator = 1.0;

            double novaVelocidade = fator * VelocidadeVerticalMaxima;

            if (Math.Abs(novaVelocidade) < 1.0)
                novaVelocidade = novaVelocidade >= 0 ? 1.0 : -1.0;

            VelocidadeY = novaVelocidade;
        }

        /*
         * Método AumentarVelocidade
         *
         * A cada rebatida a bola fica um pouco mais rápida, até um limite.
         * O limite é importante: uma bola rápida demais poderia pular
         * a raquete de um quadro para o outro sem colidir.
         */
        public void AumentarVelocidade()
        {
            double velocidade = Math.Abs(VelocidadeX) + AumentoPorRebatida;

            if (velocidade > VelocidadeMaxima)
                velocidade = VelocidadeMaxima;

            VelocidadeX = Math.Sign(VelocidadeX) * velocidade;
        }

        /*
         * Método Reposicionar
         *
         * Coloca a bola no centro da área de jogo e sorteia uma nova direção
         * inicial: para a esquerda ou para a direita, subindo ou descendo, com
         * inclinação aleatória. A velocidade volta ao valor inicial.
         * Usado no início, a cada ponto e ao reiniciar.
         */
        public void Reposicionar(int centroX, int centroY)
        {
            X = centroX - Tamanho / 2.0;
            Y = centroY - Tamanho / 2.0;

            int sentido = _random.Next(2) == 0 ? -1 : 1;
            int inclinacao = _random.Next(2, 5);
            if (_random.Next(2) == 0) inclinacao = -inclinacao;

            VelocidadeX = sentido * VelocidadeInicial;
            VelocidadeY = inclinacao;
        }
    }
}