using ComputerInterface;
using ComputerInterface.ViewLib;

namespace ComputerModExample
{
    public class MyModView : ComputerView
    {
        // This is called when you view is opened
        public override void OnShow()
        {
            base.OnShow();
            // changing the Text property will fire an PropertyChanged event
            // which lets the computer know the text has changed and update it
            Text = "Monkey Computer\nMonkey Computer\nMounkey Computer";
        }

        // you can do something on keypresses by overriding "OnKeyPressed"
        // it get's an EKeyboardKey passed as a parameter which wraps the old character string
        public override void OnKeyPressed(EKeyboardKey key)
        {
            switch (key)
            {
                case EKeyboardKey.Delete:
                    // "ReturnToMainMenu" will basically switch to the main menu again
                    ReturnToMainMenu();
                    break;
                case EKeyboardKey.Option1:
                    // If you want to switch to another view you can do it like this
                    ShowView<MyOtherModView>();
                    break;
            }
        }
    }
}