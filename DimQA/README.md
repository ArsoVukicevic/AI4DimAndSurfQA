# SurfQA - surface inspection module

The module for surface inspection is developed upon the Halcon MVTec library https://www.mvtec.com/products/halcon. Accordingly, "halcondontet" and "hdevelopdotnet" DLL files are added to the arsClassLibrary references. 

The source code is available in the C# solution, which structure is illustrated below: 

![project image](https://github.com/ArsoVukicevic/AI4DimAndSurfQA/blob/main/DimQA/VS_project.jpg)

It consists of two projects, one class library, and one console application. 

The arsClassLibrary project contains three classes:
*HalconCalibration - containing  code for calibrating the camera using the MVTec calibration table (see detailed video tutorial here: https://www.youtube.com/watch?v=iEjH244KRbw)
*IO - contains helper functions for reading/writing data
*QA - contains DimQA code for performing dimensional inspection

For end-users, of interest is the DoAI function of QA.DimQA class:
```C#
public void DoAI()
            {
                // set HDevEngine object
                HDevEngine engine = new HDevEngine();
                // set path to the *.hdlp function (h develop function, in this project this is "leather_quality_control_lib.hdpl")
                engine.SetProcedurePath(this.getHalconProcedurePath());

                // get a "match_and_get_contours" procedure from the hdlp
                HDevProcedure procedure = new HDevProcedure("match_and_get_contours");
                HDevProcedureCall procedureCall = new HDevProcedureCall(procedure);

                // set input parameters for "match_and_get_contours"
                procedureCall.SetInputCtrlParamTuple("InputImageDestination", this.getImagePath());
                procedureCall.SetInputCtrlParamTuple("InputCameraParamDestination", this.getCameraParametersPath());
                procedureCall.SetInputCtrlParamTuple("InputDXFDestination", this.getDXFPath());
                procedureCall.SetInputCtrlParamTuple("ScaleFactor", this.getScaleFactor());
                procedureCall.SetInputCtrlParamTuple("tolerance", this.getTolerance());

                // execute procedure
                procedureCall.Execute();

                //Take Halcon outputs
                this.ImageRaw           = procedureCall.GetOutputIconicParamImage("Image");
                this.ImageRawMirrorGray = procedureCall.GetOutputIconicParamImage("ImageMirror");
                this.Deformations       = procedureCall.GetOutputIconicParamXld("Deformations");
                this.DXFContours        = procedureCall.GetOutputIconicParamXld("DXFContours");
                this.ImgContours        = procedureCall.GetOutputIconicParamXld("ImgContours");
            }
```

The project "ConsoleApp-UnitTests" consists of two unit-test that demonstrate how to perform two key steps: 1) camera calibration and 2) dimensional inspection 
* public void test_shape_matching()
* public void test_calibration_using_caltab()

# test_calibration_using_caltab()

## Step #1 - Prepare input files to run the calibration procedure

Before performing shape matching, the camera needs to be calibrated. In this project, we used MVTec caltab calibration plate. There is the official video tutorial from MVTec how to do it: https://www.youtube.com/watch?v=iEjH244KRbw.

Calibration input files are placed in "Halcon/Calibration" folder: 

Specifically, in the "Caltab" folder you can find the following files:
* caltab.jpg      - image of caltab
* caltab.ps       - if you want to print your own calibration table
* calplate.cpd    - info file, that will be read by the HDevelop code to get calibration plate info
* calbtab.hdevelp - is hdevelop code, where you can create your own caltab (e.g. bigger one) - create_caltab documentation is available online at: https://www.mvtec.com/doc/halcon/12/en/create_caltab.html

Calibration input images are placed in "Halcon/Images". You need to have a physical calibration table, and a fixed camera and follow the instructions from the video above to get a sufficient number of images. 

## Step #2 - Run the calibration procedure

public void test_calibration_using_caltab()
        {
            // set inputs
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
```

