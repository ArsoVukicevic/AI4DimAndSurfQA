using System;
using arsClassLibrary;
using System.Collections.Generic;

namespace ConsoleApp_UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleApp_UnitTests.DimQA_tests test = new ConsoleApp_UnitTests.DimQA_tests();
            test.test_calibration_using_caltab();
            test.test_shape_matching();
        }
    }
}
