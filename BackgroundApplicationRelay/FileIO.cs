using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
namespace BackgroundApplicationRelay
{
   public sealed class FileIO
    {
        String notes;
        String filename = @"\\192.168.0.110\c$\PlantyData";
        bool modulePwr=false;
        public IAsyncAction writeTOFileAs (String notes)
        {
            this.notes = notes;
            return Task.Run(WriteToFile).AsAsyncAction();
        }
        private async Task WriteToFile()
        {
            if(modulePwr)
            {
                try
                {
                    if (!File.Exists(filename))
                    {
                        // File.Create(filename);
                        using (StreamWriter sw = File.CreateText(filename))
                        {
                            sw.WriteLine("Starting Log..");
                           
                        }
                    }
                    else
                    {
                        FileInfo fi = new FileInfo(filename);

                        if (fi.Length > 16777216)
                        {
                            File.Create(filename);
                        }

                    }
                    var x = System.IO.File.ReadAllText(filename);
                    x += "\n";
                    x += DateTime.Now.ToString() + "," + notes;
                    File.WriteAllText(filename, notes);
                }
                catch (Exception ex)
                {


                }
            }
           
        }
    }
}
