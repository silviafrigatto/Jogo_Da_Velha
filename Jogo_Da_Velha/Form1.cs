﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Jogo_Da_Velha
{
    public partial class Form1 : Form
    {

        // classe player com dois valores X e O (controle de símbolos do player e da AI)
        public enum Player
        {
            X, O
        }

        Player currentPlayer; // chama a classe player 

        List<Button> buttons; // cria uma lista de botões
        Random rand = new Random(); // importa uma classe geradora de números aleatórios
        int playerWins = 0; // inicializa o número de vitórias do player
        int computerWins = 0; // inicializa o número de vitórias do computador
        public Form1()
        {
            InitializeComponent();
            resetGame(); // chama a função
        }

        private void playerClick(object sender, EventArgs e)
        {
            var button = (Button)sender; // verifica qual botão foi clicado
            currentPlayer = Player.X; // atribui X ao player
            button.Text = currentPlayer.ToString(); // altera o texto do botão do player
            button.Enabled = false; // desabilita o botão
            button.BackColor = System.Drawing.Color.Cyan; // muda a cor do player
            buttons.Remove(button); // remove o o botão da lista para que o computador não possa "clicar"
            if(buttons.Count <= 4)
                Check(); // verifica se o player venceu
            AImoves.Start(); // inicia o timer da IA
        }


        //  INICIAR THREAD DA IA
        private void AImove(object sender, EventArgs e)
        {
            Thread th = new Thread(Thread_IA);
            th.Start();
        }

        private delegate void myDelegate();

        private void Thread_IA()
        {
            if(this.InvokeRequired)
            {
                myDelegate md = new myDelegate(Thread_IA);
                this.Invoke(md, null);
            }
            else
            {
                // A CPU vai escolher um número aleatório da lista para "clicar". 
                // Enquanto a lista for maior que 0 a CPU vai operar no jogo.
                // Se a lista for menor que 0, para de jogar
                if (buttons.Count > 0)
                {
                    int index = rand.Next(buttons.Count); // gera um número aleatório dentro do número de botões disponíveis
                    buttons[index].Enabled = false; // atribui o número ao botão                   

                    currentPlayer = Player.O; // define o símbolo da AI
                    buttons[index].Text = currentPlayer.ToString(); // mostra O no botão
                    buttons[index].BackColor = System.Drawing.Color.DarkSalmon; // muda a cor do background do botão
                    buttons.RemoveAt(index); // remove o botão da lista
                    if(buttons.Count <= 4)
                        Check(); // verifica se a AI ganhou
                    AImoves.Stop(); // para o timer da AI 
                }
            }
        }
        //  FIM THREAD DA IA

        private void restartGame(object sender, EventArgs e)
        {
            resetGame();
        }

        private void loadbuttons()
        {
            // coloca todos os botões na lista
            buttons = new List<Button> { button1, button2, button3, button4, button5, button6, button7, button9, button8 };
        }

        private void resetGame() // reinicia os objetos do jogo
        {
            foreach (Control X in this.Controls)
            {
                if (X is Button && X.Tag == "play")
                {
                    ((Button)X).Enabled = true; // habilita os botões
                    ((Button)X).Text = "?"; // muda o texto para ?
                    ((Button)X).BackColor = default(Color); // reseta as cores
                }
            }

            loadbuttons(); // reinsere os botões no jogo
        }

        private void Check() //  verifica quem venceu ou se houve empate
        {
            // verifica as possibilidades de vitória do player
            if (button1.Text == "X" && button2.Text == "X" && button3.Text == "X"
               || button4.Text == "X" && button5.Text == "X" && button6.Text == "X"
               || button7.Text == "X" && button9.Text == "X" && button8.Text == "X"
               || button1.Text == "X" && button4.Text == "X" && button7.Text == "X"
               || button2.Text == "X" && button5.Text == "X" && button8.Text == "X"
               || button3.Text == "X" && button6.Text == "X" && button9.Text == "X"
               || button1.Text == "X" && button5.Text == "X" && button9.Text == "X"
               || button3.Text == "X" && button5.Text == "X" && button7.Text == "X")
            {
               
                AImoves.Stop(); 
                MessageBox.Show("Você venceu!"); 
                playerWins++; 
                label1.Text = "Jogador (X): " + playerWins; 
                resetGame(); 
            }
            // verifica as possibilidades de vitória da IA
            else if (button1.Text == "O" && button2.Text == "O" && button3.Text == "O"
            || button4.Text == "O" && button5.Text == "O" && button6.Text == "O"
            || button7.Text == "O" && button9.Text == "O" && button8.Text == "O"
            || button1.Text == "O" && button4.Text == "O" && button7.Text == "O"
            || button2.Text == "O" && button5.Text == "O" && button8.Text == "O"
            || button3.Text == "O" && button6.Text == "O" && button9.Text == "O"
            || button1.Text == "O" && button5.Text == "O" && button9.Text == "O"
            || button3.Text == "O" && button5.Text == "O" && button7.Text == "O")
            {

                AImoves.Stop(); 
                MessageBox.Show("Não foi dessa vez... O Computador ganhou."); 
                computerWins++; 
                label2.Text = "Computador (O): " + computerWins; 
                resetGame(); 
            }
            // verifica empate
            else if (buttons.Count == 0)
            {
                AImoves.Stop();
                MessageBox.Show("Empatou.");
                resetGame();
            }
        }
    }
}