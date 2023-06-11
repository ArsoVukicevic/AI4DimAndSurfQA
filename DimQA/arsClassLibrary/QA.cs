using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace arsClassLibrary
{
    namespace QA
    {
        public class DimQA
        {
            // Halcon inputs
            private string ImagePath;
            private string DXFPath;
            private string CameraParametersPath;
            private double ScaleFactor;
            private double Tolerance;
            private string HalconProcedurePath;
            // Halcon outputs
            private HImage ImageRaw;
            private HImage ImageRawMirrorGray;
            private HXLD Deformations;
            private HXLD DXFContours;
            private HXLD ImgContours;
            // Output folders
            private string Results_folder;
            
            // constructors
            public DimQA()
            {
                
            }
            // getters / setters
            public void setHalconProcedurePath(string HalconProcedurePath)
            {
                this.HalconProcedurePath = HalconProcedurePath;
            }
            public string getHalconProcedurePath()
            {
                return this.HalconProcedurePath;
            }
            public void setTolerance(double Tolerance)
            {
                this.Tolerance = Tolerance;
            }
            public double getTolerance()
            {
                return this.Tolerance;
            }
            public void setScaleFactor(double ScaleFactor)
            {
                this.ScaleFactor = ScaleFactor;
            }
            public double getScaleFactor()
            {
                return this.ScaleFactor;
            }
            public void setImagePath(string img_path)
            {
                this.ImagePath = img_path;
            }
            public string getImagePath()
            {
                return this.ImagePath;
            }
            public void setCameraParametersPath(string CamParamPath)
            {
                this.CameraParametersPath = CamParamPath;
            }
            public string getCameraParametersPath()
            {
                return this.CameraParametersPath;
            }
            public void setDXFPath(string DXFPath)
            {
                this.DXFPath = DXFPath;
            }
            public string getDXFPath()
            {
                return this.DXFPath;
            }
            public void setResultsFolder(string path)
            {
                this.Results_folder = path;
            }
            public string getResultsFolder()
            {
                return this.Results_folder;
            }
            // Methods

            public void DoAI()
            {
                HDevEngine engine = new HDevEngine();
                engine.SetProcedurePath(this.getHalconProcedurePath());

                HDevProcedure procedure = new HDevProcedure("match_and_get_contours");
                HDevProcedureCall procedureCall = new HDevProcedureCall(procedure);

                procedureCall.SetInputCtrlParamTuple("InputImageDestination", this.getImagePath());
                procedureCall.SetInputCtrlParamTuple("InputCameraParamDestination", this.getCameraParametersPath());
                procedureCall.SetInputCtrlParamTuple("InputDXFDestination", this.getDXFPath());
                procedureCall.SetInputCtrlParamTuple("ScaleFactor", this.getScaleFactor());
                procedureCall.SetInputCtrlParamTuple("tolerance", this.getTolerance());

                procedureCall.Execute();

                // take Halcon outputs
                this.ImageRaw = procedureCall.GetOutputIconicParamImage("Image");
                this.ImageRawMirrorGray = procedureCall.GetOutputIconicParamImage("ImageMirror");
                this.Deformations = procedureCall.GetOutputIconicParamXld("Deformations");
                this.DXFContours = procedureCall.GetOutputIconicParamXld("DXFContours");
                this.ImgContours = procedureCall.GetOutputIconicParamXld("ImgContours");
            }

            public void drawHalconResultsOnHalconWindows(ref HalconDotNet.HSmartWindowControl hWindow)
            {
                hWindow.HalconWindow.DispImage(this.ImageRawMirrorGray);
                hWindow.SetFullImagePart();
                hWindow.HalconWindow.SetLineWidth(1);
                hWindow.HalconWindow.SetColor("yellow");
                hWindow.HalconWindow.DispObj(this.ImgContours);
                hWindow.HalconWindow.SetColor("green");
                hWindow.HalconWindow.DispObj(this.DXFContours);
                hWindow.HalconWindow.SetColor("red");
                hWindow.HalconWindow.SetLineWidth(2);
                hWindow.HalconWindow.DispObj(this.Deformations);
            }

            public void drawDXF(ref HalconDotNet.HSmartWindowControl hWindow)
            {
                HObject tmp_img_hObject = new HObject();
                this.ImageRawMirrorGray.GetImageSize(out HTuple w, out HTuple h);
                HOperatorSet.GenImageConst(out tmp_img_hObject, "byte", w, h);
                HImage img = new HImage(tmp_img_hObject);
                hWindow.HalconWindow.DispImage(img);
                hWindow.SetFullImagePart();
                hWindow.HalconWindow.SetColor("green");
                hWindow.HalconWindow.DispObj(this.DXFContours);
            }

            public void saveHalconOutputs()
            {
                ;
                //
                HDevEngine engine = new HDevEngine();
                engine.SetProcedurePath(this.getHalconProcedurePath());

                HDevProcedure procedure         = new HDevProcedure("write_contours");
                HDevProcedureCall procedureCall = new HDevProcedureCall(procedure)   ;

                procedureCall.SetInputIconicParamObject("DXFContours" , this.DXFContours);
                procedureCall.SetInputIconicParamObject("ImgContours" , this.ImgContours);
                procedureCall.SetInputIconicParamObject("Deformations", this.Deformations);

                arsClassLibrary.IO.deleteDirectory(this.getResultsFolder());
                
                procedureCall.SetInputCtrlParamTuple("OutputDestination", this.getResultsFolder());
                procedureCall.SetInputCtrlParamTuple("DXFContOutput"    , "dxf_cont");
                procedureCall.SetInputCtrlParamTuple("ImgContOutput"    , "img_cont");
                procedureCall.SetInputCtrlParamTuple("DeformContOutput" , "deform_cont");

                string current_dir = System.IO.Directory.GetCurrentDirectory();
                procedureCall.Execute();
                System.IO.Directory.SetCurrentDirectory(current_dir);
            }

        }

    }
}
