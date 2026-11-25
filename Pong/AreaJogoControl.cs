using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Pong.Models;

namespace Pong
{
    /*
     * CLASSE AreaJogoControl
     *
     * Controle visual que representa a quadra do Pong. Herda de Control
     * (herança), então se comporta como qualquer componente do Windows Forms.
     * Ele apenas EXIBE as imagens: o fundo (fundo.png), as duas raquetes e
     * a bola, todas fornecidas pelo GerenciadorImagens. Não desenha formas
     * por código e não contém nenhuma regra do jogo.
     *
     * ASSOCIAÇÃO: recebe o Jogo pela propriedade Jogo e lê dele as posições
     * onde cada imagem deve ser colocada. Usa buffer duplo, o que evita
     * tremulação e deixa o movimento fluido.
     */
    public class AreaJogoControl : Control
    {
        private Jogo _jogo;

        /*
         * Propriedade Jogo
         *
         * O Jogo que será exibido. Ao receber um novo valor, a quadra
         * é redesenhada. Não aparece no Designer do Visual Studio.
         */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Jogo Jogo
        {
            get { return _jogo; }
            set
            {
                _jogo = value;
                Invalidate();
            }
        }

        /*
         * Construtor
         *
         * Ativa o desenho próprio com buffer duplo (sem tremulação).
         */
        public AreaJogoControl()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
        }

        /*
         * Método OnPaint (sobrescrito)
         *
         * Chamado sempre que a quadra precisa ser atualizada. Em ordem:
         *  1. Limpa a área e coloca a imagem de fundo, esticada para o
         *     tamanho da quadra.
         *  2. Copia as posições das raquetes e da bola dentro de um "lock" no
         *     objeto Sincronia do Jogo (assim a thread do jogo não muda os
         *     valores no meio da leitura).
         *  3. Coloca a imagem de cada raquete e da bola na posição lida.
         * Se alguma imagem não existir, aquele elemento simplesmente não aparece.
         */
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(BackColor);

            Image fundo = GerenciadorImagens.ObterFundo();
            if (fundo != null)
                g.DrawImage(fundo, new Rectangle(0, 0, Width, Height));

            if (_jogo == null)
                return;

            Rectangle raquete1;
            Rectangle raquete2;
            Rectangle bola;

            lock (_jogo.Sincronia)
            {
                raquete1 = _jogo.Jogador1.Raquete.Retangulo;
                raquete2 = _jogo.Jogador2.Raquete.Retangulo;
                bola = _jogo.Bola.Retangulo;
            }

            ColocarImagem(g, GerenciadorImagens.ObterRaquete(1), raquete1);
            ColocarImagem(g, GerenciadorImagens.ObterRaquete(2), raquete2);
            ColocarImagem(g, GerenciadorImagens.ObterBola(), bola);
        }

        /*
         * Método ColocarImagem (privado)
         *
         * Coloca uma imagem na área indicada, esticada para o tamanho dela.
         * Se a imagem for nula (arquivo não encontrado), não faz nada.
         */
        private void ColocarImagem(Graphics g, Image imagem, Rectangle area)
        {
            if (imagem != null)
                g.DrawImage(imagem, area);
        }
    }
}