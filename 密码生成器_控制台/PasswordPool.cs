using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 密码生成器_控制台
{
    internal static class PasswordGenerator
    {
        public static readonly char[] CharsSymbol = new char[32];
        public static readonly char[] CharsDigit = new char[10];
        public static readonly char[] CharsLetter = new char[52];

        public static bool LetterChecked { get; set; } = true;
        public static bool DigitChecked { get; set; } = true;
        public static bool SymbolChecked { get; set; } = true;

        [Flags]
        public enum PasswordTypeEnum
        {
            Letter = 1,
            Digit = 2,
            Symbol = 4,
            LetterAndDigit = Letter | Digit,
            LetterAndSymbol = Letter | Symbol,
            All = Letter | Digit | Symbol
        }

        public static PasswordTypeEnum PasswordType { get; set; } = PasswordTypeEnum.All;

        private static void InitChars()
        {
            for (var i = 0; i < CharsDigit.Length; i++)
            {
                CharsDigit[i] = (char)(i + 48);
            }

            for (var i = 0; i < CharsLetter.Length; i++)
            {
                CharsLetter[i] = (char)(i + (i <= 25 ? 65 : 71));
            }

            for (var i = 0; i < CharsSymbol.Length; i++)
            {
                CharsSymbol[i] = (char)(i + i switch
                {
                    < 15 => 33,
                    < 22 => 43,
                    < 28 => 69,
                    _ => 95
                });
            }
        }

        private static char[] GetPasswordCharPool()
        {
            InitChars();
            char[] pool;
            switch (PasswordType)
            {
                case PasswordTypeEnum.Letter:
                    pool = new char[CharsLetter.Length];
                    for (var i = 0; i < pool.Length; i++)
                    {
                        pool[i] = CharsLetter[i];
                    }

                    break;
                case PasswordTypeEnum.LetterAndDigit:
                    pool = new char[CharsLetter.Length + CharsDigit.Length];
                    for (var i = 0; i < pool.Length; i++)
                    {
                        pool[i] = i < CharsLetter.Length ? CharsLetter[i] : CharsDigit[i - CharsLetter.Length];
                    }

                    break;
                case PasswordTypeEnum.LetterAndSymbol:
                    pool = new char[CharsLetter.Length + CharsSymbol.Length];
                    for (var i = 0; i < pool.Length; i++)
                    {
                        pool[i] = i < CharsLetter.Length ? CharsLetter[i] : CharsSymbol[i - CharsLetter.Length];
                    }

                    break;
                case PasswordTypeEnum.All:
                case PasswordTypeEnum.Digit:
                case PasswordTypeEnum.Symbol:
                default:
                    pool = new char[CharsLetter.Length + CharsDigit.Length + CharsSymbol.Length];
                    for (var i = 0; i < pool.Length; i++)
                    {
                        if (i < CharsLetter.Length)
                        {
                            pool[i] = CharsLetter[i];
                        }
                        else if (i - CharsLetter.Length < CharsDigit.Length)
                        {
                            pool[i] = CharsDigit[i - CharsLetter.Length];
                        }
                        else
                        {
                            pool[i] = CharsSymbol[i - CharsLetter.Length - CharsDigit.Length];
                        }
                    }

                    break;
            }

            return pool;
        }

        public static char[] GetPassword(int length)
        {
            var r = new Random(Guid.NewGuid().GetHashCode());
            var passwordCharPool = GetPasswordCharPool();
            var password = new char[length];

            var letterFixedIndex = r.Next(0, length);
            int digitFixedIndex = -1;
            int symbolFixedIndex = -1;

            switch (PasswordType)
            {
                case PasswordTypeEnum.LetterAndDigit:
                {
                    do
                    {
                        digitFixedIndex = r.Next(0, length);
                    } while (digitFixedIndex == letterFixedIndex);
                    
                    break;
                }
                case PasswordTypeEnum.LetterAndSymbol:
                {
                    do
                    {
                        symbolFixedIndex = r.Next(0, length);
                    } while (symbolFixedIndex == letterFixedIndex);
                    
                    break;
                }
                case PasswordTypeEnum.Letter:
                case PasswordTypeEnum.Digit:
                case PasswordTypeEnum.Symbol:
                case PasswordTypeEnum.All:
                default:
                {
                    do
                    {
                        digitFixedIndex = r.Next(0, length);
                    } while (digitFixedIndex == letterFixedIndex);

                    do
                    {
                        symbolFixedIndex = r.Next(0, length);
                    } while (symbolFixedIndex == digitFixedIndex || symbolFixedIndex == letterFixedIndex);
                    
                    break;
                }
            }

            for (var i = 0; i < length; i++)
            {
                switch (PasswordType)
                {
                    case PasswordTypeEnum.Letter:
                        password[i] = CharsLetter[r.Next(0, CharsLetter.Length)];
                        break;
                    case PasswordTypeEnum.LetterAndDigit:
                    {
                        if (i == letterFixedIndex)
                        {
                            password[i] = CharsLetter[r.Next(0, CharsLetter.Length)];
                        }
                        else if (i == digitFixedIndex)
                        {
                            password[i] = CharsDigit[r.Next(0, CharsDigit.Length)];
                        }
                        else
                        {
                            password[i] = passwordCharPool[r.Next(0, passwordCharPool.Length)];
                        }
                        
                        break;
                    }
                    case PasswordTypeEnum.LetterAndSymbol:
                    {
                        if (i == letterFixedIndex)
                        {
                            password[i] = CharsLetter[r.Next(0, CharsLetter.Length)];
                        }
                        else if (i == symbolFixedIndex)
                        {
                            password[i] = CharsSymbol[r.Next(0, CharsSymbol.Length)];
                        }
                        else
                        {
                            password[i] = passwordCharPool[r.Next(0, passwordCharPool.Length)];
                        }

                        break;
                    }
                    case PasswordTypeEnum.Digit:
                    case PasswordTypeEnum.Symbol:
                    case PasswordTypeEnum.All:
                    default:
                    {
                        if (i == letterFixedIndex)
                        {
                            password[i] = CharsLetter[r.Next(0, CharsLetter.Length)];
                        }
                        else if (i == digitFixedIndex)
                        {
                            password[i] = CharsDigit[r.Next(0, CharsDigit.Length)];
                        }
                        else if (i == symbolFixedIndex)
                        {
                            password[i] = CharsSymbol[r.Next(0, CharsSymbol.Length)];
                        }
                        else
                        {
                            password[i] = passwordCharPool[r.Next(0, passwordCharPool.Length)];
                        }

                        break;
                    }
                }
            }

            return password;
        }
    }
}