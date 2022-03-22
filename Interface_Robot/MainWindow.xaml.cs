using ExtendedSerialPort;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.Encoding.GetEncoding(1252);
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InterfaceFailbot
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool toggle = false;
        DispatcherTimer timerAffichage;
        ReliableSerialPort serialPort1;
        //-------------------------------//
        //Initialisation de l'objet Robot//
        //-------------------------------//
        Robot robot = new Robot();
        public MainWindow()
        {
            InitializeComponent();
            
            //-----------------------------//
            // Initialisation du serialPort//
            //-----------------------------//
            serialPort1 = new ReliableSerialPort("COM10", 115200, Parity.None, 8, StopBits.One);
            serialPort1.Open();
            serialPort1.DataReceived += SerialPort1_DataReceived;

            //--------------------------------//
            //Initialisation du timerAffichage//
            //--------------------------------//
            timerAffichage = new DispatcherTimer();
            timerAffichage.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerAffichage.Tick += TimerAffichage_Tick; ;
            timerAffichage.Start();
        }

        //---------------//
        //Évènement Timer//
        //---------------//
        private void TimerAffichage_Tick(object sender, EventArgs e)
        {
            if (robot.receivedText != "")
            {
                // Ecriture du message reçu dans la boite de réception
                textBoxReception.Text += "Reçu : " + robot.receivedText + "\n";
                robot.receivedText = "";
            }
            if (robot.byteListReceived != [])
            {
                // Ecriture du message reçu dans la boite de réception
                textBoxReception.Text += "Reçu : ";
                for (int k = 0; k < robot.byteListReceived.Length; k++)
                {
                    textBoxReception.Text += robot.byteListReceived.Dequeue();
                }
                textBoxReception.Text += "\n";
            }
        }

        //---------------------//
        //Fonctions des boutons//
        //---------------------//
        private void buttonEnvoyer_Click(object sender, RoutedEventArgs e)
        {
            // Couleur du bouton
            if (toggle == false)
            {
                buttonEnvoyer.Background = Brushes.Beige;
                toggle = true;
            }
            else
            {
                buttonEnvoyer.Background = Brushes.RoyalBlue;
                toggle = false;
            }
            // Appel de la fonction d'envoi du message
            SendMessage();
        }
        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteList = new byte[20];
            for (int k=0; k<20; k++)
            {
                byteList[k] = (byte)(2 * k);
            }
            serialPort1.Write(byteList,0,byteList.Length);
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxReception.Text = "";
        }
        private void textBoxEmission_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        //----------------------------//
        //Fonctions d'envoi du message//
        //----------------------------//
        private void SendMessage()
        {
            // Envoi sur le port série
            serialPort1.WriteLine (textBoxEmission.Text);
            textBoxEmission.Text = "";
        }
        //---------------------------------//
        //Fonctions de reception du message//
        //---------------------------------//
        public void SerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            for (int k = 0; k < e.Data.Length; k++)
            {
                robot.byteListReceived.Enqueue(e.Data[k]);
            }
        }
    }
}
