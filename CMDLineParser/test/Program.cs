/* Tests for Lightweight C# Command line parser
 *
 * Author  : Christian Bolterauer
 * Date    : 8-Aug-2009
 * Version : 1.0
 * Changes : 
 */

using System;
using CMDLine;

namespace ConsoleApplication1
{
    class ProgramX
    {
        static void MainX(string[] args)
        {
            //Run Tests and print result to console window
            Test.TestCMDLineParser.RunTests();                   
        }
    }
}
