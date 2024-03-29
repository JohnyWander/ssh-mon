﻿using System;
using System.Security.Cryptography;
namespace ssh_mon.Tools
{
    internal sealed class Tools : Interfaces.IStringTools, Interfaces.IKeyGen,Interfaces.IInputPassword,Interfaces.IConsoleWrite
    {
        // text
        public string left(string text, int count)
        {
            return text.Substring(0, count);
        }

        public string right(string text, int count)
        {
            string output = "";
            int ite = 0;
            foreach (char x in text)
            {
                if (ite >= count)
                {
                    output += x;
                }
                ite++;
            }
            return output;
        }
        // crypto

        public RSA keypair_gen()
        {
            return RSA.Create();
        }


        public void draw_hash()
        {
            for (int i = 0; i <= Console.WindowWidth - Console.CursorLeft; i++)
            {
                Console.Write("#");
            }

        }

        public void echo_chars()
        {
            for (int i = 1; i < 256; i++)
            {
                Console.WriteLine(i + " = " + (char)i);

                if (i % 22 == 0)
                {
                    Console.WriteLine("Please press any key to turn page");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        public string input_password()
        {
            string pass = "";
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass;
        }



        public void color_consoleWriteLine(ConsoleColor consoleColor,string text)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();

        }




    }
}
