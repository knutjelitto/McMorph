/* Tests for CMDLineParser
 *
 * Author  : Christian Bolterauer
 * Date    : 8-Aug-2009
 * Version : 1.0
 * Changes : 
 */

using System;
using System.Globalization;
using CMDLine;


namespace Test
{
    /// <summary>
    /// Some basic tests for CMDLineParser
    /// </summary>
    static class TestCMDLineParser
    {
        /// <summary>
        /// Run Test Cases for CMDLineParser
        /// </summary>
        public static void RunTests()
        {

            Console.WriteLine("\nStarting CMDLineParser tests:\n");
            //create parser
            CMDLineParser parser = new CMDLineParser();
            parser.throwInvalidOptionsException = false;

            //add Options
            #region addOptions
            CMDLineParser.Option DebugOption = parser.AddBoolSwitch("-Debug", "Print Command Line Parser Debug information");
            DebugOption.AddAlias("/Debug");
            CMDLineParser.Option OptionOpenfolder = parser.AddBoolSwitch("/openfolder", "Open files in folder if path is a folder");
            OptionOpenfolder.AddAlias("-openfolder");
            CMDLineParser.Option UserNameOpt = parser.AddStringParameter("-User=", "User Name", true);
            UserNameOpt.AddAlias("-U");
            CMDLineParser.NumberOption DoubleOpt = parser.AddDoubleParameter("-DoubleNum", "A Double", false);
            NumberFormatInfo numberformat = (new CultureInfo("de-DE", false)).NumberFormat;
            CMDLineParser.NumberOption FormatedNumOpt = parser.AddDoubleParameter("-NegNum", "A negativ Number", false, numberformat);
            CMDLineParser.Option IntNumOpt = parser.AddIntParameter("-IntNum", "A Integer Number", false);
            
            //Test creation of an invalid option - Throws exception
            string test = "Test creation of an invalid option";
            string testcomment = " - " + test + " - :";
            try
            {
                CMDLineParser.Option InvalidOpt = parser.AddStringParameter("-Option Nr1", "Invalid Option", false);
                Console.WriteLine("\nTestFailed: " + testcomment);
            }
            catch (CMDLineParser.CMDLineParserException ex)
            {

                Console.WriteLine("\nTestOK: " + testcomment + "Caught Error: " + ex.Message);
            } 
            #endregion

            //do tests and write results to the console window
            #region Tests
            //test missing required opt -U
            String[] testmissingreqopt = { "/Debug" }; //missing required opt -U
            TestException("missing required opt -U", parser, testmissingreqopt, typeof(CMDLineParser.MissingRequiredOptionException));

            //test neg. integer
            String[] test1 = { "-UChris", "-NegNum", "-13,56", "-IntNum", "-123", "/Debug" };
            TestOption("test neg. integer", parser, test1, IntNumOpt);

            //test Double Option
            double val = -10113.56;
            String[] testDoubleOptPoint = { "-UChris", "-DoubleNum", "-10113.56" }; //test formated double
            String[] testDoubleOptComma = { "-UChris", "-DoubleNum", "-10113,56" }; //test formated double
            TestOptionValue("testDoubleOpt-dec.point", parser, testDoubleOptPoint, DoubleOpt, val);
            TestOptionValue("testDoubleOpt-dec.comma", parser, testDoubleOptComma, DoubleOpt, val);

            //test formated (globalized) double
            String[] test2 = { "-UChris", "-NegNum", "-10.113,56", "-IntNum", "123", "/Debug" }; //test formated double
            TestOption("test formated double", parser, test2, FormatedNumOpt);

            //test wrong int format
            String[] test3 = { "-UChris", "-IntNum", "123.00", "/Debug" }; //test wrong int format
            TestException("test wrong int format", parser, test3, typeof(CMDLineParser.ParameterConversionException));

            //test InvalidOptionsException
            String[] test4 = { "-UChris", "-Inv", "-IntNum", "-123", "/Debug", "/Inv2" }; //invalid options found
            parser.throwInvalidOptionsException = true;
            TestException("invalid options found", parser, test4, typeof(CMDLineParser.InvalidOptionsException));
            //parser.Debug();

            //test custom (subclassed) option
            String[] testDate = { "-Date", "2001-11-22" }; //test custom (subclassed) option

            // New parser instance
            CMDLineParser parser2 = new CMDLineParser();

            parser2.throwInvalidOptionsException = true;
            PastDateOption DateOpt = new PastDateOption("-Date", "A test Date", false);
            parser2.AddOption(DateOpt);
            TestOption("test custom (subclassed) option", parser2, testDate, DateOpt);
            //parser2.Debug();

            //Test missing parseValue method
            MissingMethodOption missMethOpt = new MissingMethodOption("-missing", "test opt", false);
            parser2.AddOption(missMethOpt);
            string[] testmiss = { "-missing", "123" };
            TestException("Test missing parseValue method", parser2, testmiss, typeof(CMDLineParser.ParameterConversionException));

            //test 'help'
            parser2.AddHelpOption();
            parser2.isConsoleApplication = true;
            parser2.throwInvalidOptionsException = false;
            String[] testh = { "-?" };
            TestOption("test help", parser2, testh, null);

            //test generic DefaultOption ( using Convert.ChangeType..)
            //one of the two methods will fail depend on your system settings
            double dval = -10113.56;
            String[] testDefOpt1 = { "/Debug", "-NegNum", "-10113.56"}; //test formated double
            String[] testDefOpt2 = { "/Debug", "-NegNum", "-10113,56"}; //test formated double

            DefaultOption defnumop = new DefaultOption("-NegNum", "Test default Option", typeof(double), false);
            parser2.AddOption(defnumop);
            TestOptionValue("defnumop - dec.point", parser2, testDefOpt1, defnumop, dval);
            TestOptionValue("defnumop - dec.comma", parser2, testDefOpt2, defnumop, dval);

            #endregion

            parser2.Debug();

            //showCulturinfos();
        }


        #region Test Sublasses of custom Options
        class PastDateOption : CMDLineParser.Option
        {
            //constuctor
            public PastDateOption(string name, string desription, bool required)
                : base(name, desription, typeof(DateTime), true, required) { }

            //implementation of parseValue
            public override object parseValue(string parameter)
            {
                // !! system globalisation depended !!
                DateTime date = System.Convert.ToDateTime(parameter);
                DateTime now = DateTime.Now;
                //validate
                if (date > now)
                    throw new System.ArgumentException("Date: " + date + " is greater then: " + now);
                return date;
            }
        }

        class MissingMethodOption : CMDLineParser.Option
        {
            //constuctor
            public MissingMethodOption(string name, string desription, bool required)
                : base(name, desription, typeof(DateTime), true, required) { }

            //implementation of parseValue is missing
        }

        public new class DefaultOption : CMDLineParser.Option
        {
            //constuctor
            public DefaultOption(string name, string desription, Type type, bool required)
                : base(name, desription, type, true, required) { }

            //default implementation of parseValue
            public override object parseValue(string parameter)
            {
                //this generic approach may fail if command line parameters do not fit 
                //together with the system globilization settings! 
                return (Convert.ChangeType(parameter, base.Type));
            }
        }

        #endregion


        #region TestCases
        public static bool TestOptionValue(string test, CMDLineParser parser, string[] testargs, CMDLineParser.Option opt, object val)
        {
            string testcomment = " - " + test + " - :";
            try
            {
                parser.Parse(testargs);
                if (opt.Value.Equals(val))
                {
                    Console.WriteLine("\nTestOK: " + testcomment + printValue(opt));
                    return true;
                }
                else
                {
                    Console.WriteLine("\nTestFailed: " + testcomment + " Expected Value:" + val + "::Value found:" + opt.Value);
                    return false;
                }
            }
            catch (CMDLineParser.CMDLineParserException pex)
            {
                Console.WriteLine("\nTestFailed: " + testcomment + pex.Message);
            }
            return false;
        }

        public static bool TestOption(string test,CMDLineParser parser,string[] testargs,CMDLineParser.Option opt)
        {
            string testcomment= " - " + test + " - :";
            try
            {
                parser.Parse(testargs);
                Console.WriteLine("\nTestOK: " + testcomment + printValue(opt));
                return true;
            }
            catch (CMDLineParser.CMDLineParserException pex)
            {
                Console.WriteLine("\nTestFailed: " + testcomment + pex.Message);
            }
            return false;
        }
        public static string printValue(CMDLineParser.Option opt)
        {
            if (opt == null) return "";
            return("'" + opt.Name + "'=" + opt.Value);
        }

        public static bool TestException(string test , CMDLineParser parser, string[] testargs, System.Type exceptionType)
        {
            string testcomment = " - " + test + " - :";
            try
            {
                parser.Parse(testargs);
                Console.WriteLine("\nTestFailed: " + testcomment);
            }
            catch (Exception ex)
            {

                if (ex.GetType() == exceptionType)
                {
                    Console.WriteLine("\nTestOK: "+ testcomment + "Caught Error: " + ex.Message);
                    return true;
                }
                else
                    Console.WriteLine("\nTestFailed: " + testcomment + ex.Message);
            }
            return false;
        }
        #endregion


        #region ExtraStuff

        //
        public static void showCulturinfos()
        {

            // Displays several properties of the neutral cultures.
            Console.WriteLine("CULTURE ISO ISO WIN DISPLAYNAME                              ENGLISHNAME");

            int points = 0;
            int comma = 0;
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                Console.Write("{0,-7}", ci.Name);
                Console.Write(" {0,-3}", ci.TwoLetterISOLanguageName);
                Console.Write(" {0,-3}", ci.ThreeLetterISOLanguageName);
                Console.Write(" {0,-4}", ci.ThreeLetterWindowsLanguageName);

                if (!ci.IsNeutralCulture)
                {
                    Console.Write("NumSep= {0,-3}", ci.NumberFormat.NumberDecimalSeparator);
                    Console.Write("NGrSep= {0,-3}", ci.NumberFormat.NumberGroupSeparator);
                    if (ci.NumberFormat.NumberDecimalSeparator.Equals(".")) points++;
                    if (ci.NumberFormat.NumberDecimalSeparator.Equals(",")) comma++;


                }
                //Console.Write(" {0,-40}", ci.DisplayName);
                Console.WriteLine(" {0,-40}", ci.EnglishName);
            }
            int sum = points + comma;
            Console.WriteLine("DecSepPoins={0}({1}%) - DecSepComma={2}({3}%)", points, (100 * points / sum), comma, (100 * comma / sum));
        } 
        #endregion


    }
}