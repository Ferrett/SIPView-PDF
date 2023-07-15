using ImageGear.Formats.PDF;
using ImageGear.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageGear.Core;
using System.Threading;

namespace SIPView_PDF
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ImGearLicense.SetSolutionName("SIPVIEW");
            ImGearLicense.SetSolutionKey(0xCA3EEB74, 0x7323EFB4, 0xB2BBD7DA, 0x7C473EC2);
            ImGearLicense.SetOEMLicenseKey("2.0.RBcUufkdudGCeye3R169VrQB8yG1GBeJ8UNfeB8yz9OUzfG4eUctk9cfz9kBj4erjUL46tVtQd81O36UuyzrL4u9zdVtzrR4jU8B8Zzt6yGCutV1GtNULrj3edNZuJc9jrQEGyLKV9RCcdeZLJ6Jx1N4LBNrjEG36E83LZNtk3xUOButcyQ1N46Ucy8rOfeKV18rxU84uEx9Q4Q4VJ84creJ61OyOr8tkUuUereUQKjEN3VJ63OBe9uUjWRyzJNEOULBjJeJNfuJLCNUz4zCztVJxUGrzJedxfRKL1RyG3Qtk9jEeyNfRUu3OyNyeZkyzfVdu3uKjKuyk1u3xJGBxBRdOt8BeCktjy83GCkKLERy8ruf8Ue9j9z364OEcrNyj9GUVKVCGCerN4zrGJOZNUjCNtLJ6yj3VfuCQ4kEG9eBL4z1O36UVyVfeExELEkJ61xBLKO9x3G4GdN4cdkBQJOyGCO16KLKQUjtQKufu4R9uKkBQE6Z1");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
