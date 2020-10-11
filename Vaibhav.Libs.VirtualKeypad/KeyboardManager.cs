using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Vaibhav.Libs.VirtualKeypad
{
    public class KeyboardManager
    {
        private readonly DependencyObject owner;
        private Flyout flyout = new Flyout();
        private Keyboard keyboard = new Keyboard();
        public KeyboardManager(DependencyObject owner)
        {
            this.owner = owner;
        }

        public void Init()
        {
            InitFlyOut();
            RegisterControls();
        }

        private void InitFlyOut()
        {
            flyout.Content = keyboard;
            flyout.Placement = FlyoutPlacementMode.Right;
            flyout.AllowFocusOnInteraction = false;
            flyout.AreOpenCloseAnimationsEnabled = true;
            flyout.ShowMode = FlyoutShowMode.TransientWithDismissOnPointerMoveAway;
        }

        private void RegisterControls()
        {
            List<TextBox> lst = new List<TextBox>();
            FindChildren<TextBox>(lst, owner);

            foreach (var item in lst)
            {
                item.AddHandler(UIElement.PointerReleasedEvent,
                            new PointerEventHandler(TextBox_PointerReleased), true);

                item.AddHandler(UIElement.PointerCaptureLostEvent, new PointerEventHandler(TextBox_LostFocus), true);
            }
        }


        private void TextBox_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            RaiseCaptureEvent(sender);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            flyout.Hide();
        }

        private void RaiseCaptureEvent(object sender)
        {
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null)
            {
                if (flyout.IsOpen)
                    flyout.Hide();

                flyout.ShowAt(currentTextBox);
                currentTextBox.Focus(FocusState.Pointer);
            }
        }

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode)
                    where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }
    }
}
