using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using qualityControl;

namespace arsClassLibrary
{
    namespace HalconCalibrationParameters
    {
       
        public enum calibration_types
        {
            area_scan_division,
            area_scan_division2 // opcija #2
        }

        public enum calibration_plates
        {
            caltab,
            caltab2,
            caltab3,
            caltab4
        }

        // base class - as there may be different calibration types
        public class HalconParameters
         {
            public virtual void setParameters(System.Windows.Forms.TextBox Focus, System.Windows.Forms.TextBox Kappa, System.Windows.Forms.TextBox Sx, System.Windows.Forms.TextBox Sy, System.Windows.Forms.TextBox Cx, System.Windows.Forms.TextBox Cy, System.Windows.Forms.TextBox ImageWidht, System.Windows.Forms.TextBox ImageHeight) { }
            public virtual void setParameters(double Focus, double Kappa, double Sx, double Sy, double Cx, double Cy, double ImageWidht, double ImageHeight, double ScaleFactor) { }
            public virtual void writeToTextBoxes(ref System.Windows.Forms.TextBox Focus, ref System.Windows.Forms.TextBox Kappa, ref System.Windows.Forms.TextBox Sx, ref System.Windows.Forms.TextBox Sy, ref System.Windows.Forms.TextBox Cx, ref System.Windows.Forms.TextBox Cy, ref System.Windows.Forms.TextBox ImageWidht, ref System.Windows.Forms.TextBox ImageHeight) { }
            public virtual void writeToJSON(string json_path) { }
            public virtual void writeToTxt(string txt_path) { }
            public virtual void readFromJSON(string json_path) { }
            public virtual void readFromTxt(string txt_path) { }
            public virtual void setScaleFactor(double ScaleFactor) { }
            public virtual double getScaleFactor() {return 0;}
        }

        public class area_scan_division : HalconParameters
        { 
            double Focus;
            double Kappa;
            double Sx;
            double Sy;
            double Cx;
            double Cy;
            double ImageWidht;
            double ImageHeight;
            int calibration_plate_id;
            double ScaleFactor;
            
            public override void setParameters(double Focus, double Kappa, double Sx, double Sy, double Cx, double Cy, double ImageWidht, double ImageHeight, double ScaleFactor)
            {
                this.Focus       = Focus;
                this.Kappa       = Kappa;
                this.Sx          = Sx;
                this.Sy          = Sy;
                this.Cx          = Cx;
                this.Cy          = Cy;
                this.ImageWidht  = ImageWidht;
                this.ImageHeight = ImageHeight;
                this.ScaleFactor = ScaleFactor;
            }

            public override void setScaleFactor(double ScaleFactor) 
            {
                this.ScaleFactor = ScaleFactor;
            }
            public override double getScaleFactor()
            {
                return this.ScaleFactor;
            }
            public void setCalibrationPlateId(int id)
            {
                this.calibration_plate_id = id;
            }

            public int getCalibrationPlateId()
            {
                return this.calibration_plate_id;
            }

            public string getCalibration_plates_name(int plate=-1)
            {
                if (plate<1)
                {
                    plate = this.getCalibrationPlateId();
                }
                switch (plate)
                {
                    case (int)calibration_plates.caltab:
                        return "caltab";
                    case (int)calibration_plates.caltab2:
                        return "caltab2";
                    case (int)calibration_plates.caltab3:
                        return "caltab3";
                    case (int)calibration_plates.caltab4:
                        return "caltab4";
                    default:
                        return "Error";
                }
            }

            public override void setParameters(System.Windows.Forms.TextBox Focus, System.Windows.Forms.TextBox Kappa, System.Windows.Forms.TextBox Sx, System.Windows.Forms.TextBox Sy, System.Windows.Forms.TextBox Cx, System.Windows.Forms.TextBox Cy, System.Windows.Forms.TextBox ImageWidht, System.Windows.Forms.TextBox ImageHeight)
            {
                this.Focus       = Convert.ToDouble(Focus.Text);
                this.Kappa       = Convert.ToDouble(Kappa.Text);
                this.Sx          = Convert.ToDouble(Sx.Text);
                this.Sy          = Convert.ToDouble(Sy.Text);
                this.Cx          = Convert.ToDouble(Cx.Text);
                this.Cy          = Convert.ToDouble(Cy.Text);
                this.ImageWidht  = Convert.ToDouble(ImageWidht.Text);
                this.ImageHeight = Convert.ToDouble(ImageHeight.Text);
            }
            public override void writeToTextBoxes(ref System.Windows.Forms.TextBox Focus, ref System.Windows.Forms.TextBox Kappa, ref System.Windows.Forms.TextBox Sx, ref System.Windows.Forms.TextBox Sy, ref System.Windows.Forms.TextBox Cx, ref System.Windows.Forms.TextBox Cy, ref System.Windows.Forms.TextBox ImageWidht, ref System.Windows.Forms.TextBox ImageHeight)
            {
                Focus.Text       = this.Focus.ToString("N5");
                Kappa.Text       = this.Kappa.ToString();
                Sx.Text          = this.Sx.ToString("N10");
                Sy.Text          = this.Sy.ToString("N10");
                Cx.Text          = this.Cx.ToString();
                Cy.Text          = this.Cy.ToString();
                ImageWidht.Text  = this.ImageWidht.ToString();
                ImageHeight.Text = this.ImageHeight.ToString();
            }
            public override void writeToJSON(string json_path="ars.json") 
            {
               
            }
            public override void writeToTxt(string txt_path = "ars.txt")
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(txt_path))
                {
                    writer.WriteLine("# Focus Kappa Sx    Sy  Cx  Cy  ImageWidth  ImageHeight ScaleFactor");
                    writer.WriteLine(this.Focus.ToString() + " " + this.Kappa.ToString() + " " + this.Sx.ToString() + " " + this.Sy.ToString() + " " + this.Cx.ToString() + " " + this.Cy.ToString() + " " + this.ImageWidht.ToString() + " " + this.ImageHeight.ToString() + " " + this.ScaleFactor.ToString());
                }
            }
            public override void readFromJSON(string json_path = "ars.json")
            {

            }

            public override void readFromTxt(string txt_path) 
            {
                string[] lines = System.IO.File.ReadAllLines(txt_path);
                // # FocalLenght, Sx, Sy, Kappa, Cx, Cy, ImageWidth, ImageHeight
                //   1            2   3   4      5   6   7           8
                List<string> parameters_string = lines[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                List<double> parameters_double = parameters_string.ConvertAll(double.Parse);
                this.setParameters(parameters_double[0], parameters_double[1], parameters_double[2], parameters_double[3], parameters_double[4], parameters_double[5], parameters_double[6], parameters_double[7], parameters_double[8]);
            }
        }
    }
    /// <summary>
    /// Class for halcon calibration //////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class HalconCalibration
    {        
        private string       calibrationFolder ; // where calibration data is
        private arsClassLibrary.HalconCalibrationParameters.HalconParameters calibrationParameters; // base class for various calibration types
        public  int          calibration_type  ; // check arsClassLibrary.HalconCalibrationParameters.calibration_types
        private List<string> calibration_images;
        private string       hdplLibraryPath   ; // path to the *.hdlp function
        private string       caltablePlatePath ; // path to the *.cpb file with info abut a calplate used
        private double       caltableCircleSpacing;

        public HalconCalibration(
            string HalconFolder      = "Halcon", 
            int    idCalibrationType = (int)arsClassLibrary.HalconCalibrationParameters.calibration_types.area_scan_division,
            int    idCaltable        = (int)arsClassLibrary.HalconCalibrationParameters.calibration_plates.caltab,
            string hdpl_library      = "leather_quality_control_lib.hdpl"
            )
        {
            this.setCalibrationFolder(System.IO.Path.Combine(HalconFolder, "Calibration"));
            //arsClassLibrary.IO.deleteAllFilesFromDirectory(this.getCalibrationFolderPath());
            this.setCalibrationType(idCalibrationType);
            this.setCaltablePath(idCaltable);
            this.readCalibrationImagesFolder();
            this.setHdplLibraryPath(System.IO.Path.Combine(HalconFolder, hdpl_library));
        }

        public double GetPixToMm(HTuple CamParam, double circles_spacing=5.1613, string img_path="")
        {
            return Dxf.GetPxToMm(circles_spacing, img_path, CamParam);
        }

        public void setCaltablePath(int idCaltable)
        {
            switch (idCaltable)
            {
                // area_scan_division
                case (int)arsClassLibrary.HalconCalibrationParameters.calibration_plates.caltab:
                    this.caltablePlatePath = System.IO.Path.Combine(this.getCalibrationFolderPath(), "Caltab", "calplate.cpd");
                    this.caltableCircleSpacing = 5.1613;
                    break;
                // opcija #2
                case (int)arsClassLibrary.HalconCalibrationParameters.calibration_plates.caltab2:
                    this.caltablePlatePath = System.IO.Path.Combine(this.getCalibrationFolderPath(), "Caltab", "calplate2.cpd");
                    this.caltableCircleSpacing = 9.000;
                    break;
                // opcija #3
                case (int)arsClassLibrary.HalconCalibrationParameters.calibration_plates.caltab3:
                    this.caltablePlatePath = System.IO.Path.Combine(this.getCalibrationFolderPath(), "Caltab", "calplate3.cpd");
                    this.caltableCircleSpacing = 11.000;
                    break;
                // opcija #4
                case (int)arsClassLibrary.HalconCalibrationParameters.calibration_plates.caltab4:
                    this.caltablePlatePath = System.IO.Path.Combine(this.getCalibrationFolderPath(), "Caltab", "calplate4.cpd");
                    this.caltableCircleSpacing = 11.000;
                    break;
            }
        }

        public double getCaltableCircleSpacing()
        {
            return this.caltableCircleSpacing;
        }

        public void setCalibrationParameters(double Focus, double Kappa, double Sx, double Sy, double Cx, double Cy, double ImageWidht, double ImageHeight, double ScaleFactor)
        {
            this.calibrationParameters.setParameters(Focus, Kappa, Sx, Sy, Cx, Cy, ImageWidht, ImageHeight, ScaleFactor);
        }
        public void setHdplLibraryPath(string LibraryPath)
        {
            this.hdplLibraryPath = LibraryPath;
        }
        public string getHdplLibraryPath()
        {
            return this.hdplLibraryPath;
        }
        public void writeCalibrationParametersToJSON(string json_path)
        {
            this.calibrationParameters.writeToJSON(json_path);
        }
        public void writeCalibrationParametersToTxt(string txt_path)
        {
            this.calibrationParameters.writeToTxt(txt_path);
        }
        public void readCalibrationParametersFromJSON(string json_path)
        {
            this.calibrationParameters.readFromJSON(json_path);
        }
        public virtual void readFromTxt(string json_path) { }
        public void readCalibrationParametersFromTXT(string txt_path)
        {
            this.calibrationParameters.readFromTxt(txt_path);
        }
        public void readCalibrationImagesFolder()
        {
            this.calibration_images = new List<string>();
            string calibration_images_folder = System.IO.Path.Combine(this.getCalibrationFolderPath(), "Images"); 
            List<string> imgs = arsClassLibrary.IO.getFilesWithExtensionFromDirectory(calibration_images_folder);
            foreach (string img in imgs)
                this.calibration_images.Add(img);
        }
        public void setCalibrationType(int id)
        {
            switch(id)
            {
                // area_scan_division
                case (int)arsClassLibrary.HalconCalibrationParameters.calibration_types.area_scan_division:
                    this.calibrationParameters = new arsClassLibrary.HalconCalibrationParameters.area_scan_division();
                    break;
                // opcija #2
                case (int)arsClassLibrary.HalconCalibrationParameters.calibration_types.area_scan_division2:
                    break;
            }
        }

        public void addCalibrationImage(string path)
        {
            this.calibration_images.Add(path);
        }
        public void removeCalibrationImage(string path)
        {
            if (this.calibration_images.Contains(path))
            { 
                System.IO.File.Delete(path);
                int id = this.calibration_images.IndexOf(path);
                this.calibration_images.RemoveAt(id);
            }
        }
        public void removeCalibrationImage(int id=0)
        {
            System.IO.File.Delete(this.calibration_images[id]);
            this.calibration_images.RemoveAt(id);
        }
        public int getNumberOfCalibrationImages()
        {
            return this.calibration_images.Count();
        }
        public string getCalibrationImagePath(int id=0)
        {
            return this.calibration_images[id];
        }
        public System.Drawing.Bitmap readCalibrationImageBitmap(int id = 0)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(this.calibration_images[id]);
            arsClassLibrary.IO.readImageFromFile(this.calibration_images[id], ref bmp);
            return bmp;
        }
        public void setCalibrationFolder(string folder)
        {
            this.calibrationFolder = folder;
            this.readCalibrationImagesFolder();
        }
        public string getCalibrationFolderPath()
        {
            return this.calibrationFolder;
        }

        public double getScaleFactor()
        {
            return this.calibrationParameters.getScaleFactor();
        }

        public void calibrateCameraFromImages()
        {
            List<string> calibrationImages = arsClassLibrary.IO.getFilesWithExtensionFromDirectory(this.calibrationFolder);
        }

        public void readDataFromTextboxes(System.Windows.Forms.TextBox Focus, System.Windows.Forms.TextBox Kappa, System.Windows.Forms.TextBox Sx, System.Windows.Forms.TextBox Sy, System.Windows.Forms.TextBox Cx, System.Windows.Forms.TextBox Cy, System.Windows.Forms.TextBox ImageWidht, System.Windows.Forms.TextBox ImageHeight)
        {
            this.calibrationParameters.setParameters(Focus, Kappa, Sx, Sy, Cx, Cy, ImageWidht, ImageHeight);
        }
        public void setScaleFactor(double scale)
        {
            this.calibrationParameters.setScaleFactor(scale);
        }
        public void writeDataToTextboxes(ref System.Windows.Forms.TextBox Focus, ref System.Windows.Forms.TextBox Kappa, ref System.Windows.Forms.TextBox Sx, ref System.Windows.Forms.TextBox Sy, ref System.Windows.Forms.TextBox Cx, ref System.Windows.Forms.TextBox Cy, ref System.Windows.Forms.TextBox ImageWidht, ref System.Windows.Forms.TextBox ImageHeight)
        {
            this.calibrationParameters.writeToTextBoxes(ref Focus, ref Kappa, ref Sx, ref Sy, ref Cx, ref Cy, ref ImageWidht, ref ImageHeight);
        }
        public void drawImagesIntoListView(ref System.Windows.Forms.ListView listView)
        {
            System.Windows.Forms.ImageList imgList = new System.Windows.Forms.ImageList();
            int imgW = 180;
            int imgH = 180;
            imgList.ImageSize = new System.Drawing.Size(imgW, imgH);

            for (int i = 0; i < this.getNumberOfCalibrationImages(); i++)
            {
                using (System.Drawing.Image tmp = System.Drawing.Image.FromFile(this.getCalibrationImagePath(i)))
                {
                    imgList.Images.Add(new System.Drawing.Bitmap(tmp, new System.Drawing.Size(imgW, imgH)));
                }
            }
            listView.Items.Clear();
            listView.View = System.Windows.Forms.View.Details;
            listView.Columns.Clear();
            listView.Columns.Add("Images", 500);
            listView.AutoResizeColumn(0, System.Windows.Forms.ColumnHeaderAutoResizeStyle.HeaderSize);
            listView.MultiSelect = false;
            for (int i = 0; i < this.getNumberOfCalibrationImages(); i++)
            {
                listView.Items.Add(System.IO.Path.GetFileName(this.getCalibrationImagePath(i)), i);
            }
            listView.SmallImageList = imgList;
        }
        public void drawImageIntoPictureBox(ref System.Windows.Forms.PictureBox pictureBox, int imageID=0)
        {
            System.Drawing.Size size = new System.Drawing.Size(pictureBox.Size.Width, pictureBox.Size.Height);
            using (System.Drawing.Image temp = System.Drawing.Image.FromFile(this.getCalibrationImagePath(imageID)))
            {
                pictureBox.Image = new System.Drawing.Bitmap(temp, size);
            }
            GC.Collect();
        }

        public void callHalconFunctionForCalibration(double SxIn=5.75e-006, double SyIn=5.75e-006, double FocusIn=0.0500)
        {
            HTuple ImagesPath = System.IO.Path.Combine(this.getCalibrationFolderPath(), "Images"); // putanja do slika
            HTuple CalibPlate = System.IO.Path.Combine(this.getCalibrationFolderPath(),"Caltab", "calplate.cpd"); // putanja do slika

            HDevEngine engine = new HDevEngine();
            engine.SetProcedurePath(this.getHdplLibraryPath());

            HDevProcedure     procedure     = new HDevProcedure("cam_calib")  ;
            HDevProcedureCall procedureCall = new HDevProcedureCall(procedure);

            procedureCall.SetInputCtrlParamTuple("ImagePath", ImagesPath);
            procedureCall.SetInputCtrlParamTuple("CalibObjDescr", CalibPlate);
            procedureCall.SetInputCtrlParamTuple("SxIn", SxIn);
            procedureCall.SetInputCtrlParamTuple("SyIn", SyIn);
            procedureCall.SetInputCtrlParamTuple("FocusIn", FocusIn);

            procedureCall.Execute();

            HTuple Sx          = procedureCall.GetOutputCtrlParamTuple("Sx")              ; // input
            HTuple Sy          = procedureCall.GetOutputCtrlParamTuple("Sy")              ; // input
            HTuple Kappa       = procedureCall.GetOutputCtrlParamTuple("Kappa")           ; // input

            HTuple Focus       = procedureCall.GetOutputCtrlParamTuple("Focus")           ;
            HTuple Cx          = procedureCall.GetOutputCtrlParamTuple("Cx")              ;
            HTuple Cy          = procedureCall.GetOutputCtrlParamTuple("Cy")              ;
            HTuple ImageWidht  = procedureCall.GetOutputCtrlParamTuple("ImgWidth")        ;
            HTuple ImageHeight = procedureCall.GetOutputCtrlParamTuple("ImgHeight")       ;
            HTuple CamParam    = procedureCall.GetOutputCtrlParamTuple("CameraParameters");

            //HOperatorSet.SetCurrentDir(@"C:\Temp");

            // pixel spacing
            string caltab_img0_path = this.getCalibrationImagePath(7);
            double circle_spacing  = this.getCaltableCircleSpacing();
            double scale_factor    = this.GetPixToMm(CamParam, circle_spacing, caltab_img0_path);

            this.setCalibrationParameters(Focus.D, Kappa.D, Sx.D, Sy.D, Cx.D, Cy.D, ImageWidht.D, ImageHeight.D, scale_factor);

            CamParam.WriteTuple(System.IO.Path.Combine("Halcon", "Calibration" , "CamParam.tup"));

        }
    }
}
