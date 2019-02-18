﻿using OpenMacroBoard.SDK;
using StreamDeckSharp;
using System;
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
            Random Rand = new Random();
            for (int i = 0; i < Deck.Keys.Count; i++)
            {
                Deck.SetKeyBitmap(i, KeyBitmap.Create.FromRgb((byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255), (byte)Rand.Next(0, 255)));
            }

            // Infinitely loop, that way we can constantly receive key presses.
            while (true)
            {
                // TODO: Send OSC heartbeat message here.
                Thread.Sleep(1000);
            }

        }
    }
}