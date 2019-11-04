using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение:");
            string inputString = Console.ReadLine();
            Console.WriteLine(" ");
            inputString = inputString.Replace(" ", "");
            inputString = inputString.Replace(".", ",");
            inputString = inputString.Replace(":", "/");

            bool wrongString = WrongNumberFormat(inputString) || WrongOperation(inputString) || WrongFormat(inputString) || WrongGrouping(inputString);

            int count = inputString.Count(f => f == '(');
            if (!wrongString)
            {
                Console.WriteLine("Решение:");               
                Console.WriteLine(inputString);
                string endString = inputString;
                for (int i = 0; i <= count; i++)
                {
                    int indexOfBegin;
                    int indexOfEnd;
                    string Substring = "";
                    indexOfBegin = endString.LastIndexOf('(');
                    if (indexOfBegin == -1)
                    {
                        indexOfBegin = 0;
                        indexOfEnd = endString.Length - 1;
                        Substring = endString.Substring(indexOfBegin, indexOfEnd - indexOfBegin + 1);
                    }
                    else
                    {
                        indexOfEnd = indexOfBegin + endString.Remove(0, indexOfBegin).IndexOf(')');
                        Substring = endString.Substring(indexOfBegin, indexOfEnd - indexOfBegin + 1);
                    }
                    string resultOfSubstring = Substring;
                    resultOfSubstring = MathConversion(resultOfSubstring);
                    resultOfSubstring = ResultOfFirstOperation(resultOfSubstring, '^');
                    resultOfSubstring = ResultOfFirstOperation(resultOfSubstring, '*');
                    resultOfSubstring = ResultOfFirstOperation(resultOfSubstring, '/');
                    resultOfSubstring = ResultOfSecondOperation(resultOfSubstring);
                    resultOfSubstring = resultOfSubstring.Replace("(", "");
                    resultOfSubstring = resultOfSubstring.Replace(")", "");
                    resultOfSubstring = resultOfSubstring.Replace("(+", "(");
                    endString = (endString.Remove(indexOfBegin, indexOfEnd - indexOfBegin + 1)).Insert(indexOfBegin, resultOfSubstring);
                    Console.WriteLine(endString);
                }
                Console.WriteLine(" ");
                Console.WriteLine("Ответ: " + endString);
                Console.ReadKey();
            }
        }

        //Возвращает TRUE, когда в строке количество открывающихся и закрывающихся скобок не совпадает.
        //Иначе возвращает FALSE.
        static public bool WrongGrouping(string s)
        {
            if (s.Count(f => f == '(') != s.Count(f => f == ')'))
            {
                Console.WriteLine("В выражении количество закрывающихся и открывающихсяс скобок не совпадает!");
                return true;
            }
            else
            {
                return false;
            }           
        }

        //Возвращает TRUE, когда в строке встречается число с двумя или более запятыми.
        //Иначе возвращает FALSE.
        static public bool WrongNumberFormat(string s)
        {
            int countComma = 0;
            foreach (Char ch in s)
            {
                if (Char.IsDigit(ch))
                {
                    continue;
                }
                else if (ch == ',')
                {
                    countComma = countComma + 1;
                    if (countComma == 2)
                    {
                        Console.WriteLine("В дробном числе встречаюся две или более запятых!");
                        Console.ReadKey();              
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (!Char.IsDigit(ch) && ch != ',')
                {
                    countComma = 0;
                    continue;
                }
            }
            return false;
        }

        //Возвращает TRUE, когда в строке после символов +,-,*,/,^ не находится число или открывающаяся скобка.
        //Иначе возвращает FALSE.
        static public bool WrongOperation(string s)
        {
            if ((WrongOperationChar(s, '^')) || (WrongOperationChar(s, '*')) ||
               (WrongOperationChar(s, '/')) || (WrongOperationChar(s, '+')) || (WrongOperationChar(s, '-')))               
            {
                Console.WriteLine("Введена неверная арифметическая операция!");
                Console.ReadKey();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Возвращает TRUE, когда в строке после заданного символа не находится число или открывающаяся скобка.
        //Иначе возвращает FALSE.
        static public bool WrongOperationChar(string s, char ch)
        {
            int indexOfChar = s.IndexOf(ch);
            return !((indexOfChar == -1) || ((indexOfChar != -1) && (Char.IsDigit(s[indexOfChar + 1]) || s[indexOfChar + 1] == '(')));              
        }

        //Возвращает TRUE, когда в строке встречаются символы кроме +,-,*,/,:,),( а также запятой, точки и цифр.
        //Иначе возвращает FALSE.
        static public bool WrongFormat(string s)
        {
            string figString = s.Replace(")","").Replace("(","").Replace("+","").Replace("-","").Replace("*","").Replace("/","").Replace("^","").Replace(".","").Replace(",","");
            foreach (Char ch in figString)
            {
                if (Char.IsDigit(ch))
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Введено неверное выражение!\nОно должно содержать только цифры и следующие символы:\n+\n-\n*\n/\n^\n(\n)\n,");
                    Console.ReadKey();
                    return true;
                }
            }
            return false;
        }

        //Возвращает строку, 
        //Иначе возвращает FALSE.
        static public string MathConversion(string s)
        {
            s = s.Replace("+-", "-");
            s = s.Replace("-+", "-");
            s = s.Replace("--", "+");
            s = s.Replace("(+", "(");
            return s;
        }

        //Вовращает строку с выполненными ариметическими операцими: возведение в степень, умножение, деление
        static public string ResultOfFirstOperation(string s, char char1)
        {
            int count = s.Count(f => f == char1);
            string[] arrayOfNum;
            arrayOfNum = new string[4];
            if (count > 0)
            {
                string result = s;
                result = result.Replace("(", "").Replace(")", "");
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(result);

                    var arrayAddit = TwoNumber(result, char1, arrayOfNum);
                    double num1 = double.Parse(arrayAddit[0]);
                    double num2 = double.Parse(arrayAddit[1]);
                    int num1Index = Int32.Parse(arrayAddit[2]);
                    int num2Index = Int32.Parse(arrayAddit[3]);

                    double numResult;
                    if (char1 == '^')
                    {
                        numResult = Math.Pow(num1, num2);
                        result = result.Remove(num1Index, num2Index - num1Index + 1);
                        result = result.Insert(num1Index, numResult.ToString());
                    }
                    else if (char1 == '*')
                    {
                        numResult = num1 * num2;
                        result = result.Remove(num1Index, num2Index - num1Index + 1);
                        result = result.Insert(num1Index, numResult.ToString());
                    }
                    else if (char1 == '/')
                    {
                        numResult = num1 / num2;
                        result = result.Remove(num1Index, num2Index - num1Index + 1);
                        result = result.Insert(num1Index, numResult.ToString());
                    }
                    result = MathConversion(result);
                }
                return (result);
            }
            else
            {
                return (s);
            }
        }

        //Вовращает строку с выполненными ариметическими операцими: сложение, вычитание
        static public string ResultOfSecondOperation(string s)
        {
            int countPlus = s.Count(f => f == '+');
            int countMinus = s.Count(f => f == '-');
            int countOperation = countMinus + countPlus;
            string[] arrayOfNum;
            arrayOfNum = new string[4];
            if (countOperation > 0)
            {
                string result = s;
                result = result.Replace("(", "").Replace(")", "");
                for (int i = 0; i < countOperation; i++)
                {
                    Console.WriteLine(result);
                    if ((result.Count(f => f == '-') == 1) && (result[0] == '-') && (result.Count(f => f == '+') == 0))
                    {
                        break;
                    }

                    if (result[0] != '-')
                    {
                        if ((result.Count(f => f == '+') != 0 && result.Count(f => f == '-') != 0 && result.IndexOf('+') < result.IndexOf('-')) || result.Count(f => f == '-') == 0)
                        {
                            var arrayAddit = TwoNumber(result, '+', arrayOfNum);
                            double num1 = double.Parse(arrayAddit[0]);
                            double num2 = double.Parse(arrayAddit[1]);
                            int num1Index = Int32.Parse(arrayAddit[2]);
                            int num2Index = Int32.Parse(arrayAddit[3]);

                            double numResult;

                            numResult = num1 + num2;
                            result = result.Remove(num1Index, num2Index - num1Index + 1);
                            result = result.Insert(num1Index, numResult.ToString());
                        }
                        else if ((result.Count(f => f == '+') != 0 && result.Count(f => f == '-') != 0 && result.IndexOf('-') < result.IndexOf('+')) || result.Count(f => f == '+') == 0)
                        {
                            var arrayAddit = TwoNumber(result, '-', arrayOfNum);
                            double num1 = double.Parse(arrayAddit[0]);
                            double num2 = double.Parse(arrayAddit[1]);
                            int num1Index = Int32.Parse(arrayAddit[2]);
                            int num2Index = Int32.Parse(arrayAddit[3]);

                            double numResult;

                            numResult = num1 - num2;
                            result = result.Remove(num1Index, num2Index - num1Index + 1);
                            result = result.Insert(num1Index, numResult.ToString());
                        }
                    }
                    else
                    {
                        if ((result.Count(f => f == '+') != 0 && result.Count(f => f == '-') > 1 && result.IndexOf('+') < result.IndexOf('-',1)) || result.Count(f => f == '-') == 1)
                        {
                            var arrayAddit = TwoNumber(result, '+', arrayOfNum);
                            double num1 = double.Parse(arrayAddit[0]);
                            double num2 = double.Parse(arrayAddit[1]);
                            int num1Index = Int32.Parse(arrayAddit[2]);
                            int num2Index = Int32.Parse(arrayAddit[3]);

                            double numResult;

                            numResult = num1 + num2;
                            result = result.Remove(num1Index, num2Index - num1Index + 1);
                            result = result.Insert(num1Index, numResult.ToString());
                        }
                        else if ((result.Count(f => f == '+') != 0 && result.Count(f => f == '-') > 0 && result.IndexOf('-',1) < result.IndexOf('+')) || result.Count(f => f == '+') == 0)
                        {
                            var arrayAddit = TwoNumber(result, '-', arrayOfNum);
                            double num1 = double.Parse(arrayAddit[0]);
                            double num2 = double.Parse(arrayAddit[1]);
                            int num1Index = Int32.Parse(arrayAddit[2]);
                            int num2Index = Int32.Parse(arrayAddit[3]);

                            double numResult;

                            numResult = num1 - num2;
                            result = result.Remove(num1Index, num2Index - num1Index + 1);
                            result = result.Insert(num1Index, numResult.ToString());
                        }
                    }
                    result = MathConversion(result);                  
                }
                return (result);
            }
            else
            {
                return (s);
            }
        }

        //Возвращает массив, где 
        //0-ой элемент - число, которое находится до заданного символа.
        //1-ый элемент - число, которое находится после заданного символа.
        //2-ой элемент - индекс первой цифры числа, которое находится до заданного символа.
        //3-ий элемент - индекс последней цифры числа, которое находится до заданного символа.
        static public string[] TwoNumber(string s, char ch, string[] arrayOfNumbers) 
        {
            int indexOfChar = s.IndexOf(ch);
            if (indexOfChar == 0 && ch == '-' && s.IndexOf(ch, 1) != -1)
            {
                indexOfChar = s.IndexOf(ch, 1);
            }
            string num1String = "";
            string num2String = "";

            string stringBeforeChar = s.Substring(0, indexOfChar);
            for (int x1 = stringBeforeChar.Length - 1; x1 >= 0; x1--)
            {
                if ((Char.IsDigit(stringBeforeChar[x1]) || stringBeforeChar[x1] == ',') && x1 != 0)
                {
                    num1String = stringBeforeChar[x1] + num1String;

                    arrayOfNumbers[0] = num1String;
                    arrayOfNumbers[2] = x1.ToString();
                }
                else if ((Char.IsDigit(stringBeforeChar[x1]) || stringBeforeChar[x1] == ',' || stringBeforeChar[x1] == '-') && x1 == 0)
                {
                    num1String = stringBeforeChar[x1] + num1String;

                    arrayOfNumbers[0] = num1String;
                    arrayOfNumbers[2] = x1.ToString();
                    break;
                }
                else
                {
                    break;
                }
            }

            string stringAfterChar = s.Substring(indexOfChar + 1);
            for (int x2 = 0; x2 <= stringAfterChar.Length - 1; x2++)
            {
                if ((Char.IsDigit(stringAfterChar[x2]) || stringAfterChar[x2] == ',' || (stringAfterChar[x2] == '-' && x2 == 0)) && x2 != (stringAfterChar.Length - 1))
                {
                    num2String = num2String + stringAfterChar[x2];

                    arrayOfNumbers[1] = num2String;
                    arrayOfNumbers[3] = (indexOfChar + x2 + 1).ToString();
                }
                else if ((Char.IsDigit(stringAfterChar[x2]) || stringAfterChar[x2] == ',' || (stringAfterChar[x2] == '-' && x2 == 0)) && x2 == (stringAfterChar.Length - 1))
                {
                    num2String = num2String + stringAfterChar[x2];

                    arrayOfNumbers[1] = num2String;
                    arrayOfNumbers[3] = (indexOfChar + x2 + 1).ToString();
                    break;
                }
                else
                {
                    break;
                }
            }
            return arrayOfNumbers;
        }
    }
}
