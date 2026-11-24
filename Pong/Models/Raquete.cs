using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pong.Models
{
    /*
     * CLASSE Raquete
     *
     * Representa a raquete de um jogador. Ela fica em uma posição X fixa
     * (na esquerda ou na direita) e se movimenta apenas na vertical.
     *
     * Conceitos de POO aplicados:
     *  - Encapsulamento: X, Largura, Altura e Velocidade são somente leitura;
     *    o Y só muda pelos métodos da própria classe, e a direção só pode
     *    ser alterada pelo método DefinirDirecao.
     *  - Construtor: cria a raquete já com tamanho, posição X e velocidade.
     *
     * A direção é "volatile" porque é escrita pela thread da interface
     * (quando uma tecla é pressionada) e lida pela thread do jogo.
     */
    public class Raquete
    {
        private volatile int _direcao; // -1 = sobe, 0 = parada, 1 = desce

        public int X { get; }
        public int Y { get; private set; }
        public int Largura { get; }
        public int Altura { get; }
        public int Velocidade { get; }

        /*
         * Construtor
         *
         * Recebe a posição horizontal, o tamanho e a velocidade (pixels por
         * quadro). A posição vertical é definida depois, por Reposicionar.
         */
        public Raquete(int x, int largura, int altura, int velocidade)
        {
            X = x;
            Largura = largura;
            Altura = altura;
            Velocidade = velocidade;
        }

        /*
         * Propriedade CentroY
         *
         * Posição vertical do centro da raquete. A classe Jogo usa para
         * calcular em que ponto da raquete a bola bateu.
         */
        public double CentroY
        {
            get { return Y + Altura / 2.0; }
        }

        /*
         * Propriedade Retangulo
         *
         * Área ocupada pela raquete. Usada na detecção de colisão e no
         * posicionamento da imagem na tela.
         */
        public Rectangle Retangulo
        {
            get { return new Rectangle(X, Y, Largura, Altura); }
        }

        /*
         * Método DefinirDirecao
         *
         * Define se a raquete deve subir (-1), descer (1) ou ficar parada (0).
         * Qualquer outro número é convertido para um desses três valores.
         * A raquete só sai do lugar quando o método Mover é chamado.
         */
        public void DefinirDirecao(int direcao)
        {
            if (direcao < 0) _direcao = -1;
            else if (direcao > 0) _direcao = 1;
            else _direcao = 0;
        }

        /*
         * Método Mover
         *
         * Desloca a raquete na direção atual. Em seguida limita a posição
         * para que ela nunca ultrapasse a borda superior nem a inferior
         * da área de jogo.
         */
        public void Mover(int alturaArea)
        {
            int novoY = Y + _direcao * Velocidade;

            if (novoY < 0) novoY = 0;
            if (novoY > alturaArea - Altura) novoY = alturaArea - Altura;

            Y = novoY;
        }

        /*
         * Método Reposicionar
         *
         * Coloca a raquete no meio da altura da área de jogo. Usado ao
         * criar e ao reiniciar a partida.
         */
        public void Reposicionar(int alturaArea)
        {
            Y = (alturaArea - Altura) / 2;
        }
    }
}