using System;
using System.Threading;
using System.Windows.Forms;
using Pong.Models;

namespace Pong
{
    /*
     * CLASSE Form1 (parte lógica da tela)
     *
     * É a janela principal. Herda de Form (herança) e é "partial": a definição
     * é dividida com o Form1.Designer.cs, que cria os controles visuais.
     *
     * ASSOCIAÇÃO: possui um Jogo (as regras) e um MotorJogo (a thread). O
     * formulário NÃO contém regras do Pong: apenas
     *  - repassa os botões e o teclado para o Jogo e o MotorJogo;
     *  - atualiza o placar, a mensagem, os botões e a quadra.
     *
     * THREADS: o MotorJogo avisa, pela thread do jogo, que um quadro foi
     * processado. O Windows Forms só permite alterar a tela pela thread da
     * interface, por isso o aviso é repassado com BeginInvoke. Uma trava
     * (_atualizacaoPendente) impede que se acumulem pedidos de redesenho
     * quando a tela demora, o que mantém a interface fluida.
     */
    public partial class Form1 : Form
    {
        private readonly Jogo _jogo;
        private readonly MotorJogo _motor;

        private bool _teclaW;
        private bool _teclaS;
        private bool _teclaCima;
        private bool _teclaBaixo;

        private int _atualizacaoPendente;

        /*
         * Construtor
         *
         * Monta os controles (InitializeComponent), cria o Jogo com o tamanho
         * da quadra, entrega o Jogo à quadra para ela poder exibi-lo, cria o
         * MotorJogo e se inscreve no evento Atualizado. Por fim, mostra o
         * estado inicial da partida.
         */
        public Form1()
        {
            InitializeComponent();

            _jogo = new Jogo(areaJogo.Width, areaJogo.Height, "Jogador 1", "Jogador 2");
            areaJogo.Jogo = _jogo;

            lblJogador1.Text = _jogo.Jogador1.Nome + " (W / S)";
            lblJogador2.Text = _jogo.Jogador2.Nome + " (Setas)";

            _motor = new MotorJogo(_jogo);
            _motor.Atualizado += AoAtualizarMotor;

            AtualizarTela();
        }

        // ---------- Comunicação com a thread do jogo ----------

        /*
         * Método AoAtualizarMotor
         *
         * ATENÇÃO: este método roda na THREAD DO JOGO, e não na da interface.
         * Por isso ele não mexe em nenhum controle: pede à thread da interface,
         * com BeginInvoke, para executar AtualizarTela. BeginInvoke não bloqueia
         * a thread do jogo (ao contrário do Invoke), o que evita travamento
         * ao fechar a janela. Se já houver um redesenho pendente, o pedido
         * é ignorado.
         */
        private void AoAtualizarMotor()
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (Interlocked.Exchange(ref _atualizacaoPendente, 1) == 1)
                return;

            try
            {
                BeginInvoke(new Action(AtualizarTela));
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        /*
         * Método AtualizarTela
         *
         * Roda na thread da interface. Lê do Jogo, dentro do "lock" (para não
         * ler no meio de um quadro), a pontuação, o estado e a mensagem; depois
         * atualiza o placar, o texto de status, os botões e pede o redesenho
         * da quadra.
         */
        private void AtualizarTela()
        {
            Interlocked.Exchange(ref _atualizacaoPendente, 0);

            int pontos1;
            int pontos2;
            EstadoPartida estado;
            string mensagem;

            lock (_jogo.Sincronia)
            {
                pontos1 = _jogo.Jogador1.Pontos;
                pontos2 = _jogo.Jogador2.Pontos;
                estado = _jogo.Estado;
                mensagem = _jogo.ObterMensagem();
            }

            lblPlacar.Text = $"{pontos1}  x  {pontos2}";
            lblMensagem.Text = mensagem;
            AtualizarBotoes(estado);
            areaJogo.Invalidate();
        }

        /*
         * Método AtualizarBotoes
         *
         * Habilita ou bloqueia os botões conforme o estado da partida:
         *  - Aguardando: só Iniciar.
         *  - EmAndamento: só Pausar.
         *  - Pausada: Iniciar (que vira "Retomar") e Reiniciar.
         *  - Encerrada: só Reiniciar.
         * Reiniciar e Exibir Pontuação ficam sempre disponíveis.
         */
        private void AtualizarBotoes(EstadoPartida estado)
        {
            btnIniciar.Enabled = estado == EstadoPartida.Aguardando || estado == EstadoPartida.Pausada;
            btnPausar.Enabled = estado == EstadoPartida.EmAndamento;
            btnIniciar.Text = estado == EstadoPartida.Pausada ? "Retomar Jogo" : "Iniciar Jogo";
        }

        // ---------- Eventos dos botões ----------

        /*
         * Evento do botão Iniciar Jogo
         *
         * Inicia a partida (ou retoma a pausada). O MotorJogo cria a thread
         * na primeira vez. Depois devolve o foco à quadra.
         */
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            _motor.Iniciar();
            AtualizarTela();
            areaJogo.Focus();
        }

        /*
         * Evento do botão Pausar Jogo
         *
         * Interrompe a partida: o MotorJogo fecha o portão da thread.
         */
        private void btnPausar_Click(object sender, EventArgs e)
        {
            _motor.Pausar();
            AtualizarTela();
            areaJogo.Focus();
        }

        /*
         * Evento do botão Reiniciar Jogo
         *
         * Zera a pontuação e reposiciona raquetes e bola. A partida volta
         * ao estado Aguardando.
         */
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            _motor.Reiniciar();
            AtualizarTela();
            areaJogo.Focus();
        }

        /*
         * Evento do botão Exibir Pontuação
         *
         * Mostra a pontuação atual dos dois jogadores. Se a partida estiver
         * em andamento, ela é pausada enquanto a janela de aviso está aberta
         * e retomada ao fechá-la, para ninguém ser prejudicado.
         */
        private void btnPontuacao_Click(object sender, EventArgs e)
        {
            bool estavaRodando = _jogo.Estado == EstadoPartida.EmAndamento;

            if (estavaRodando)
            {
                _motor.Pausar();
                AtualizarTela();
            }

            int pontos1;
            int pontos2;
            lock (_jogo.Sincronia)
            {
                pontos1 = _jogo.Jogador1.Pontos;
                pontos2 = _jogo.Jogador2.Pontos;
            }

            MessageBox.Show(
                _jogo.Jogador1.Nome + ": " + pontos1 + " ponto(s)" + Environment.NewLine +
                _jogo.Jogador2.Nome + ": " + pontos2 + " ponto(s)" + Environment.NewLine + Environment.NewLine +
                "Vence quem chegar a " + Jogo.PontuacaoMaxima + " pontos.",
                "Pontuação");

            if (estavaRodando)
            {
                _motor.Iniciar();
                AtualizarTela();
            }

            areaJogo.Focus();
        }

        // ---------- Teclado ----------

        /*
         * Método ProcessCmdKey (sobrescrito)
         *
         * Intercepta as teclas ANTES dos botões. Isso é necessário porque as
         * setas do teclado, por padrão, mudam o foco entre os botões. Aqui,
         * quando uma tecla de jogo é pressionada, só marcamos que ela está
         * pressionada e atualizamos a direção das raquetes:
         *  - Jogador 1: W (sobe) e S (desce).
         *  - Jogador 2: Seta para Cima e Seta para Baixo.
         */
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.W: _teclaW = true; AtualizarDirecoes(); return true;
                case Keys.S: _teclaS = true; AtualizarDirecoes(); return true;
                case Keys.Up: _teclaCima = true; AtualizarDirecoes(); return true;
                case Keys.Down: _teclaBaixo = true; AtualizarDirecoes(); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /*
         * Evento KeyUp do formulário
         *
         * Quando a tecla é solta, desmarca e atualiza a direção. Se a outra
         * tecla do mesmo jogador continuar pressionada, a raquete segue
         * por ela.
         */
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: _teclaW = false; break;
                case Keys.S: _teclaS = false; break;
                case Keys.Up: _teclaCima = false; break;
                case Keys.Down: _teclaBaixo = false; break;
            }

            AtualizarDirecoes();
        }

        /*
         * Evento Deactivate do formulário
         *
         * Quando a janela perde o foco (por exemplo, com Alt+Tab), o soltar da
         * tecla não chega ao programa. Por isso todas as teclas são
         * consideradas soltas, evitando raquete "presa" andando sozinha.
         */
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            _teclaW = false;
            _teclaS = false;
            _teclaCima = false;
            _teclaBaixo = false;
            AtualizarDirecoes();
        }

        /*
         * Método AtualizarDirecoes
         *
         * Converte as teclas pressionadas em direção (-1 sobe, 0 para, 1 desce)
         * e informa a cada raquete. O movimento em si é feito pela thread do
         * jogo, a cada quadro. As duas raquetes podem se mover ao mesmo tempo.
         */
        private void AtualizarDirecoes()
        {
            _jogo.Jogador1.Raquete.DefinirDirecao((_teclaS ? 1 : 0) - (_teclaW ? 1 : 0));
            _jogo.Jogador2.Raquete.DefinirDirecao((_teclaBaixo ? 1 : 0) - (_teclaCima ? 1 : 0));
        }

        // ---------- Encerramento ----------

        /*
         * Método OnFormClosing (sobrescrito)
         *
         * Chamado ao fechar a janela. Pede ao MotorJogo para encerrar a thread
         * e aguardar o término dela. Sem isso, a thread poderia continuar
         * rodando depois que a aplicação fosse fechada.
         */
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_motor != null)
                _motor.Encerrar();

            base.OnFormClosing(e);
        }
    }
}