using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace PSL1GHTBuilder
{
    class Program
    {
        static string startUp = "";
        static bool exit = false;
        static bool hasErrors = false;

        private static Process p;
        public static void Build(string sdkPath, string makeCmd)
        {
            string batch = "@echo off\r\n";
            batch += sdkPath.Split(':')[0] + ":\r\n"; //move to new drive (if new drive, safe to be safe)
            batch += "cd \"" + Path.Combine(sdkPath, "MinGW/msys/1.0/bin").Replace("\\", "/") + "\"\r\n";
            batch += "sh --login -i\r\n";
            File.WriteAllText(Path.Combine(startUp, "temp.bat"), batch);

            try { p.Kill(); }
            catch { }
            p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                FileName = "CMD.exe",
                Arguments = "/c \"" + Path.Combine(startUp, "temp.bat").Replace("\\", "/") + "\""
            };

            p.OutputDataReceived += p_OutputDataReceived;
            p.ErrorDataReceived += p_ErrorDataReceived;
            p.Exited += p_Exited;

            p.Start();

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            p.StandardInput.WriteLine("cd \"" + startUp + "\"");
            p.StandardInput.WriteLine(makeCmd);
            p.StandardInput.WriteLine("exit");
        }

        static void p_Exited(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(startUp, "temp.bat")))
                File.Delete(Path.Combine(startUp, "temp.bat"));
            exit = true;
        }

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                p_Exited(sender, null);
                return;
            }

            if (e.Data.Length > 0 && e.Data[0] != 27)
            {
                string str = e.Data.Trim('$').Trim();

                if (str.Length > 0)
                {
                    string[] words = str.Split(' ');
                    if (words.Length > 1 && words[1].ToLower().IndexOf("error") == 0)
                    {
                        hasErrors = true;
                        string error = "";
                        string[] colonWords = str.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                        if (colonWords[0].Length == 1) //Full path
                        {
                            if (colonWords[2].IndexOf("error") < 0)
                            {
                                error = colonWords[0].ToUpper() + ":" + colonWords[1].Replace("/", "\\") + "(" + colonWords[2] + ") : ";
                                error += "At index " + colonWords[3] + " " + String.Join(":", colonWords, 4, colonWords.Length - 4).Trim();
                            }
                            else
                            {
                                error = colonWords[0].ToUpper() + ":" + colonWords[1].Replace("/", "\\") + "(" + colonWords[2] + ") : ";
                                error += String.Join(":", colonWords, 3, colonWords.Length - 3).Trim();
                            }
                        }
                        else
                        {
                            if (colonWords[2].IndexOf("error") < 0)
                            {
                                error = colonWords[0].Replace("/", "\\") + "(" + colonWords[1] + ") : ";
                                error += "At index " + colonWords[2] + " " + String.Join(":", colonWords, 3, colonWords.Length - 3).Trim();
                            }
                            else
                            {
                                error = colonWords[0].Replace("/", "\\") + "(" + colonWords[1] + ") : ";
                                error += String.Join(":", colonWords, 2, colonWords.Length - 2).Trim();
                            }
                        }
                        Console.Error.WriteLine(error);
                    }
                    else if (words.Length > 1 && words[1].ToLower().IndexOf("warning") == 0)
                    {
                        hasErrors = true;
                        string error = "";
                        string[] colonWords = str.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                        if (colonWords[0].Length == 1) //Full path
                        {
                            if (colonWords[3].IndexOf("warning") < 0)
                            {
                                error = colonWords[0].ToUpper() + ":" + colonWords[1].Replace("/", "\\") + "(" + colonWords[2] + "," + colonWords[3] + ") : ";
                                error += String.Join(":", colonWords, 4, colonWords.Length - 4).Trim();
                            }
                            else
                            {
                                error = colonWords[0].ToUpper() + ":" + colonWords[1].Replace("/", "\\") + "(" + colonWords[2] + ") : ";
                                error += String.Join(":", colonWords, 3, colonWords.Length - 3).Trim();
                            }
                        }
                        else
                        {
                            if (colonWords[2].IndexOf("warning") < 0)
                            {
                                error = colonWords[0].Replace("/", "\\") + "(" + colonWords[1] + "," + colonWords[2] + ") : ";
                                error += String.Join(":", colonWords, 3, colonWords.Length - 3).Trim();
                            }
                            else
                            {
                                error = colonWords[0].Replace("/", "\\") + "(" + colonWords[1] + ",1) : ";
                                error += String.Join(":", colonWords, 2, colonWords.Length - 2).Trim();
                            }
                        }
                        Console.Error.WriteLine(error);
                    }

                    Console.WriteLine(str);
                }
            }
        }

        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
                return;

            startUp = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (args[1] == "build")
            {
                Build(args[0], "make");
            }
            else if (args[1] == "clean")
            {
                Build(args[0], "make clean");
            }
            else if (args[1] == "run")
            {
                Build(args[0], "make run");
            }
            else if (args[1] == "package")
            {
                Build(args[0], "make pkg");
            }
            else
                exit = true;

            while (!exit)
            {
                System.Threading.Thread.Sleep(100);
            }

            if (hasErrors)
            {
                Console.WriteLine("Errors/warnings detected!");
                Console.Write("Press any key to exit...");
                //Console.ReadKey();
            }
        }
    }
}
