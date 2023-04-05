using System;
using System.Collections.Generic;

namespace Bulls_and_Cows
{
    class Program
    {
        static void Main(string[] args)
        {
            bool adminMode = false; // Переменная, сообщающая о текущем режиме игры. (По умолчанию - "игрок").
            do
            {
                Console.Clear(); // Очищает консоль.
                Console.WriteLine("Добро пожаловать в игру \"Быки и коровы\".");
                Menu(ref adminMode); // Открывает меню.
                Console.WriteLine("\nДля выхода из игры нажмите ESC, для возвращения в меню - любую другую клавишу.\n");
            } while (Console.ReadKey().Key != ConsoleKey.Escape); // Реализует повторное решение.
        }
        
        // Правила игры.
        private static string rules = "Правила игры:\n" + 
                                      "1. Игрок вводит число от 1 до 10 (длину загадываемого числа).\n"+
                                      "2. Генерируется число с таким количеством цифр (причем все цифры различны).\n" +
                                      "3. Игрок, пытаясь отгадать это число, вводит своё число той же длины.\n" +
                                      "4. Компьютер сообщает количество быков и коров в введённом числе.\n" +
                                      "   Быки - угаданные цифры, которые стоят на своих местах.\n" +
                                      "   Коровы - угаданные цифры, которые стоят не на своих местах.\n" +
                                      "5. Всё повторяется с пункта (3), пока игрок не отгадает число.";
        // Список читов.
        private static string cheats = "\nСписок читов:\n" +
                                        "#answer# - выводит загаданное число на экран.\n" +
                                        "#end# - преждевременно завершает игру.";

        /// <summary>
        /// Метод реализует "пользовательский интерфейс".
        /// </summary>
        /// <param name="adminMode"> Режим игры. </param>
        public static void Menu(ref bool adminMode)
        {
            Console.WriteLine("\nЧтобы продолжить, нажмите соответсвующую клавишу:");
            Console.WriteLine("1. Начать игру.\n2. Прочитать правила.\n3. Выбрать режим игры.\n"+
                                "4. Список чит-кодов.\n5. Выйти из игры."); // Список опций.
            switch (Console.ReadKey().Key) // Считывает выбранную пользователем опцию.
            {
                // Ползователь нажал 1 или NumPad1.
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear(); // Очищает консоль.
                    InitGame(adminMode); // Начинает игру.
                    break;
                // Ползователь нажал 2 или NumPad2.
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Clear(); // Очищает консоль.
                    Console.WriteLine(rules); // Выводит правила игры.
                    return;
                // Ползователь нажал 3 или NumPad3.
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.Clear(); // Очищает консоль.
                    SwitchMode(ref adminMode); // Вызывает метод, меняющий режим игры.
                    return;
                // Ползователь нажал 4 или NumPad4.
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    Console.Clear(); // Очищает консоль.
                    Console.WriteLine("Чтобы воспользоваться чит-кодом, введите его во время игры.");
                    Console.WriteLine(cheats); // Выводит список читов.
                    return;
                // Ползователь нажал 5 или NumPad5.
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    Console.Clear(); // Очищает консоль.
                    Console.WriteLine("Вы уверены, что хотите выйти?");
                    return; // Выходит из программы.
                // Пользователь нажал любую другую клавишу.
                default:
                    // Выводится соответсвующее сообщение и пользователю предлагают
                    // выйти из программы или вернуться в меню.
                    Console.Clear(); // Очищает консоль.
                    Console.WriteLine("Такой опции нет!");
                    return;
            }
        }

        /// <summary>
        /// Метод начинает игру.
        /// </summary>
        /// <param name="adminMode"> Режим игры. </param>
        public static void InitGame(bool adminMode)
        {
            int len; // Длина загадываемого числа.
            string input; // Строка для ввода.
            bool flag; // Переменная для проверки верности ввода.
            do
            {
                // Пользователь вводит длину загадываемого числа.
                Console.Write("Введите длину загадываемого числа (от 1 до 10): ");
                input = Console.ReadLine();
                // Проверяем корректность ввода.
                flag = int.TryParse(input, out len) && (len >= 1) && (len <= 10);
                // Если ввод неверный, пользователь получает соответсвующее сообщение.
                if (!flag)
                    Console.WriteLine("Ошибка ввода!");
            } while (!flag); // Повторяет ввод, пока он не будет корректным.

            long rightNumber = RandomNum(len); // Загадывает число заданной длины.

            Console.WriteLine($"Игра началась! Длина загаданного числа - {len}. Ваша задача - отгадать его. Удачи!");
            // Если выбран режим игры "администратор", выводит загаданное число.
            if (adminMode) 
                Console.WriteLine($"Загаданное число: {rightNumber}.");

            // Запускается непосредственно процесс игры.
            Game(rightNumber, len);
        }

        /// <summary>
        /// Метод непосредственно реализует процесс игры.
        /// </summary>
        /// <param name="rightNumber"> Загаданное компьютером число. </param>
        /// <param name="len"> Длина загаданного числа. </param>
        public static void Game(long rightNumber, int len)
        {
            long userNumber = 0;    // Переменная для записи пользовательского числа.
            bool flag,              // Переменная для проверки корректности ввода.
                    endOfGame;      // Переменная для отслеживания конца игры.
            string input;           // Переменная для ввода данных.
            // Записываем цифры верного числа в список.
            List<int> rightNumberDigits = NumToList(rightNumber);
            int countTurns = 0; // Считает количество ходов.
            do
            {
                do
                {
                    Console.Write("\nВведите предполагаемое число: ");
                    input = Console.ReadLine(); // Пользователь вводит предполагаемое число.
                    switch (input) // Проверяет, является ли введённая строка чит-кодом или верным числом.
                    {
                        // Чит-код на вывод ответа.
                        case "#answer#":
                            Console.WriteLine("Верное число: " + rightNumber);
                            flag = false;
                            break;  
                        // Чит-код на преждевременное завершение игры.
                        case "#end#":
                            return;
                        // Выполняется, если введённая строка не является чит-кодом.
                        default:
                            // Проверяет, является ли введённая строка числом.
                            flag = IsInputCorrect(input, len, out userNumber);
                            // Если введённая строка - подходящее число, увеличвает кол-во сделанных ходов.
                            if (flag) countTurns++; 
                            break;
                    }
                } while (!flag); // Повторяет ввод, пока он не будет корректным.
                Console.WriteLine(Check(rightNumberDigits, userNumber, out endOfGame)); // Проверяет введённое число.
            } while (!endOfGame); // Повторяется, пока игрок не угадает число.
            // Когда игра окончена, выводится соответсвующее сообщение.
            Console.WriteLine($"Вы победили! Количество потребовавшихся ходов - {countTurns}.");
        }

        /// <summary>
        /// Метод проверяет, является ли ввод корректным.
        /// </summary>
        /// <param name="input"> Введённая строка. </param>
        /// <param name="len"> Длина, которой должно соответствовать введённое число. </param>
        /// <param name="userNumber"> Переменная для возвращения введённого пользователем числа. </param>
        /// <returns> Возвращает true, если ввод был корректным, false - в ином случае. </returns>
        public static bool IsInputCorrect(string input, int len, out long userNumber)
        {
            // Проверяет, является ли введённая строка числом.
            if (!long.TryParse(input, out userNumber))
            {
                Console.WriteLine("Введённое строка не является числом!");
                return false;
            }
            // Если введённая строка - число, проверяет, совпадает ли длина этого числа с длтиной загаданного.
            else if (userNumber.ToString().Length != len)
            {
                Console.WriteLine("Длина введённого числа не совпадает с длиной загаданного.");
                return false;
            }
            // Выполняется, если введённая строка - число необходимого порядка.
            else
                return true;
        }
        
        /// <summary>
        /// Метод реализует возможность смены редима игры.
        /// </summary>
        /// <param name="adminMode"> Перемнная типа bool, сообщающая о текущем режиме игры. </param>
        public static void SwitchMode(ref bool adminMode)
        {
            Console.WriteLine("Для выбора режима нажмите соответствующую клавишу:");
            Console.WriteLine("1. Режим администратора (загаданное число выводится в начале игры).");
            Console.WriteLine("2. Режим игрока (загаданное число изначально неизвестно).");
            
            // Считывает нажатую пользователем клавишу.
            switch (Console.ReadKey().Key)
            {
                // Пользователь нажимает на 1 или NumPad1.
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    adminMode = true;
                    break;
                // Пользователь нажимает на 2 или NumPad2.
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    adminMode = false;
                    break;
                // Если пользователь нажимает любую другую клавишу, программа предлагает
                // выйти из неё или вернуться в меню.
                default:
                    Console.Clear(); // очищает консоль.
                    Console.WriteLine("Такой опции нет!");
                    return;
            }
            Console.WriteLine("\nРежим игры установлен.");
        }
        
        /// <summary>
        /// Метод случайно генерирует число заданной длины, причём все его цифры попарно различны. 
        /// </summary>
        /// <param name="len"> Длина генерируемого числа. </param>
        /// <returns> Возвращает сгенерированное число. </returns>
        public static long RandomNum(int len)
        {
            List<int> digits = new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}; // Список из всех цифр.
            Random rnd = new Random(); // Объект для генерации случайных чисел.
            int pos;        // Вспомогательная переменная, в которую будет записываться случайная позиция в списке.   
            long num = 0;   // Переменная для записи итогового числа.
            for (int i = 0; i < len; i++) // Цикл повторяется len раз и генерирует число с различными цифрами.
            {
                // Случайно генерирует позицию списка, из которой будет взята цифра.
                if ((len > 1) && (i == 0)) 
                    pos = rnd.Next(1, digits.Count); 
                else
                    pos = rnd.Next(digits.Count);
                num = num * 10 + digits[pos];   // В конец генерируемого числа записываем новую цифру,
                                                // которая стоит в списке на полученной позиции.
                digits.RemoveAt(pos); // Из списка доступных цифр убираем использованную.
            }
            return num;
        }

        /// <summary>
        /// Метод проверяет введённое пользователем число и считает количество "быков" и "коров".
        /// </summary>
        /// <param name="rightNumberDigits"> Цифры верного числа в виде списка. </param>
        /// <param name="userNumber"> Введённое пользователем число. </param>
        /// <param name="endOfGame"> Переменная типа bool сообщает завершена ли игра или нет. </param>
        /// <returns> Метод возвращает количество "быков" и коров" в виде строки. </returns>
        public static string Check(List<int> rightNumberDigits, long userNumber, out bool endOfGame)
        {
            // Для проверки записываем цифры введённого числа в список.
            List<int> userNumberDigits = NumToList(userNumber);

            int bulls = 0;  // Переменная для счёта "быков".
            int cows = 0;   // Переменная для счёта "коров".

            // Проверяем каждую цифру пользовательского числа.
            for (int i = 0; i < userNumberDigits.Count; i++)
            {
                // Проверяет, содержится ли проверяемая цифра в загаданном числе.
                if (rightNumberDigits.Contains(userNumberDigits[i]))
                    // Если содержится и стоит на своём месте, увеличивается кол-во "быков".
                    if (userNumberDigits[i] == rightNumberDigits[i])
                        bulls++;
                    // Если содержится, но не стоит на своём месте, то увеличивается кол-во "коров".
                    else
                        cows++;
            }

            // Если количество быков = длине загаданного числа, то игра окончена. 
            // Переменная возвращает значение true в метод Game, если игра окончена.
            endOfGame = bulls == rightNumberDigits.Count;
            
            // Возвращает сообщение о количестве "быков" и "коров" в введённом числе.
            return String.Format("Количество быков - {0}, количество коров - {1}.", bulls, cows);
        }

        /// <summary>
        /// Метод записывает цифры числа в список (в обратном порядке).
        /// </summary>
        /// <param name="num"> Число, цифры которого необходимо записать в список. </param>
        /// <returns> Возвращает цифры числа (в обратном порядке) в виде списка. </returns>
        public static List<int> NumToList(long num)
        {
            List<int> digits = new List<int>(); // Список, в который будут записаны цифры числа.
            // Начиная с последней цифры числа добавляем по одной в список.
            while (num > 0)
            {
                digits.Add((int) num % 10);
                num /= 10;
            }

            // Возвращаем заготовленный список.
            return digits;
        }
    }
}