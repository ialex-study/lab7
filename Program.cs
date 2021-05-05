using System;
using System.Text.RegularExpressions;

namespace lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            RationalNumber num1, num2;
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите число №1: ");
                    num1 = RationalNumber.ReadFromString(Console.ReadLine());

                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Неверные числа");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Неверный формат ввода");
                }
            }
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите число №2: ");
                    num2 = RationalNumber.ReadFromString(Console.ReadLine());

                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Неверные числа");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Неверный формат ввода");
                }
            }
            int choise = -1;
            Console.WriteLine("Выберите действие");
            while(choise != 0)
            {
                Console.WriteLine(
                    "0. Выйти\n" +
                    "1. Сравнить числа\n" +
                    "2. Сложить\n" +
                    "3. Вычесть\n" +
                    "4. Перемножить\n" +
                    "5. Разделить\n" +
                    "6. Преобразовать к целому\n" +
                    "7. Преобразовать к вещественному"
                    );
                try
                {
                    choise = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    choise = -1;
                }
                if (choise == 0)
                    break;
                else if(choise == 1)
                {
                    if (num1 > num2)
                        Console.WriteLine("{0} > {1}", num1, num2);
                    else if(num1 < num2)
                        Console.WriteLine("{0} < {1}", num1, num2);
                    else
                        Console.WriteLine("{0} = {1}", num1, num2);
                }
                else if(choise == 2)
                {
                    Console.WriteLine("{0} + {1} = {2}", num1, num2, num1 + num2);
                }
                else if(choise == 3)
                {
                    Console.WriteLine("{0} - {1} = {2}", num1, num2, num1 - num2);
                }
                else if(choise == 4)
                {
                    Console.WriteLine("{0} * {1} = {2}", num1, num2, num1 * num2);
                }
                else if(choise == 5)
                {
                    try
                    {
                        Console.WriteLine("{0} / {1} = {2}", num1, num2, num1 / num2);
                    }
                    catch(DivideByZeroException)
                    {
                        Console.WriteLine("Деление на ноль");
                    }
                }
                else if(choise == 6)
                {
                    Console.WriteLine("(int){0} : {1}", num1, (int)num1);
                    Console.WriteLine("(int){0} : {1}", num2, (int)num2);
                }
                else if(choise == 7)
                {
                    Console.WriteLine("(int){0} : {1}", num1, (float)num1);
                    Console.WriteLine("(int){0} : {1}", num2, (float)num2);
                }
                else
                {
                    Console.WriteLine("Неверный ввод");
                }
                
            }
        }
    }
    class RationalNumber
    {
        int n;
        int m;

        public enum TypeOfStringFormat
        {
            RightType,
            NotRightType
        };
        TypeOfStringFormat TypeOfString { get; set; }

        int Numerator
        {
            get => n;
            set => n = value;
        }
        int Denominator
        {
            get => m;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                m = value;
            }
        }
        private int GetGCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            int q = a / b;
            int r = a % b;

            while(r > 0)
            {
                a = b;
                b = r;
                r = a % b;
            }
            return b;
        }
        private void Format()
        {
            int gcd = GetGCD(n, m);
            while (gcd != 1)
            {
                n /= gcd;
                m /= gcd;
                gcd = GetGCD(n, m);
            }
        }
        public RationalNumber(int n, int m, TypeOfStringFormat typeOfString = TypeOfStringFormat.RightType)
        {
            Numerator = n;
            Denominator = m;
            TypeOfString = typeOfString;
            Format();
        }
        public static RationalNumber ReadFromString(string input)
        {
            Match match = new Regex(@"-?\d+").Match(input);
            int n, m, k = 0;
            TypeOfStringFormat type = TypeOfStringFormat.RightType;
            if (!match.Success)
                throw new ArgumentException();
            n = int.Parse(match.Value);
            match = match.NextMatch();

            if (!match.Success)
                throw new ArgumentException();
            m = int.Parse(match.Value);
            match = match.NextMatch();

            if (match.Success)
                k = int.Parse(match.Value);

            match = new Regex(@"Not\s*right", RegexOptions.IgnoreCase).Match(input);
            if (match.Success)
                type = TypeOfStringFormat.NotRightType;

            if (k != 0)
            {
                int one = n < 0 ? -1 : 1;
                if (m < 0)
                    one *= -1;
                return new RationalNumber(one * (Math.Abs(n) * k + Math.Abs(m)), k, type);
            }
            else
                return new RationalNumber(n, m, type);
        }
        public static RationalNumber operator +(RationalNumber num1, RationalNumber num2)
        {
            int n = num1.Numerator * num2.Denominator + num1.Denominator * num2.Numerator;
            int m = num1.Denominator * num2.Denominator;
            return new RationalNumber(n, m, num1.TypeOfString);
        }
        public static RationalNumber operator -(RationalNumber num1, RationalNumber num2)
        {
            int n = num1.Numerator * num2.Denominator - num1.Denominator * num2.Numerator;
            int m = num1.Denominator * num2.Denominator;
            return new RationalNumber(n, m, num1.TypeOfString);
        }
        public static RationalNumber operator *(RationalNumber num1, RationalNumber num2)
        {
            int n = num1.Numerator * num2.Numerator;
            int m = num1.Denominator * num2.Denominator;
            return new RationalNumber(n, m, num1.TypeOfString);
        }
        public static RationalNumber operator /(RationalNumber num1, RationalNumber num2)
        {
            if (num2.Numerator == 0)
                throw new DivideByZeroException();
            if(num2.Numerator < 0)
            {
                num2.Numerator *= -1;
                num2.Denominator *= -1;
            }
            int n = num1.Numerator * num2.Denominator;
            int m = num1.Denominator * num2.Numerator;
            return new RationalNumber(n, m, num1.TypeOfString);
        }
        public static bool operator ==(RationalNumber num1, RationalNumber num2)
        {
            return ((num1.Numerator == num2.Numerator) && (num1.Denominator == num2.Denominator));
        }
        public static bool operator !=(RationalNumber num1, RationalNumber num2)
        {
            return ((num1.Numerator != num2.Numerator) || (num1.Denominator != num2.Denominator));
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            RationalNumber num = (RationalNumber) obj;
            return ((Numerator == num.Numerator) && (Denominator == num.Denominator));
        }
        public override int GetHashCode()
        {
            return n ^ m;
        }
        public static bool operator >(RationalNumber num1, RationalNumber num2)
        {
            int n1 = num1.Numerator * num2.Denominator;
            int n2 = num2.Numerator * num1.Denominator;
            return (n1 > n2);
        }
        public static bool operator <(RationalNumber num1, RationalNumber num2)
        {
            int n1 = num1.Numerator * num2.Denominator;
            int n2 = num2.Numerator * num1.Denominator;
            return (n1 < n2);
        }
        public static bool operator <=(RationalNumber num1, RationalNumber num2)
        {
            return ((num1 < num2) || (num1 == num2));
        }
        public static bool operator >=(RationalNumber num1, RationalNumber num2)
        {
            return ((num1 > num2) || (num1 == num2));
        }
        public static implicit operator int(RationalNumber num)
        {
            return num.Numerator / num.Denominator;
        }
        public static implicit operator float (RationalNumber num)
        {
            return (float)num.Numerator / (float)num.Denominator;
        }
        public override string ToString()
        {
            if (TypeOfString == TypeOfStringFormat.NotRightType)
            {
                return String.Format("{0}/{1}", n, m);
            }
            else
            {
                if(n % m != 0)
                    return String.Format("{0} and {1}/{2}", (int)(n / m), Math.Abs(n) % m, m);
                else
                    return String.Format("{0}", (int)(n / m));
            }
        }
    }
}
