using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using WindowsInput;
using WindowsInput.Native;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vaibhav.Libs.VirtualKeypad
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Keyboard : Page
    {
        InputSimulator sim = new InputSimulator();
        public Keyboard()
        {
            this.InitializeComponent();
        }

        private void simulateTypingText(string Text, int typingDelay = 10, int startDelay = 0)
        {
            sim.Keyboard.Sleep(startDelay);
            string[] lines = Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int current = 1;
            foreach (string line in lines)
            {
                char[] words = line.ToCharArray();
                foreach (char word in words)
                {
                    sim.Keyboard.TextEntry(word).Sleep(typingDelay);
                }
                current++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            simulateTypingText(((Button)sender).Content.ToString());
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
        }
    }
}
