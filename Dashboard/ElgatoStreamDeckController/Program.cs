using OpenMacroBoard.SDK;
using StreamDeckSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using vJoyInterfaceWrap;

namespace ElgatoStreamDeckController
{
    class Program
    {

        // Declaring one joystick and a position structure.
        // The ID must be between 1 and 16. 
        static public vJoy VirtualJoystick = new vJoy();
        static public vJoy.JoystickState VirtualJoystickState = new vJoy.JoystickState();
        static public uint Id = 1;
        static public List<Bitmap> buttonBitmaps = new List<Bitmap>();

        // Handles stream deck key presses.
        static void StreamDeckKeyPressed(object Sender, KeyEventArgs EventArgs)
        {
            VirtualJoystick.SetBtn(EventArgs.IsDown, 1, (uint)EventArgs.Key + 1);
        }

        // Main program, which handles joystick initialization.
        static void Main(string[] args)
        {

            // Verify the vJoy driver is enabled.
            if (!VirtualJoystick.vJoyEnabled()) return;

            // Get the state of the requested device.
            VjdStat Status = VirtualJoystick.GetVJDStatus(Id);

            switch (Status)
            {
                case VjdStat.VJD_STAT_OWN:
                case VjdStat.VJD_STAT_FREE:
                    break;

                case VjdStat.VJD_STAT_BUSY:
                case VjdStat.VJD_STAT_MISS:
                default:
                    return;

            };

            // Acquire the target joystick.
            if ((Status == VjdStat.VJD_STAT_OWN) || ((Status == VjdStat.VJD_STAT_FREE) && (!VirtualJoystick.AcquireVJD(Id)))) return;

            // Open the Stream Deck device.
            var Deck = StreamDeck.OpenDevice();

            // Set the brightness of the keys.
            Deck.SetBrightness(100);

            // Register the key pressed event handler.
            Deck.KeyStateChanged += StreamDeckKeyPressed;

            // Send the bitmap information to the device.
            Bitmap FullImage = new Bitmap(@"StreamDeckImage.png");

            for (int i = 0; i < Deck.Keys.Count; i++)
            {
                int Left = (i % 5) * (72 + 16);
                int Top = (int)Math.Floor((double)i / 5) * (72 + 16);

                Rectangle CropArea = new Rectangle(Left, Top, 72, 72);
                buttonBitmaps.Add(FullImage.Clone(CropArea, FullImage.PixelFormat));
            }

            // Infinitely loop, that way we can constantly receive key presses.
            while (true)
            {
                // TODO: Send OSC heartbeat message here.
                byte i = 0;
                foreach (Bitmap bitmap in buttonBitmaps)
                {
                    Deck.SetKeyBitmap(i, KeyBitmap.Create.FromBitmap(bitmap));
                    i++;
                }
                Thread.Sleep(1000);
            }

        }

        // Returns the path of the program executable.
        public string GetPath()
        {
            string Folder = Environment.CurrentDirectory;
            return Folder;
        }

    }
}
