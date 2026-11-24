using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pong.Models
{
    /*
     * CLASSE Jogo
     *
     * É a classe central das regras: controla colisões, pontuação e o
     * andamento da partida. Não tem nenhum código de tela: a interface só
     * lê o estado dela e a thread (MotorJogo) só chama o método Atualizar.
     *
     * ASSOCIAÇÃO: o Jogo possui uma Bola e dois Jogadores (cada um com
     * sua Raquete) e guarda o Estado da partida.
     *
     * SINCRONIZAÇÃO: a thread do jogo altera o estado enquanto a thread da
     * interface lê o estado para desenhar. Por isso os métodos que mudam o
     * estado usam "lock" no objeto Sincronia, e a interface usa o mesmo
     * objeto ao ler. Assim ninguém lê um quadro "pela metade".
     */
    public class Jogo
    {
        public const int PontuacaoMaxima = 5;

        private const int MargemRaquete = 20;
        private const int LarguraRaquete = 26;
        private const int AlturaRaquete = 102;
        private const int VelocidadeRaquete = 8;
        private const int TamanhoBola = 26;

        private volatile EstadoPartida _estado;

        public int LarguraArea { get; }
        public int AlturaArea { get; }
        public Bola Bola { get; }
        public Jogador Jogador1 { get; }
        public Jogador Jogador2 { get; }
        public Jogador Vencedor { get; private set; }
        public object Sincronia { get; } = new object();

        /*
         * Construtor
         *
         * Recebe o tamanho da área de jogo e o nome dos jogadores. Cria a bola
         * e os dois jogadores (a raquete 1 na esquerda e a raquete 2 na direita)
         * e deixa tudo posicionado, com a partida em estado Aguardando.
         */
        public Jogo(int larguraArea, int alturaArea, string nomeJogador1, string nomeJogador2)
        {
            LarguraArea = larguraArea;
            AlturaArea = alturaArea;

            Bola = new Bola(TamanhoBola);

            Jogador1 = new Jogador(nomeJogador1,
                new Raquete(MargemRaquete, LarguraRaquete, AlturaRaquete, VelocidadeRaquete));

            Jogador2 = new Jogador(nomeJogador2,
                new Raquete(larguraArea - MargemRaquete - LarguraRaquete,
                            LarguraRaquete, AlturaRaquete, VelocidadeRaquete));

            Reiniciar();
        }

        /*
         * Propriedade Estado
         *
         * Estado atual da partida (Aguardando, EmAndamento, Pausada ou Encerrada).
         */
        public EstadoPartida Estado
        {
            get { return _estado; }
        }

        /*
         * Método Iniciar
         *
         * Coloca a bola em movimento. Funciona tanto para começar a partida
         * quanto para retomar uma partida pausada. Em outros estados, não faz nada.
         */
        public void Iniciar()
        {
            lock (Sincronia)
            {
                if (_estado == EstadoPartida.Aguardando || _estado == EstadoPartida.Pausada)
                    _estado = EstadoPartida.EmAndamento;
            }
        }

        /*
         * Método Pausar
         *
         * Interrompe a partida temporariamente. Com o estado Pausada, o método
         * Atualizar deixa de mover a bola e as raquetes.
         */
        public void Pausar()
        {
            lock (Sincronia)
            {
                if (_estado == EstadoPartida.EmAndamento)
                    _estado = EstadoPartida.Pausada;
            }
        }

        /*
         * Método Reiniciar
         *
         * Reinicia completamente a partida: zera a pontuação dos dois jogadores,
         * apaga o vencedor, reposiciona as raquetes e a bola e volta
         * para o estado Aguardando.
         */
        public void Reiniciar()
        {
            lock (Sincronia)
            {
                Jogador1.ZerarPontos();
                Jogador2.ZerarPontos();
                Vencedor = null;

                Jogador1.Raquete.Reposicionar(AlturaArea);
                Jogador2.Raquete.Reposicionar(AlturaArea);
                Bola.Reposicionar(LarguraArea / 2, AlturaArea / 2);

                _estado = EstadoPartida.Aguardando;
            }
        }

        /*
         * Método Atualizar
         *
         * Executa UM quadro do jogo. É chamado repetidamente pela thread
         * do jogo. Se a partida não estiver em andamento, não faz nada.
         * Em cada quadro: move as raquetes, move a bola, trata a colisão
         * com as bordas, trata a colisão com as raquetes e verifica se
         * alguém marcou ponto.
         */
        public void Atualizar()
        {
            lock (Sincronia)
            {
                if (_estado != EstadoPartida.EmAndamento)
                    return;

                Jogador1.Raquete.Mover(AlturaArea);
                Jogador2.Raquete.Mover(AlturaArea);

                Bola.Mover();

                TratarColisaoBordas();
                TratarColisaoRaquetes();
                VerificarPonto();
            }
        }

        /*
         * Método TratarColisaoBordas (privado)
         *
         * Se a bola tocar a borda superior ou inferior, ela é recolocada
         * exatamente na borda (para não atravessá-la) e a direção vertical
         * é invertida. A verificação da direção evita inverter duas vezes
         * seguidas e a bola ficar presa.
         */
        private void TratarColisaoBordas()
        {
            if (Bola.Y <= 0)
            {
                Bola.DefinirPosicao(Bola.X, 0);
                if (Bola.VelocidadeY < 0) Bola.InverterDirecaoY();
            }
            else if (Bola.Y + Bola.Tamanho >= AlturaArea)
            {
                Bola.DefinirPosicao(Bola.X, AlturaArea - Bola.Tamanho);
                if (Bola.VelocidadeY > 0) Bola.InverterDirecaoY();
            }
        }

        /*
         * Método TratarColisaoRaquetes (privado)
         *
         * Verifica se a bola encostou em uma raquete. Para colidir, a bola
         * precisa estar indo na direção da raquete e ter o centro do lado de
         * dentro dela. Isso evita que uma bola que já passou da raquete seja
         * "puxada de volta". Na colisão, a bola é encostada na face da raquete
         * e rebatida.
         */
        private void TratarColisaoRaquetes()
        {
            Rectangle retanguloBola = Bola.Retangulo;
            Raquete raquete1 = Jogador1.Raquete;
            Raquete raquete2 = Jogador2.Raquete;

            if (Bola.VelocidadeX < 0
                && retanguloBola.IntersectsWith(raquete1.Retangulo)
                && Bola.CentroX > raquete1.X + raquete1.Largura / 2.0)
            {
                Bola.DefinirPosicao(raquete1.X + raquete1.Largura, Bola.Y);
                RebaterNaRaquete(raquete1);
            }
            else if (Bola.VelocidadeX > 0
                && retanguloBola.IntersectsWith(raquete2.Retangulo)
                && Bola.CentroX < raquete2.X + raquete2.Largura / 2.0)
            {
                Bola.DefinirPosicao(raquete2.X - Bola.Tamanho, Bola.Y);
                RebaterNaRaquete(raquete2);
            }
        }

        /*
         * Método RebaterNaRaquete (privado)
         *
         * Aplica a rebatida: inverte a direção horizontal, ajusta a inclinação
         * conforme o ponto da raquete atingido (centro = reto, pontas =
         * inclinado) e aumenta um pouco a velocidade.
         */
        private void RebaterNaRaquete(Raquete raquete)
        {
            Bola.InverterDirecaoX();

            double fator = (Bola.CentroY - raquete.CentroY) / (raquete.Altura / 2.0);
            Bola.AjustarVelocidadeVertical(fator);

            Bola.AumentarVelocidade();
        }

        /*
         * Método VerificarPonto (privado)
         *
         * Se a bola saiu completamente pela esquerda, o jogador 2 pontua;
         * se saiu pela direita, o jogador 1 pontua.
         */
        private void VerificarPonto()
        {
            if (Bola.X + Bola.Tamanho < 0)
                Pontuar(Jogador2);
            else if (Bola.X > LarguraArea)
                Pontuar(Jogador1);
        }

        /*
         * Método Pontuar (privado)
         *
         * Soma o ponto ao jogador. Se ele atingiu a pontuação máxima, define
         * o vencedor e encerra a partida. A bola sempre volta ao centro,
         * com uma nova direção sorteada.
         */
        private void Pontuar(Jogador autor)
        {
            autor.AdicionarPonto();

            if (autor.Pontos >= PontuacaoMaxima)
            {
                Vencedor = autor;
                _estado = EstadoPartida.Encerrada;
            }

            Bola.Reposicionar(LarguraArea / 2, AlturaArea / 2);
        }

        /*
         * Método ObterMensagem
         *
         * Devolve o texto de status mostrado na tela, conforme o estado:
         * instrução inicial, aviso de pausa ou o vencedor da partida.
         */
        public string ObterMensagem()
        {
            switch (_estado)
            {
                case EstadoPartida.Aguardando:
                    return "Pressione Iniciar Jogo";
                case EstadoPartida.Pausada:
                    return "Jogo pausado";
                case EstadoPartida.Encerrada:
                    return $"{Vencedor.Nome} venceu a partida!";
                default:
                    return "";
            }
        }
    }
}
