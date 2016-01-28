using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace File_Folder_Note
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (IsRunAsAdministrator())
                {
                    RegistyCheck();
                    return;
                }
                var fileFolderPath = args[0];
                //Console.WriteLine(fileFolderPath);
                //Console.ReadKey();
                //return;
                var text = Clipboard.GetText(TextDataFormat.Text).Trim();
                if (IsFile(fileFolderPath))
                {
                    var file = Path.ChangeExtension(fileFolderPath, ".txt");
                    if (!File.Exists(file))
                    {
                        File.WriteAllText(file, text);
                    }
                    if (File.Exists(file)) Process.Start(file);
                }
                else
                {
                    var dirInfo = new DirectoryInfo(fileFolderPath);
                    var dirName = dirInfo.Name;
                    var dirPath = dirInfo.Parent;
                    if (dirPath != null)
                    {
                        string file = "";
                        if (args.Length > 1 && args[1] == "Background")
                        {
                            file = Path.Combine(dirPath.FullName, dirName, dirName + ".txt");
                        }
                        else
                        {
                            file = Path.Combine(dirPath.FullName, dirName + ".txt");
                        }
                        
                        if (!File.Exists(file))
                        {
                            File.WriteAllText(file, text);
                        }
                        if (File.Exists(file)) Process.Start(file);
                    }
                    //Console.WriteLine(dirPath.FullName);
                    //Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("Error Log.txt",ex.ToString() + "\r\n*********************************************************************\r\n" );
                Environment.Exit(0);
            }
            
        }
        public static bool IsFile(string path)
        {
            if (Path.GetExtension(path) == "")
            {
                return false;
            }
            return true;
        }

        public static void RegistyCheck()
        {
            var fullName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //var thisPath = Directory.GetCurrentDirectory();
            //var iconPath = Path.Combine(thisPath, "Note.ico");
            var iconPathReg = "\"" + fullName + "\"" + "";
            var exePathReg = "\"" + fullName + "\"" + " \"%1\"";
            var exePathReg2 = "\"" + fullName + "\"" + " \"%V\" Background";
            //File
            Registry.SetValue(@"HKEY_CLASSES_ROOT\*\shell\Create File Note", "Icon", iconPathReg, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\*\shell\Create File Note\command", "", exePathReg, RegistryValueKind.String);
            //Folder
            Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\shell\Create Folder Note", "Icon", iconPathReg, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\shell\Create Folder Note\command", "", exePathReg, RegistryValueKind.String);
            //Inside folder
            Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\Background\shell\Create Folder Note", "Icon", iconPathReg, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\Background\shell\Create Folder Note\command", "", exePathReg2, RegistryValueKind.String);
            MessageBox.Show("Success Install", "Success Install", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Registry.SetValue(@"", "", Path.Combine(thisPath, "Note.ico"), RegistryValueKind.String);
            //Registry.SetValue(@"", "", "", RegistryValueKind.String);
            //Registry.SetValue(@"", "", "", RegistryValueKind.String);
            //Registry.SetValue(@"", "", "", RegistryValueKind.String);
            //Registry.SetValue(@"", "", "", RegistryValueKind.String);
            //Registry.SetValue(@"", "", "", RegistryValueKind.String);
        }
        private static bool IsRunAsAdministrator()
        {
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            WindowsPrincipal wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
/*
[HKEY_CLASSES_ROOT\*\shell\Create Note This]
"Icon"="D:\\Program_X86\\Registry\\Registry File\\Add Text File Note\\Note.ico"
[HKEY_CLASSES_ROOT\*\shell\Create Note This\command]
@="\"D:\\Visual Studio\\Projects\\File Folder Note\\File Folder Note\\bin\\Debug\\File Folder Note.exe\" \"%1\""

[HKEY_CLASSES_ROOT\Directory\shell\Create Note This]
"Icon"="D:\\Program_X86\\Registry\\Registry File\\Add Text File Note\\Note.ico"
[HKEY_CLASSES_ROOT\Directory\shell\Create Note This\command]
@="\"D:\\Visual Studio\\Projects\\File Folder Note\\File Folder Note\\bin\\Debug\\File Folder Note.exe\" \"%1\""

;****************************************************
;[HKEY_CLASSES_ROOT\Directory\Shell]
;@="none"

;[HKEY_CLASSES_ROOT\Directory\shell\Create Note This]
	
[HKEY_CLASSES_ROOT\Directory\Background\shell\Create Note This]
@="Create Note This"
"Icon"="D:\\Program_X86\\Registry\\Registry File\\Add Text File Note\\Note.ico"
;"MUIVerb"="git bash here"
;"Position"="bottom" 
"NoWorkingDirectory"=""
;[HKEY_CLASSES_ROOT\Directory\shell\Create Note This\command] 
	
[HKEY_CLASSES_ROOT\Directory\Background\shell\Create Note This\command]
@="\"D:\\Visual Studio\\Projects\\File Folder Note\\File Folder Note\\bin\\Debug\\File Folder Note.exe\" \"%V\" \"Background\""*/