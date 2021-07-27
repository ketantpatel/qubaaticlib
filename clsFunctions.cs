using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;

/// <summary>
/// 	Summary description for clsFunctions
/// </summary>
public class clsFunctions
{
    private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
    private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;
    public int[] image = new int[6];
    public string usercode = "";
    // Define supported password characters divided into groups.
    // You can add (or remove) characters to (from) these groups.
    //private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
    //private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
    //private static string PASSWORD_CHARS_NUMERIC = "123456789ABCDEFGHJKLMNPQRSTWXYZ*$-+=";
    //private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

    public static string Generate()
    {
        return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                        DEFAULT_MAX_PASSWORD_LENGTH);
    }

    /// <summary>
    /// 	Generates a random password of the exact length.
    /// </summary>
    /// <param name = "length">
    /// 	Exact password length.
    /// </param>
    /// <returns>
    /// 	Randomly generated password.
    /// </returns>
    public static string Generate(int length)
    {
        return Generate(length, length);
    }

    /// <summary>
    /// 	Generates a random password.
    /// </summary>
    /// <param name = "minLength">
    /// 	Minimum password length.
    /// </param>
    /// <param name = "maxLength">
    /// 	Maximum password length.
    /// </param>
    /// <returns>
    /// 	Randomly generated password.
    /// </returns>
    /// <remarks>
    /// 	The length of the generated password will be determined at
    /// 	random and it will fall with the range determined by the
    /// 	function parameters.
    /// </remarks>
    public static string Generate(int minLength,
                                  int maxLength)
    {
        // Make sure that input parameters are valid.
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
            return null;

        // Create a local array containing supported password characters
        // grouped by types. You can remove character groups from this
        // array, but doing so will weaken the password strength.
        char[][] charGroups = new char[][]
                                  {
                                      //PASSWORD_CHARS_NUMERIC.ToCharArray()
                                  };

        // Use this array to track the number of unused characters in each
        // character group.
        int[] charsLeftInGroup = new int[charGroups.Length];

        // Initially, all characters in each group are not used.
        for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

        // Use this array to track (iterate through) unused character groups.
        int[] leftGroupsOrder = new int[charGroups.Length];

        // Initially, all character groups are not used.
        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;


        byte[] randomBytes = new byte[4];

        // Generate 4 random bytes.
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomBytes);

        // Convert 4 bytes into a 32-bit integer value.
        int seed = (randomBytes[0] & 0x7f) << 24 |
                   randomBytes[1] << 16 |
                   randomBytes[2] << 8 |
                   randomBytes[3];

        // Now, this is real randomization.
        Random random = new Random(seed);

        // This array will hold password characters.
        char[] password = null;


        if (minLength < maxLength)
            password = new char[random.Next(minLength, maxLength + 1)];
        else
            password = new char[minLength];


        int nextCharIdx;


        int nextGroupIdx;


        int nextLeftGroupsOrderIdx;


        int lastCharIdx;


        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;


        for (int i = 0; i < password.Length; i++)
        {
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0,
                                                     lastLeftGroupsOrderIdx);

            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];


            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;


            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);

            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            if (lastCharIdx == 0)
                charsLeftInGroup[nextGroupIdx] =
                    charGroups[nextGroupIdx].Length;

            else
            {
                if (lastCharIdx != nextCharIdx)
                {
                    char temp = charGroups[nextGroupIdx][lastCharIdx];
                    charGroups[nextGroupIdx][lastCharIdx] =
                        charGroups[nextGroupIdx][nextCharIdx];
                    charGroups[nextGroupIdx][nextCharIdx] = temp;
                }
                charsLeftInGroup[nextGroupIdx]--;
            }

            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            else
            {
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] =
                        leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                }

                lastLeftGroupsOrderIdx--;
            }
        }


        return new string(password);
    }

    public static string CreateRandomPassword(int passwordLength)
    {
        string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        char[] chars = new char[passwordLength];
        Random rd = new Random();

        for (int i = 0; i < passwordLength; i++)
        {
            chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
        }

        return new string(chars);
    }

    public static string FormatString(object sNum, string Format)
    {
        string Num;
        if (sNum == null)
        {
            Num = "0";
        }
        else
        {
            Num = sNum.ToString();
        }

        decimal d = 0;
        if (Num.Trim() == "")
        {
            switch (Format)
            {
                case "0":
                    Num = "0";
                    break;
                case "0.0":
                    Num = "0.0";
                    break;
                case "0.00":
                    Num = "0.00";
                    break;
                case "0.000":
                    Num = "0.000";
                    break;
                case "0.0000":
                    Num = "0.0000";
                    break;
                default:
                    Num = "0.00";
                    break;
            }
        }
        try //Making Round
        {
            if (Num.Contains("."))
            {
                Num = Num + "0000";
            }
            else
            {
                Num = Num + ".0000";
            }
            d = decimal.Parse(Num);
            switch (Format)
            {
                case "0":
                    d = Math.Round(d, 0);
                    break;
                case "0.0":
                    d = Math.Round(d, 1);
                    break;
                case "0.00":
                    d = Math.Round(d, 2);
                    break;
                case "0.000":
                    d = Math.Round(d, 3);
                    break;
                case "0.0000":
                    d = Math.Round(d, 4);
                    break;
                default:
                    d = Math.Round(d, 2);
                    break;
            }
        }
        catch
        {
        }

        return d.ToString();
    }

    public static string sendSms(string Mobile_Number, string Message)
    {
        string stringpost = "User=auro&passwd=12345_remove&mobilenumber=" + Mobile_Number + "&message=" + Message +
                            "&MTYPE=N&sid=MY JOBS";
        //Response.Write(stringpost)
        string functionReturnValue = null;
        functionReturnValue = "";

        HttpWebRequest objWebRequest = null;
        HttpWebResponse objWebResponse = null;
        StreamWriter objStreamWriter = null;
        StreamReader objStreamReader = null;

        try
        {
            string stringResult = null;

            objWebRequest = (HttpWebRequest) WebRequest.Create("http://sms.test.co.in/WebserviceSMS.aspx");
            //domain name: Domain name Replace With Your Domain  
            objWebRequest.Method = "POST";

            // Response.Write(objWebRequest)

            // Use below code if you want to SETUP PROXY.
            //Parameters to pass: 1. ProxyAddress 2. Port
            //You can find both the parameters in Connection settings of your internet explorer.


            // If You are In the proxy Then You Uncomment the below lines and Enter IP And Port Number


            //System.Net.WebProxy myProxy = new System.Net.WebProxy("192.168.1.108", 6666);
            //myProxy.BypassProxyOnLocal = true;
            //objWebRequest.Proxy = myProxy;

            objWebRequest.ContentType = "application/x-www-form-urlencoded";

            objStreamWriter = new StreamWriter(objWebRequest.GetRequestStream());
            objStreamWriter.Write(stringpost);
            objStreamWriter.Flush();
            objStreamWriter.Close();

            objWebResponse = (HttpWebResponse) objWebRequest.GetResponse();


            objWebResponse = (HttpWebResponse) objWebRequest.GetResponse();

            objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
            stringResult = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            return (stringResult);
        }
        catch (Exception ex)
        {
            return (ex.ToString());
        }
        finally
        {
            if ((objStreamWriter != null))
            {
                objStreamWriter.Close();
            }
            if ((objStreamReader != null))
            {
                objStreamReader.Close();
            }
            objWebRequest = null;
            objWebResponse = null;
        }
    }

    public static void sendEmail(string Email, string Subject, string Message)
    {
        string stringpost = "User=&Email=" + Email + "&subject=" + Subject + "&message=" + Message + "&MTYPE=N";
        //Response.Write(stringpost)
        //string functionReturnValue = null;

        //functionReturnValue = "";    
    }

    public static int MonthToInt(string monthName)
    {
        int monthNum = 0;
        switch (monthName)
        {
            case "January":
            case "Jan":
            case "Jan.":
                monthNum = 1;
                break;
            case "February":
            case "Feb":
            case "Feb.":
                monthNum = 2;
                break;
            case "March":
            case "Mar":
            case "Mar.":
                monthNum = 3;
                break;
            case "April":
            case "Apr":
            case "Apr.":
                monthNum = 4;
                break;
            case "May":
                monthNum = 5;
                break;
            case "June":
            case "Jun":
            case "Jun.":
                monthNum = 6;
                break;
            case "July":
            case "Jul":
            case "Jul.":
                monthNum = 7;
                break;
            case "August":
            case "Aug":
            case "Aug.":
                monthNum = 8;
                break;
            case "September":
            case "Sep":
            case "Sep.":
                monthNum = 9;
                break;
            case "October":
            case "Oct":
            case "Oct.":
                monthNum = 10;
                break;
            case "November":
            case "Nov":
            case "Nov.":
                monthNum = 11;
                break;
            case "December":
            case "Dec":
            case "Dec.":
                monthNum = 12;
                break;
            default:
                break;
        }
        return monthNum;
    }

    public static string ReplaceQuotes(string STRING)
    {
        return STRING.Replace("'", "''");
    }

    public static string MonthToString(int monthName)
    {
        string monthNum = "";
        switch (monthName)
        {
            case 1:
                monthNum = "Jan";
                break;
            case 2:
                monthNum = "Feb";
                break;
            case 3:
                monthNum = "Mar";
                break;
            case 4:
                monthNum = "Apr";
                break;
            case 5:
                monthNum = "May";
                break;
            case 6:
                monthNum = "Jun";
                break;
            case 7:
                monthNum = "Jul";
                break;
            case 8:
                monthNum = "Aug";
                break;
            case 9:
                monthNum = "Sep";
                break;
            case 10:
                monthNum = "Oct";
                break;
            case 11:
                monthNum = "Nov";
                break;
            case 12:
                monthNum = "Dec";
                break;
            default:
                break;
        }
        return monthNum;
    }
}