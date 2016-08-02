using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Quic.Messaging;

namespace Quic.App.CLI
{
    class CLI
    {
        static bool initialized = false;
        static Dictionary<ArgType, string> commandLineArgs = new Dictionary<ArgType, string>();
        static ArgType currentArgType;

        static void Main(string[] args)
        {
            if (!initialized)
            {
                Console.WriteLine("Quic Command Line Interface");
                Console.WriteLine();

                Messenger.Prompted += Messenger_Prompted;
                Messenger.Notified += Messenger_Notified;
                Messenger.NotificationsCleared += Messenger_NotificationsCleared;

                initialized = true;
            }

            try
            {
                //assume the first arg to be the input file
                currentArgType = ArgType.InputFile;

                //then look for other args
                for (int index = 0; index < args.Length; index++)
                {
                    if (args[index].StartsWith("-")) //an attribute
                    {
                        if (args[index] == "--input-file" || args[index] == "-i")
                        {
                            currentArgType = ArgType.InputFile;
                        }
                        else if (args[index] == "--output-dir" || args[index] == "-o")
                        {
                            currentArgType = ArgType.OutputDirectory;
                        }
                        else if (args[index] == "--done" || args[index] == "-d")
                        {
                            currentArgType = ArgType.Done;
                        }
                    }
                    else //a value
                    {
                        commandLineArgs.Add(currentArgType, args[index]);
                    }
                }

                //if there are missing command line args, ask for them
                if (!commandLineArgs.ContainsKey(ArgType.InputFile))
                {
                    Console.WriteLine("Specify the path to the input file:");
                    string inputFile = Console.ReadLine();
                    commandLineArgs.Add(ArgType.InputFile, inputFile);
                }
                if (!commandLineArgs.ContainsKey(ArgType.OutputDirectory))
                {
                    Console.WriteLine("Specify the path to the output directory:");
                    string outputDirectory = Console.ReadLine();
                    commandLineArgs.Add(ArgType.OutputDirectory, outputDirectory);
                }

                string sourceFile = commandLineArgs[ArgType.InputFile];
                string outputDir = commandLineArgs[ArgType.OutputDirectory];

                if (!string.IsNullOrWhiteSpace(sourceFile) && !string.IsNullOrWhiteSpace(outputDir))
                {
                    Messenger.ClearNotifications();
                    QuicDocument doc = QuicDocument.Load(sourceFile);
                    doc.Render(outputDir);

                    //a success notif
                    if (Messenger.Notifications.Length == 0)
                    {
                        Messenger.Notify(
                       new QuicException("Build complete.", sourceFile) { MessageType = MessageType.Success });
                    }

                    if (commandLineArgs.ContainsKey(ArgType.Done))
                    {
                        string done = commandLineArgs[ArgType.Done];
                        if (done.Equals("open-dir", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (Directory.Exists(outputDir))
                                Process.Start(outputDir);
                        }
                    }
                }
            }
            catch (ArgumentException) 
            {
                Messenger.Prompt("A command line option appears more than once.", MessageType.Error);
            }
            catch (QuicException ex)
            {
                Messenger.Notify(ex);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Quic");
                Messenger.Prompt(ex.Message, MessageType.Error);
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit");
            Console.Read();
        }

        static void Messenger_NotificationsCleared(object sender, EventArgs e)
        {
            //
        }
        static void Messenger_Notified(object sender, Messenger.NotifyEventArgs e)
        {
            if (e.Notification != null)
            {
                Console.WriteLine();
                Console.WriteLine("Type: " + e.Notification.MessageType.ToString());
                Console.WriteLine("File: " + e.Notification.FilePath);
                Console.WriteLine("Line: " + e.Notification.Line);
                Console.WriteLine("Column: " + e.Notification.Column);
                Console.WriteLine("Message: " + e.Notification.Message);
                //if (e.Notification.MessageType == MessageType.Error)
                //{
                //    Console.WriteLine("Error>> " + e.Notification.Message);
                //}
                //else if (e.Notification.MessageType == MessageType.Info)
                //{
                //    Console.WriteLine("Info>> " + e.Notification.Message);
                //}
                //else if (e.Notification.MessageType == MessageType.Question)
                //{
                //    Console.WriteLine("Question>> " + e.Notification.Message);
                //}
                //else if (e.Notification.MessageType == MessageType.Success)
                //{
                //    Console.WriteLine("Success>> " + e.Notification.Message);
                //}
                //else if (e.Notification.MessageType == MessageType.Warning)
                //{
                //    Console.WriteLine("Warning>> " + e.Notification.Message);
                //}
                //else
                //{
                //    Console.WriteLine(">> " + e.Notification.Message);
                //}
            }
        }
        static void Messenger_Prompted(object sender, Messenger.MessengerEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Alert>> " + e.Message);
        }

        enum ArgType
        {
            InputFile = 0,
            OutputDirectory,
            Done
        }
    }
}
