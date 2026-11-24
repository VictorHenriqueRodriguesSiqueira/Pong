using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Pong.Models
{
    /*
     * CLASSE MotorJogo
     *
     * É a classe responsável pela THREAD do jogo. Ela cria uma thread própria
     * que fica em um laço, chamando Jogo.Atualizar a cada quadro (cerca de
     * 60 vezes por segundo). Por isso a bola se movimenta continuamente e
     * de forma independente da thread principal da interface, que continua
     * livre para responder a teclas e botões sem congelar.
     *
     * SEPARAÇÃO DE RESPONSABILIDADES:
     *  - Jogo: conhece as regras.
     *  - MotorJogo: conhece o tempo e a thread (iniciar, pausar, retomar
     *    e encerrar).
     *  - Form1: só desenha e recebe os comandos do usuário.
     *
     * CONTROLE DA THREAD:
     *  - ManualResetEvent (_liberarExecucao): funciona como um portão. Aberto
     *    (Set), a thread roda; fechado (Reset), ela fica bloqueada no WaitOne,
     *    sem gastar processador. É assim que se pausa e se retoma.
     *  - Campo _encerrar: sinaliza para a thread terminar o laço.
     *
     * COMUNICAÇÃO COM A INTERFACE: a cada quadro a thread dispara o evento
     * Atualizado. Quem se inscrever nele (o formulário) deve lembrar que o
     * evento chega na thread do jogo, e não na da interface.
     */
    public class MotorJogo
    {
        private const int IntervaloQuadro = 16; // milissegundos (~60 quadros por segundo)

        private readonly Jogo _jogo;
        private readonly ManualResetEvent _liberarExecucao = new ManualResetEvent(false);
        private Thread _thread;
        private volatile bool _encerrar;

        /*
         * Evento Atualizado
         *
         * Disparado pela thread do jogo ao final de cada quadro, avisando que
         * o estado mudou e a tela precisa ser redesenhada.
         */
        public event Action Atualizado;

        /*
         * Construtor
         *
         * Recebe o Jogo que será executado pela thread (associação).
         * A thread só é criada na primeira vez que o jogo for iniciado.
         */
        public MotorJogo(Jogo jogo)
        {
            _jogo = jogo;
        }

        /*
         * Método Iniciar
         *
         * Inicia a partida ou retoma uma partida pausada. Na primeira chamada,
         * cria e inicia a thread (como thread de segundo plano, para não manter
         * o programa aberto se algo der errado). Nas chamadas seguintes,
         * apenas abre o portão, e a thread continua de onde parou.
         */
        public void Iniciar()
        {
            _jogo.Iniciar();

            if (_thread == null)
            {
                _thread = new Thread(Executar);
                _thread.IsBackground = true;
                _thread.Name = "ThreadJogoPong";
                _thread.Start();
            }

            _liberarExecucao.Set();
        }

        /*
         * Método Pausar
         *
         * Marca a partida como pausada e fecha o portão. A thread termina
         * o quadro atual e fica bloqueada até alguém chamar Iniciar novamente.
         */
        public void Pausar()
        {
            _jogo.Pausar();
            _liberarExecucao.Reset();
        }

        /*
         * Método Reiniciar
         *
         * Fecha o portão e reinicia o Jogo (pontos zerados, elementos no lugar).
         * A thread fica aguardando um novo Iniciar.
         */
        public void Reiniciar()
        {
            _liberarExecucao.Reset();
            _jogo.Reiniciar();
        }

        /*
         * Método Encerrar
         *
         * Termina a thread de forma segura. Marca o encerramento, abre o portão
         * (para que uma thread bloqueada acorde e perceba o sinal) e espera
         * a thread terminar (Join, com limite de 1 segundo). Deve ser chamado
         * ao fechar a janela, para a thread não continuar rodando depois que a
         * aplicação for fechada.
         */
        public void Encerrar()
        {
            _encerrar = true;
            _liberarExecucao.Set();

            if (_thread != null)
            {
                if (_thread.Join(1000))
                    _liberarExecucao.Close();
            }
            else
            {
                _liberarExecucao.Close();
            }
        }

        /*
         * Método Executar (privado): o corpo da thread
         *
         * Repete até receber o sinal de encerramento:
         *  1. Espera o portão abrir (bloqueia enquanto estiver pausado).
         *  2. Chama Jogo.Atualizar para processar um quadro.
         *  3. Dispara o evento Atualizado para a interface se redesenhar.
         *  4. Se a partida acabou, fecha o portão e aguarda um novo comando.
         *  5. Dorme um curto intervalo, para manter cerca de 60 quadros por segundo.
         */
        private void Executar()
        {
            while (!_encerrar)
            {
                _liberarExecucao.WaitOne();

                if (_encerrar)
                    break;

                _jogo.Atualizar();

                Action aoAtualizar = Atualizado;
                if (aoAtualizar != null)
                    aoAtualizar();

                if (_jogo.Estado == EstadoPartida.Encerrada)
                    _liberarExecucao.Reset();

                Thread.Sleep(IntervaloQuadro);
            }
        }
    }
}