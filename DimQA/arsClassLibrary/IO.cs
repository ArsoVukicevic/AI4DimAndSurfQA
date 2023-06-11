using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arsClassLibrary
{
    public static class IO
    {
        public static string getCurrentDirectory()
        {
            string rez = System.IO.Directory.GetCurrentDirectory();
            return rez;
        }
        public static void deleteAllFilesFromDirectory(string dirPath)
        {
            string[] files = System.IO.Directory.GetFiles(dirPath);
            foreach (string file in files)
                System.IO.File.Delete(file);
        }

        public static void deleteDirectory(string path)
        {           
            if (System.IO.Directory.Exists(path))
            {
                System.IO.Directory.Delete(path, true);
            }
        }

        public static List<string> getFilesWithExtensionFromDirectory(string dirPath, List<string> extensions = null)
        {
            if (extensions==null)
            {
                extensions = new List<string> { "jpg", "gif", "png" };
            }
            List<string> files = System.IO.Directory.GetFiles(dirPath, "*.*", System.IO.SearchOption.AllDirectories)
                .Where(s => extensions.Contains(System.IO.Path.GetExtension(s).TrimStart('.').ToLowerInvariant())).ToList();
            return files;
        }

        public static void readImageFromFile(string imgFile, ref System.Drawing.Bitmap img)
        {
            using (System.Drawing.Bitmap tmp = new System.Drawing.Bitmap(imgFile))
            {
                img = new System.Drawing.Bitmap(tmp);
            }
            GC.Collect();
        }

        public static List<double> readDataFromHalconOutptutFile(string FileToRead)
        {
            List<double> rez_points = new List<double>();
            //
            String[] lines = System.IO.File.ReadAllLines(FileToRead);
            //
            int number_of_points = Convert.ToInt32(lines[0]);
            for(int i=1; i<=number_of_points; i++)
            {
                string coordinate_string = lines[i].Split(' ')[1];
                coordinate_string = coordinate_string.Replace('.', ','); // UK/US notation
                double coordinate = Convert.ToDouble(coordinate_string);
                rez_points.Add(coordinate);
            }
            return rez_points;
        }

        // Halcon output contour folder path contains
        //   col folder - that contains N files conatinng list of column coordinates
        //   row folder - that contains N files conatinng list of row    coordinates
        public static List<List<List<double>>> readContoursFromHalconOutptutFolder(string folder_to_read)
        {
            string col_dir = System.IO.Path.Combine(folder_to_read, "col");
            string row_dir = System.IO.Path.Combine(folder_to_read, "row");
            string[] col_files = System.IO.Directory.GetFiles(col_dir);
            string[] row_files = System.IO.Directory.GetFiles(row_dir);
            //
            int number_of_contours = col_files.Count();
            //
            List<List<List<double>>> Contours = new List<List<List<double>>>();
            for (int iContour=0; iContour<number_of_contours; iContour++)
            {
                List<double> row = arsClassLibrary.IO.readDataFromHalconOutptutFile(row_files[iContour]);
                List<double> col = arsClassLibrary.IO.readDataFromHalconOutptutFile(col_files[iContour]);
                //
                List<List<double>> Contour = new List<List<double>>();
                for (int iPoint=0; iPoint<row.Count(); iPoint++)
                {
                    List<double> point = new List<double>();
                    point.Add(row[iPoint]);
                    point.Add(col[iPoint]);
                    Contour.Add(point);
                }
                Contours.Add(Contour);
            }
            return Contours;
        }

        public static string selectFolderDialog(string InitialDirectory = null)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            string folderPath = null;
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = System.IO.Path.GetDirectoryName(folderBrowser.SelectedPath);
            }
            return folderPath;
        }

        public static string selectFileDialog(string InitialDirectory=null)
        {
            System.Windows.Forms.OpenFileDialog fileBrowser = new System.Windows.Forms.OpenFileDialog();
            if (InitialDirectory != null)
            {
                fileBrowser.InitialDirectory = InitialDirectory;
            }
            string filePath = null;
            if (fileBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = fileBrowser.FileName;
            }
            return filePath;
        }
    }
}
