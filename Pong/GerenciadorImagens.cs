using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Pong
{
    /*
     * CLASSE GerenciadorImagens (estática)
     *
     * Carrega as imagens PNG do jogo: fundo, bola e raquetes. Não precisa
     * ser instanciada: é usada direto, como GerenciadorImagens.ObterBola().
     *
     * Funcionamento:
     *  - As imagens ficam na pasta "Imagens", ao lado do executável
     *    (bin\Debug\Imagens ou bin\Release\Imagens). O programa descobre
     *    essa pasta sozinho, sem caminho fixo, e lê todos os arquivos *.png.
     *  - Todas as imagens são carregadas uma única vez e guardadas em um cache
     *    (dicionário) indexado pelo nome do arquivo.
     *  - Nomes esperados: fundo.png, bola.png, raquete1.png e raquete2.png
     *    (ou raquete.png, usada para os dois jogadores).
     *  - Se uma imagem não existir, o método devolve null.
     */
    public static class GerenciadorImagens
    {
        private static readonly Dictionary<string, Image> _cache =
            new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        /*
         * Construtor estático
         *
         * Executa uma única vez, na primeira utilização da classe, e carrega
         * as imagens da subpasta "Imagens" e também de arquivos soltos
         * ao lado do executável.
         */
        static GerenciadorImagens()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            CarregarPasta(Path.Combine(baseDir, "Imagens"));
            CarregarPasta(baseDir);
        }

        /*
         * Método CarregarPasta
         *
         * Lê todos os arquivos PNG de uma pasta e os guarda no cache. Cada imagem
         * é copiada para um Bitmap novo, o que libera o arquivo no disco.
         * Se a pasta não existir, o método simplesmente sai.
         */
        private static void CarregarPasta(string pasta)
        {
            if (!Directory.Exists(pasta))
                return;

            foreach (string arquivo in Directory.GetFiles(pasta, "*.png"))
            {
                string nome = Path.GetFileName(arquivo);
                if (_cache.ContainsKey(nome))
                    continue;

                using (var fs = File.OpenRead(arquivo))
                using (var original = Image.FromStream(fs))
                {
                    _cache[nome] = new Bitmap(original);
                }
            }
        }

        /*
         * Método ObterFundo
         *
         * Devolve a imagem de fundo da quadra (fundo.png), ou null se não existir.
         */
        public static Image ObterFundo()
        {
            return Buscar("fundo.png");
        }

        /*
         * Método ObterBola
         *
         * Devolve a imagem da bola (bola.png), ou null se não existir.
         */
        public static Image ObterBola()
        {
            return Buscar("bola.png");
        }

        /*
         * Método ObterRaquete
         *
         * Devolve a imagem da raquete do jogador informado (1 ou 2). Procura
         * raquete1.png ou raquete2.png; se não achar, usa raquete.png.
         */
        public static Image ObterRaquete(int numeroJogador)
        {
            Image imagem = Buscar("raquete" + numeroJogador + ".png");
            return imagem ?? Buscar("raquete.png");
        }

        /*
         * Método Buscar
         *
         * Procura uma imagem no cache pelo nome do arquivo. Devolve a imagem ou null.
         */
        private static Image Buscar(string nomeArquivo)
        {
            Image imagem;
            _cache.TryGetValue(nomeArquivo, out imagem);
            return imagem;
        }
    }
}