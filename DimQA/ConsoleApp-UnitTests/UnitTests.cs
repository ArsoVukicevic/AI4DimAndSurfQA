using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using arsClassLibrary;

namespace ConsoleApp_UnitTests
{


    public class DimQA_tests
    {
        public void test_shape_matching()
        {
            // define dimQA object
            arsClassLibrary.QA.DimQA dimQA;
            // instance
            dimQA = new arsClassLibrary.QA.DimQA();
            dimQA.setImagePath("test_images\\IMG_0034.JPG");
            string camera_parameters_path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Halcon", "Calibration", "CamParam.tup");
            dimQA.setCameraParametersPath(camera_parameters_path);
            string HalconProcedurePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Halcon", "leather_quality_control_lib.hdpl");
            dimQA.setHalconProcedurePath(HalconProcedurePath);
            string DxfPath = System.IO.Path.Combine("test_tools", "Tool#1.dxf");
            dimQA.setDXFPath(DxfPath); // "Referentni crtez 1-bez slitova.dxf"
            //
            string halcon_output_path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Halcon", "Calibration", "HalconOutput.txt");
            string[] lines = System.IO.File.ReadAllLines(halcon_output_path);
            // # FocalLenght, Sx, Sy, Kappa, Cx, Cy, ImageWidth, ImageHeight, ScaleFactor
            //   1            2   3   4      5   6   7           8            9
            List<string> parameters_string = lines[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            List<double> parameters_double = parameters_string.ConvertAll(double.Parse);
            dimQA.setScaleFactor(parameters_double[8]);
            dimQA.setTolerance(12);
            dimQA.setResultsFolder(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Halcon", "ShapeMatching", "Outputs"));
            // call the procedure
            dimQA.DoAI();
            // C# prikaze u GUI
            //dimQA.drawHalconResultsOnHalconWindows(ref this.hWindowDump);// not possible in console app
            //dimQA.drawDXF(ref this.hSmartWindowControl_DXF_View); // not possible in console app
            // Halkon upise rezultate u foldere
            dimQA.saveHalconOutputs();
        }

        public void test_calibration_using_caltab()
        {
            arsClassLibrary.HalconCalibration halconCalibration;
            halconCalibration = new HalconCalibration();
            halconCalibration.readCalibrationParametersFromTXT(System.IO.Path.Combine(halconCalibration.getCalibrationFolderPath(), "HalconInput.txt"));

            double SxIn    = 5.75e-006;
            double SyIn    = 5.75e-006;
            double FocusIn = 0.0500   ;

            halconCalibration.callHalconFunctionForCalibration(SxIn, SyIn, FocusIn);

            // write intpus for shape matching
            string calibration_input_txt_file_path = System.IO.Path.Combine(halconCalibration.getCalibrationFolderPath(), "HalconInput.txt");
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(calibration_input_txt_file_path))
            {
                writer.WriteLine("# Focus Kappa Sx         Sy         Cx  Cy  ImageWidth  ImageHeight ScaleFactor");
                writer.WriteLine(FocusIn.ToString() + " 0 " + SxIn.ToString() + " " + SyIn.ToString() + " 0   0   0           0  " + halconCalibration.getScaleFactor().ToString());
            }
        }
    }
}
