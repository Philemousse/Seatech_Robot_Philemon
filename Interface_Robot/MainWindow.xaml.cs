using ExtendedSerialPort;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
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
        string receivedText = "";
        DispatcherTimer timerAffichage;
        ReliableSerialPort serialPort1;
        public MainWindow()
        {
            InitializeComponent();
            
            // Initialisation du serialPort
            serialPort1 = new ReliableSerialPort("COM10", 115200, Parity.None, 8, StopBits.One);
            serialPort1.Open();
            serialPort1.DataReceived += SerialPort1_DataReceived;

            // Initialisation du timerAffichage
            timerAffichage = new DispatcherTimer();
            timerAffichage.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerAffichage.Tick += TimerAffichage_Tick; ;
            timerAffichage.Start();
        }

        private void TimerAffichage_Tick(object sender, EventArgs e)
        {
        }

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

        private void textBoxEmission_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        // Fonction d'envoi du message
        private void SendMessage()
        {
            // Envoi du texte de la boite d'émission dans la box de réception
            textBoxReception.Text += "Reçu : " + textBoxEmission.Text + "\n";
            // Envoi sur le port série
            serialPort1.WriteLine (textBoxEmission.Text);
            textBoxEmission.Text = "";
        }
        public void SerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            receivedText += Encoding.UTF8.GetString(e.Data, 0, e.Data.Length);
        }
    }
}
