using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Models
{
    /*
     * ENUM EstadoPartida
     *
     * Representa em que momento a partida está:
     *  - Aguardando: a partida ainda não começou (ou acabou de ser reiniciada).
     *  - EmAndamento: a bola está em movimento.
     *  - Pausada: a partida foi interrompida temporariamente.
     *  - Encerrada: um dos jogadores atingiu a pontuação máxima.
     *
     * A interface usa esse estado para habilitar ou bloquear os botões,
     * e o Jogo usa para saber se deve ou não atualizar o movimento.
     */
    public enum EstadoPartida
    {
        Aguardando,
        EmAndamento,
        Pausada,
        Encerrada
    }
}
