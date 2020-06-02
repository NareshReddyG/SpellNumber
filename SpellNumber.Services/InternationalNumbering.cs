using System.Text;

namespace SpellNumber.Services
{
    public class InternationalNumbering : ISpellNumber
    {
        private readonly string[] _0to19 = new string[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", };
        private readonly string[] _endingWithty = new string[] { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private readonly string[] _0s = new string[] { "", "", "hundred", "thousand", "", "", "million", "", "", "billion", "", "", "trillion", "", "", "quadrillion", "", "", "quintillion", "", "", "sextillion", "", "", "septillion", };
        
        public string SpellNumberInWords(double number)
        {
            if (number.ToString().Contains("E+"))
            {
                return "Error: Numbers between -999999999999 TO 999999999999 only handeled";
            }

            StringBuilder numberInWords = new StringBuilder();

            bool isNegative = false;
            if (number < 0)
            {
                isNegative = true;
            }
            string sNumber = number.ToString();
            double wholeNumber = number;

            if (!sNumber.Contains("."))
            {
                if (double.Parse(sNumber) > wholeNumber)
                {
                    wholeNumber = double.Parse(sNumber);
                }

                if (isNegative)
                {
                    if (double.Parse(sNumber) < wholeNumber)
                    {
                        wholeNumber = double.Parse(sNumber);
                    }
                }
            }

            string fractionalPart = null;

            if (sNumber.Contains("."))
            {
                wholeNumber = double.Parse(sNumber.Substring(0, sNumber.IndexOf(".")));
                fractionalPart = sNumber.Substring(sNumber.IndexOf(".") + 1);
            }

            if (isNegative)
            {
                wholeNumber *= -1;
            }

            if (wholeNumber >= 0 && wholeNumber <= 99)
            {
                numberInWords.Append(Handle0TO99(wholeNumber));
            }

            if (wholeNumber >= 100 && wholeNumber <= 999)
            {
                numberInWords.Append(Handle100TO999(wholeNumber));
            }

            if (wholeNumber >= 1000 && wholeNumber <= 9999)
            {
                numberInWords.Append(Handle1000TO9999(wholeNumber));
            }

            if (wholeNumber >= 10000 && wholeNumber <= 99999)
            {
                numberInWords.Append(Handle10000TO99999(wholeNumber));
            }

            if (wholeNumber >= 100000 && wholeNumber <= 999999)
            {
                numberInWords.Append(Handle100000TO999999(wholeNumber));
            }

            if (wholeNumber >= 1000000 && wholeNumber <= 9999999)
            {
                numberInWords.Append(Handle1000000TO9999999(wholeNumber));
            }

            if (wholeNumber >= 10000000 && wholeNumber <= 99999999)
            {
                numberInWords.Append(Handle10000000TO99999999(wholeNumber));
            }

            if (wholeNumber >= 100000000 && wholeNumber <= 999999999)
            {
                numberInWords.Append(Handle100000000TO999999999(wholeNumber));
            }

            if (wholeNumber >= 1000000000 && wholeNumber <= 999999999999)
            {
                numberInWords.Append(HandleBillions(wholeNumber));
            }

            if (wholeNumber >= 1000000000000 && wholeNumber <= 999999999999999)
            {
                numberInWords.Append(HandleTrillions(wholeNumber));
            }

            if (fractionalPart != null)
            {
                numberInWords.Append(" point");
                foreach (var num in fractionalPart.ToCharArray())
                {
                    numberInWords.Append($" {Handle0TO99(double.Parse(num.ToString())).ToLower()}");
                }

            }

            if (isNegative)
            {
                return $"Negative {numberInWords.ToString().ToLower()}";
            }

            return numberInWords.ToString();
        }



        #region Conversion helper methods

        private string Handle0TO99(double numberToConvert)
        {
            int number = (int)numberToConvert;

            if (number < 20)
                return $"{_0to19[number]}";

            if (number % 10 == 0)
                return _endingWithty[number / 10];
            else
                return $"{_endingWithty[number / 10]} {_0to19[number % 10].ToLower()}";
        }

        private string Handle100TO999(double numberToConvert)
        {
            int number = (int)numberToConvert;

            if (number < 100)
                return Handle0TO99(number);

            if (number % 100 == 0)
                return $"{_0to19[number / 100]} {_0s[2]}";
            else
                return $"{_0to19[number / 100]} {_0s[2]} and {Handle0TO99(double.Parse(number.ToString().Substring(1, 2))).ToLower()}";
        }

        private string Handle1000TO9999(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);

            long dividedNumber = (long)(number / 1000);

            if (number % 1000 == 0)
                return $"{_0to19[dividedNumber]} {_0s[3]}";
            else
            {
                long splitNumber = long.Parse(number.ToString().Substring(1, 3));

                if (splitNumber < 20)
                    return $"{_0to19[dividedNumber]} {_0s[3]} and {Handle0TO99(splitNumber).ToLower()}";
                else
                {
                    if (splitNumber % 100 == 0)
                        return $"{_0to19[dividedNumber]} {_0s[3]} {Handle100TO999(splitNumber).ToLower()}";
                    else
                    {
                        if (splitNumber < 100)
                            return $"{_0to19[dividedNumber]} {_0s[3]} and {Handle100TO999(splitNumber).ToLower()}";
                        else
                            return $"{_0to19[dividedNumber]} {_0s[3]}, {Handle100TO999(splitNumber).ToLower()}";
                    }
                }
            }
        }

        private string Handle10000TO99999(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);
            if (number < 10000)
                return Handle1000TO9999(number);

            long dividedNumber = (long)(number / 1000);

            if (number % 10000 == 0)
                return $"{_endingWithty[dividedNumber / 10]} {_0s[3]}";
            if (number % 1000 == 0)
                return $"{Handle0TO99(number / 1000)} {_0s[3]}";
            else
            {
                long splitNumber = long.Parse(number.ToString().Substring(2, 3));

                string str = string.Empty;
                if ((number / 1000) < 20)
                    str = _0to19[dividedNumber];
                else
                    str = Handle0TO99(number / 1000);

                if (splitNumber < 20)
                    return $"{str} {_0s[3]} and {Handle0TO99(splitNumber).ToLower()}";
                if (splitNumber < 100)
                    return $"{str} {_0s[3]} and {Handle100TO999(splitNumber).ToLower()}";
                if (splitNumber < 1000)
                {
                    if (splitNumber % 100 == 0)
                        return $"{str} {_0s[3]} and {Handle100TO999(splitNumber).ToLower()}";
                    else
                        return $"{str} {_0s[3]}, {Handle100TO999(splitNumber).ToLower()}";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string Handle100000TO999999(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);
            if (number < 10000)
                return Handle1000TO9999(number);
            if (number < 100000)
                return Handle10000TO99999(number);

            long dividedNumber = (long)(number / 1000);

            if (number % 100000 == 0)
                return $"{_0to19[dividedNumber / 100]} {_0s[2]} {_0s[3]}";
            if (number % 1000 == 0)
                return $"{Handle100TO999(long.Parse(number.ToString().Substring(0, 3)))} {_0s[3]}";

            long splitNumber = long.Parse(number.ToString().Substring(3, 3));
            if (splitNumber < 100 || (splitNumber % 100 == 0))
                return $"{Handle100TO999(long.Parse(number.ToString().Substring(0, 3)))} {_0s[3]} and {Handle100TO999(splitNumber).ToLower()}";
            else
                return $"{Handle100TO999(long.Parse(number.ToString().Substring(0, 3)))} {_0s[3]}, {Handle100TO999(splitNumber).ToLower()}";
        }

        private string Handle1000000TO9999999(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);
            if (number < 10000)
                return Handle1000TO9999(number);
            if (number < 100000)
                return Handle10000TO99999(number);
            if (number < 1000000)
                return Handle100000TO999999(number);

            long dividedNumber = (long)(number / 1000);

            if (number % 1000000 == 0)
                return $"{_0to19[dividedNumber / 1000]} {_0s[6]}";

            long splitNumber = int.Parse(number.ToString().Substring(1));

            if (splitNumber < 100)
                return $"{_0to19[dividedNumber / 1000]} {_0s[6]} and {Handle0TO99(splitNumber).ToLower()}";
            if (splitNumber < 1000)
            {
                if (splitNumber % 100 == 0)
                    return $"{_0to19[dividedNumber / 1000]} {_0s[6]} and {Handle100TO999(splitNumber).ToLower()}";
                else
                    return $"{_0to19[dividedNumber / 1000]} {_0s[6]}, {Handle100TO999(splitNumber).ToLower()}";
            }

            if (splitNumber < 10000)
            {
                if (splitNumber % 1000 == 0)
                    return $"{_0to19[dividedNumber / 1000]} {_0s[6]} and {Handle1000TO9999(splitNumber).ToLower()}";
                else
                    return $"{_0to19[dividedNumber / 1000]} {_0s[6]}, {Handle1000TO9999(splitNumber).ToLower()}";
            }

            if (splitNumber < 100000)
            {
                if (splitNumber % 10000 == 0)
                    return $"{_0to19[dividedNumber / 1000]} {_0s[6]} and {Handle10000TO99999(splitNumber).ToLower()}";
                else
                    return $"{_0to19[dividedNumber / 1000]} {_0s[6]}, {Handle10000TO99999(splitNumber).ToLower()}";
            }

            return $"{_0to19[dividedNumber / 1000]} {_0s[6]}, {Handle100000TO999999(splitNumber).ToLower()}";
        }

        private string Handle10000000TO99999999(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);
            if (number < 10000)
                return Handle1000TO9999(number);
            if (number < 100000)
                return Handle10000TO99999(number);
            if (number < 1000000)
                return Handle100000TO999999(number);
            if (number < 10000000)
                return Handle1000000TO9999999(number);

            long splitNumber = int.Parse(number.ToString().Substring(0, 2));
            long dividedNumber = (long)(number / 1000);

            string str = string.Empty;
            if (splitNumber < 20)
                str = $"{_0to19[dividedNumber / 1000]} {_0s[6]}";
            else
                str = $"{Handle0TO99(splitNumber)} {_0s[6]}";

            if (number % 1000000 == 0)
            {
                return str;
            }
            else
            {
                splitNumber = int.Parse(number.ToString().Substring(2));

                if (splitNumber < 100)
                    return $"{str} and {Handle0TO99(splitNumber).ToLower()}";
                if (splitNumber < 1000)
                {
                    if (splitNumber % 100 == 0)
                        return $"{str} and {Handle100TO999(splitNumber).ToLower()}";
                    else
                        return $"{str}, {Handle100TO999(splitNumber).ToLower()}";
                }

                if (splitNumber < 10000)
                {
                    if (splitNumber % 1000 == 0)
                        return $"{str} and {Handle1000TO9999(splitNumber).ToLower()}";
                    else
                        return $"{str}, {Handle1000TO9999(splitNumber).ToLower()}";
                }

                if (splitNumber < 100000)
                {
                    if (splitNumber % 10000 == 0)
                        return $"{str} and {Handle10000TO99999(splitNumber).ToLower()}";
                    else
                    {
                        if (splitNumber % 1000 == 0)
                            return $"{str} and {Handle10000TO99999(splitNumber).ToLower()}";
                        else
                            return $"{str}, {Handle10000TO99999(splitNumber).ToLower()}";
                    }
                }

                return $"{str}, {Handle100000TO999999(splitNumber).ToLower()}";
            }
        }

        private string Handle100000000TO999999999(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);
            if (number < 10000)
                return Handle1000TO9999(number);
            if (number < 100000)
                return Handle10000TO99999(number);
            if (number < 1000000)
                return Handle100000TO999999(number);
            if (number < 10000000)
                return Handle1000000TO9999999(number);
            if (number < 100000000)
                return Handle10000000TO99999999(number);

            long splitNumber = int.Parse(number.ToString().Substring(0, 3));

            string str = $"{Handle100TO999(splitNumber)} {_0s[6]}";

            if (number % 1000000 == 0)
            {
                return str;
            }
            else
            {
                splitNumber = int.Parse(number.ToString().Substring(3));

                if (splitNumber < 100)
                    return $"{str} and {Handle100000TO999999(splitNumber).ToLower()}";
                else
                {
                    if (splitNumber % 1000 == 0 && splitNumber < 100000)
                        return $"{str} and {Handle100000TO999999(splitNumber).ToLower()}";
                    else
                        return $"{str}, {Handle100000TO999999(splitNumber).ToLower()}";
                }
            }
        }

        private string HandleBillions(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);
            if (number < 10000)
                return Handle1000TO9999(number);
            if (number < 100000)
                return Handle10000TO99999(number);
            if (number < 1000000)
                return Handle100000TO999999(number);
            if (number < 10000000)
                return Handle1000000TO9999999(number);
            if (number < 100000000)
                return Handle10000000TO99999999(number);
            if (number < 1000000000)
                return Handle100000000TO999999999(number);

            long splitNumber = 0;
            string str = string.Empty;
            if (number.ToString().Length == 10)
            {
                splitNumber = long.Parse(number.ToString().Substring(0, 1));
                str = Handle0TO99(splitNumber);
                splitNumber = long.Parse(number.ToString().Substring(1));
            }

            if (number.ToString().Length == 11)
            {
                splitNumber = long.Parse(number.ToString().Substring(0, 2));
                str = Handle0TO99(splitNumber);
                splitNumber = long.Parse(number.ToString().Substring(2));
            }

            if (number.ToString().Length == 12)
            {
                splitNumber = long.Parse(number.ToString().Substring(0, 3));
                str = Handle100TO999(splitNumber);
                splitNumber = long.Parse(number.ToString().Substring(3));
            }

            if (number % 1000000000 == 0)
                return $"{str} {_0s[9]}";
            else
            {
                if (splitNumber < 100)
                    return $"{str} {_0s[9]} and {Handle100000000TO999999999(splitNumber).ToLower()}";
                else
                    return $"{str} {_0s[9]}, {Handle100000000TO999999999(splitNumber).ToLower()}";
            }
        }

        private string HandleTrillions(double number)
        {
            if (number < 100)
                return Handle0TO99(number);
            if (number < 1000)
                return Handle100TO999(number);
            if (number < 10000)
                return Handle1000TO9999(number);
            if (number < 100000)
                return Handle10000TO99999(number);
            if (number < 1000000)
                return Handle100000TO999999(number);
            if (number < 10000000)
                return Handle1000000TO9999999(number);
            if (number < 100000000)
                return Handle10000000TO99999999(number);
            if (number < 1000000000)
                return Handle100000000TO999999999(number);
            if (number < 1000000000000)
                return HandleBillions(number);

            long splitNumber = 0;
            string str = string.Empty;
            if (number.ToString().Length == 13)
            {
                splitNumber = long.Parse(number.ToString().Substring(0, 1));
                str = Handle0TO99(splitNumber);
                splitNumber = long.Parse(number.ToString().Substring(1));
            }

            if (number.ToString().Length == 14)
            {
                splitNumber = long.Parse(number.ToString().Substring(0, 2));
                str = Handle0TO99(splitNumber);
                splitNumber = long.Parse(number.ToString().Substring(2));
            }

            if (number.ToString().Length == 15)
            {
                splitNumber = long.Parse(number.ToString().Substring(0, 3));
                str = Handle100TO999(splitNumber);
                splitNumber = long.Parse(number.ToString().Substring(3));
            }

            if (number % 1000000000000 == 0)
                return $"{str} {_0s[12]}";
            else
            {
                if (splitNumber < 100)
                    return $"{str} {_0s[12]} and {HandleBillions(splitNumber).ToLower()}";
                else
                    return $"{str} {_0s[12]}, {HandleBillions(splitNumber).ToLower()}";
            }
        }

        #endregion Conversion

    }
}
