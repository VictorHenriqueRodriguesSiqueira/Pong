using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Models
{
    /*
     * CLASSE Jogador
     *
     * Representa cada jogador da partida: guarda o nome, a pontuação
     * e a Raquete que ele controla.
     *
     * Conceitos de POO aplicados:
     *  - Associação: o Jogador possui uma Raquete.
     *  - Encapsulamento: os pontos só podem ser alterados pelos métodos
     *    AdicionarPonto e ZerarPontos (set privado).
     *  - Construtor: recebe o nome e a raquete já criada.
     */
    public class Jogador
    {
        public string Nome { get; }
        public int Pontos { get; private set; }
        public Raquete Raquete { get; }

        /*
         * Construtor
         *
         * Cria o jogador com nome e raquete, com a pontuação em zero.
         */
        public Jogador(string nome, Raquete raquete)
        {
            Nome = nome;
            Raquete = raquete;
            Pontos = 0;
        }

        /*
         * Método AdicionarPonto
         *
         * Soma um ponto ao jogador. É chamado pelo Jogo quando o adversário
         * deixa a bola passar.
         */
        public void AdicionarPonto()
        {
            Pontos++;
        }

        /*
         * Método ZerarPontos
         *
         * Volta a pontuação para zero. Usado ao reiniciar a partida.
         */
        public void ZerarPontos()
        {
            Pontos = 0;
        }
    }
}