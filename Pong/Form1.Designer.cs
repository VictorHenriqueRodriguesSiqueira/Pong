namespace Pong
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblJogador1 = new System.Windows.Forms.Label();
            this.lblPlacar = new System.Windows.Forms.Label();
            this.lblJogador2 = new System.Windows.Forms.Label();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnPausar = new System.Windows.Forms.Button();
            this.btnReiniciar = new System.Windows.Forms.Button();
            this.btnPontuacao = new System.Windows.Forms.Button();
            this.areaJogo = new Pong.AreaJogoControl();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTitulo.Location = new System.Drawing.Point(283, 26);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(85, 40);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "PONG";
            // 
            // lblJogador1
            // 
            this.lblJogador1.AutoSize = true;
            this.lblJogador1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJogador1.ForeColor = System.Drawing.Color.White;
            this.lblJogador1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblJogador1.Location = new System.Drawing.Point(41, 121);
            this.lblJogador1.Name = "lblJogador1";
            this.lblJogador1.Size = new System.Drawing.Size(79, 20);
            this.lblJogador1.TabIndex = 1;
            this.lblJogador1.Text = "Jogador 1";
            // 
            // lblPlacar
            // 
            this.lblPlacar.AutoSize = true;
            this.lblPlacar.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlacar.ForeColor = System.Drawing.Color.Gold;
            this.lblPlacar.Location = new System.Drawing.Point(282, 104);
            this.lblPlacar.Name = "lblPlacar";
            this.lblPlacar.Size = new System.Drawing.Size(81, 37);
            this.lblPlacar.TabIndex = 2;
            this.lblPlacar.Text = "0 X 0";
            // 
            // lblJogador2
            // 
            this.lblJogador2.AutoSize = true;
            this.lblJogador2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJogador2.ForeColor = System.Drawing.Color.White;
            this.lblJogador2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblJogador2.Location = new System.Drawing.Point(509, 121);
            this.lblJogador2.Name = "lblJogador2";
            this.lblJogador2.Size = new System.Drawing.Size(79, 20);
            this.lblJogador2.TabIndex = 3;
            this.lblJogador2.Text = "Jogador 2";
            // 
            // lblMensagem
            // 
            this.lblMensagem.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensagem.ForeColor = System.Drawing.Color.Gold;
            this.lblMensagem.Location = new System.Drawing.Point(203, 501);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(261, 39);
            this.lblMensagem.TabIndex = 4;
            // 
            // btnIniciar
            // 
            this.btnIniciar.BackColor = System.Drawing.Color.White;
            this.btnIniciar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciar.Location = new System.Drawing.Point(42, 581);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(109, 34);
            this.btnIniciar.TabIndex = 6;
            this.btnIniciar.Text = "Iniciar Jogo";
            this.btnIniciar.UseVisualStyleBackColor = false;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnPausar
            // 
            this.btnPausar.BackColor = System.Drawing.Color.White;
            this.btnPausar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPausar.Location = new System.Drawing.Point(184, 581);
            this.btnPausar.Name = "btnPausar";
            this.btnPausar.Size = new System.Drawing.Size(109, 34);
            this.btnPausar.TabIndex = 7;
            this.btnPausar.Text = "Pausar Jogo";
            this.btnPausar.UseVisualStyleBackColor = false;
            this.btnPausar.Click += new System.EventHandler(this.btnPausar_Click);
            // 
            // btnReiniciar
            // 
            this.btnReiniciar.BackColor = System.Drawing.Color.White;
            this.btnReiniciar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReiniciar.Location = new System.Drawing.Point(318, 581);
            this.btnReiniciar.Name = "btnReiniciar";
            this.btnReiniciar.Size = new System.Drawing.Size(109, 34);
            this.btnReiniciar.TabIndex = 8;
            this.btnReiniciar.Text = "Reiniciar Jogo";
            this.btnReiniciar.UseVisualStyleBackColor = false;
            this.btnReiniciar.Click += new System.EventHandler(this.btnReiniciar_Click);
            // 
            // btnPontuacao
            // 
            this.btnPontuacao.BackColor = System.Drawing.Color.White;
            this.btnPontuacao.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPontuacao.Location = new System.Drawing.Point(454, 581);
            this.btnPontuacao.Name = "btnPontuacao";
            this.btnPontuacao.Size = new System.Drawing.Size(126, 34);
            this.btnPontuacao.TabIndex = 9;
            this.btnPontuacao.Text = "Exibir Pontuação";
            this.btnPontuacao.UseVisualStyleBackColor = false;
            this.btnPontuacao.Click += new System.EventHandler(this.btnPontuacao_Click);
            // 
            // areaJogo
            // 
            this.areaJogo.Location = new System.Drawing.Point(12, 184);
            this.areaJogo.Name = "areaJogo";
            this.areaJogo.Size = new System.Drawing.Size(616, 264);
            this.areaJogo.TabIndex = 5;
            this.areaJogo.TabStop = false;
            this.areaJogo.Text = "areaJogoControl1";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(640, 660);
            this.Controls.Add(this.btnPontuacao);
            this.Controls.Add(this.btnReiniciar);
            this.Controls.Add(this.btnPausar);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.areaJogo);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.lblJogador2);
            this.Controls.Add(this.lblPlacar);
            this.Controls.Add(this.lblJogador1);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pong";
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblJogador1;
        private System.Windows.Forms.Label lblPlacar;
        private System.Windows.Forms.Label lblJogador2;
        private System.Windows.Forms.Label lblMensagem;
        private AreaJogoControl areaJogo;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnPausar;
        private System.Windows.Forms.Button btnReiniciar;
        private System.Windows.Forms.Button btnPontuacao;
    }
}

