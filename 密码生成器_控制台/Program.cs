namespace 密码生成器_控制台;

internal class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            var chooseHash = new HashSet<string?> { "1", "2", "3", "4" };
            string? choose;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("选择密码类型：");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("1. 字母\n2. 字母 + 数字\n3. 字母 + 符号\n4. 字母 + 数字 + 符号\n");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("输入序号并回车：");
                Console.ForegroundColor = ConsoleColor.Gray;
                choose = Console.ReadLine();
                if (!string.IsNullOrEmpty(choose) && chooseHash.Contains(choose)) continue;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("选择错误，请重选！\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            } while (string.IsNullOrEmpty(choose) || !chooseHash.Contains(choose));

            var number = int.Parse(choose);

            PasswordGenerator.PasswordType = number switch
            {
                1 => PasswordGenerator.PasswordTypeEnum.Letter,
                2 => PasswordGenerator.PasswordTypeEnum.LetterAndDigit,
                3 => PasswordGenerator.PasswordTypeEnum.LetterAndSymbol,
                4 => PasswordGenerator.PasswordTypeEnum.All,
                _ => throw new Exception()
            };

            int length;
            var lengthOk = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("输入密码长度：");
                Console.ForegroundColor = ConsoleColor.Gray;
                lengthOk = int.TryParse(Console.ReadLine(), out length);
                if (!lengthOk)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("输入错误，请重新输入！\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (length < 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("密码长度至少为6，请重新输入！\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            } while (!lengthOk || length < 6);

            var password = PasswordGenerator.GetPassword(length);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("已成功生成密码：");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var item in password)
            {
                if (PasswordGenerator.CharsLetter.Contains(item))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (PasswordGenerator.CharsDigit.Contains(item))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.Write(item);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n\n-----------------------------\n");
        }
    }
}