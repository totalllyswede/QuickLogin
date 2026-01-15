using InputSimulatorStandard;
using InputSimulatorStandard.Native;

namespace QuickLogin.Services
{
    public static class TypingService
    {
        private static readonly InputSimulator sim = new();

        public static async Task TypeLoginAsync(
            string username,
            string password,
            int delayMs = 400
        )
        {
            sim.Keyboard.TextEntry(username);

            await Task.Delay(delayMs);

            sim.Keyboard.KeyPress(VirtualKeyCode.TAB);

            await Task.Delay(delayMs);

            sim.Keyboard.TextEntry(password);

            await Task.Delay(delayMs);

            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }
    }
}
