using MonopoliaTest.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Serialization;

namespace MonopoliaTest
{
    internal class Program
    {
        public static HashSet<Pallet> Pallets = new HashSet<Pallet>();
        private static Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
        private static Regex regex = new Regex(@"\D");

        private static int _idPallet = 1;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Добро пожаловать в MonopoliaTest"
                    + "\nВыберите необходимый режим работы"
                    + "\n\n1. Начать заполнение информации о паллетах"
                    + "\n2. Информация о палетах"
                    + "\n3. Информация о 3 палетах с коробками, у которых нибольший срок годности"
                    + "\n\n4. Сохранить информацию в файл"
                    + "\n5. Загрузить информацию из файла");
                switch (char.ToLower(Console.ReadKey(true).KeyChar))
                {
                    case '1':
                        Console.Clear();
                        CreatePalletDialog();
                        break;
                    case '2':
                        Console.Clear();
                        ShowGroupedPallets();
                        break;
                    case '3':
                        Console.Clear();
                        ShowThreePalletsOfMaxBBDBoxes();
                        break;
                    case '4':
                        Console.Clear();
                        SerializeAndSave("InfoOfPallets.xml", Pallets);
                        break;
                    case '5':
                        Console.Clear();
                        Pallets = ReadAndDeserialize("InfoOfPallets.xml");
                        break;
                    default:
                        return;
                }
            }
        }

        private static HashSet<Pallet> ReadAndDeserialize(string _path)
        {
            var serializer = new XmlSerializer(typeof(HashSet<Pallet>));
            using (var reader = new StreamReader(_path))
            {
                return (HashSet<Pallet>)serializer.Deserialize(reader);
            }
        }

        private static void SerializeAndSave(string _path, HashSet<Pallet> _data)
        {
            var serializer = new XmlSerializer(typeof(HashSet<Pallet>));
            using (var writer = new StreamWriter(_path))
            {
                serializer.Serialize(writer, _data);
            }
        }

        static void CreatePalletDialog()
        {
            while (true)
            {
                Console.WriteLine("1. Нажмите 1 для создания палеты" + "\n2. Нажмите любую другую клавишу для выхода в главное меню");

                if (char.ToLower(Console.ReadKey(true).KeyChar) != '1')
                {
                    Console.Clear();
                    break;
                }

                Console.Clear();
                var _p = CreateNewPallet();
                if (_p is not null)
                {
                    Pallets.Add(_p);
                    Console.Clear();
                    break;
                }
            }
        }

        private static bool NeedExit(char _c)
        {
            if (_c == '1')
                return false;
            else
            {
                Console.Clear();
                return true;
            }
        }


        private static bool ReadDataFromConsole(ref string _param, string _description, bool _needexecption, string _exception = "Неизвестная ошибка")
        {
            while (true)
            {

                try
                {
                    Console.Write(_description);

                    _param = Console.ReadLine();

                    if (((_param.Length < 1) || (_param == "")) & _needexecption) throw new Exception(_exception);

                    return false;

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                    Console.WriteLine("Для повторного ввода нажмите 1");

                    if (NeedExit(char.ToLower(Console.ReadKey(true).KeyChar)))
                    {
                        return true;
                    }
                }
            }
        }

        //Создание новой паллеты
        static Pallet CreateNewPallet()
        {

            string name = "", lenght = "", height = "", width = "";
            Pallet p;

            if (ReadDataFromConsole(ref name, "Введите название паллеты: ", true, "название не может быть пустым")) return null;

            if (ReadDataFromConsole(ref lenght, "Введите длину паллеты(число): : ", true, "Длина не может быть меньше 1")) return null;
            lenght = regex.Replace(lenght, separator.ToString());

            if (ReadDataFromConsole(ref width, "Введите ширину паллеты(число): ", true, "Ширина не может быть меньше 1")) return null;
            width = regex.Replace(width, separator.ToString());

            if (ReadDataFromConsole(ref height, "Введите высоту паллеты(число): ", true, "Высота не может быть меньше 1")) return null;
            height = regex.Replace(height, separator.ToString());

            if (Pallets.Count > 0)
            {
                _idPallet = Pallets.Max(p => p.Id);
                _idPallet++;
            }
            try
            {
                p = new Pallet(_idPallet, name, double.Parse(width), double.Parse(lenght), double.Parse(height));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                Console.ReadKey();
                return null;
            }

            Console.Clear();
            Console.WriteLine("Желаете создать коробки для этой паллеты?"
                + "\n\n1. Да"
                + "\n2. Нет");

            while (true)
            {
                if (char.ToLower(Console.ReadKey(true).KeyChar) == '1')
                {
                    CreateNewBoxes(p.Boxes.Count, ref p);
                }
                break;
            }

            return p;

        }

        //создание и добавление новых коробок в паллету
        static void CreateNewBoxes(int _count, ref Pallet _p)
        {
            while (true)
            {
                string name = "", lenght = "", height = "", width = "", weight = "";
                string sdom = "", sbbd = "";
                DateTime dom = new DateTime();
                DateTime bbd = new DateTime();

                Console.Clear();
                if (ReadDataFromConsole(ref name, "Введите название коробки: ", true, "название не может быть пустым")) return;
                if (ReadDataFromConsole(ref lenght, "Введите длину коробки(число): : ", true, "Длина не может быть меньше 1")) return;
                lenght = regex.Replace(lenght, separator.ToString());

                if (ReadDataFromConsole(ref width, "Введите ширину коробки(число): ", true, "Ширина не может быть меньше 1")) return;
                width = regex.Replace(width, separator.ToString());

                if (ReadDataFromConsole(ref height, "Введите высоту коробки(число): ", true, "Высота не может быть меньше 1")) return;
                height = regex.Replace(height, separator.ToString());

                if (ReadDataFromConsole(ref weight, "Вес коробки в кг(число): ", true, "Вес не может быть меньше 1")) return;
                weight = regex.Replace(weight, separator.ToString());

                if (ReadDataFromConsole(ref sdom, "Дата производства коробки в формате дд.ММ.гггг: ", false)) return;
                if (sdom.Length == 10) dom = DateTime.Parse(sdom);

                if (ReadDataFromConsole(ref sbbd, "Срок годности коробки в формате дд.ММ.гггг: ", false)) return;
                if (sbbd.Length == 10) bbd = DateTime.Parse(sbbd);

                try
                {
                    Box b = new Box(++_count, name, double.Parse(width), double.Parse(lenght), double.Parse(height), double.Parse(weight), dom, bbd);
                    _p.AddBox(b);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
                Console.ReadKey();
                Console.Clear();

                Console.WriteLine("Добавить еще 1 коробку?"
                    + "\n\n1. Да"
                    + "\n2. Нет");
                if (char.ToLower(Console.ReadKey(true).KeyChar) != '1') break;
            }
        }

        static void ShowAllBoxes(ICollection<Box> _Boxes)
        {
            foreach (var b in _Boxes)
            {
                Console.WriteLine($"\tId: {b.Id}");
                Console.WriteLine($"\tНазвание: {b.Name}");
                Console.WriteLine($"\tДхШхВ: {b.Lenght}x{b.Width}x{b.Height}");
                Console.WriteLine($"\tОбъем: {string.Format("{0:f3}", b.Capacity)}");
                Console.WriteLine($"\tВес в кг: {string.Format("{0:f3}", b.Weight)}");
                if (b.DoM != DateTime.MinValue) Console.WriteLine($"\tДата производства: {b.DoM.ToString("dd.MM.yyyy")}");
                if (b.BBD != DateTime.MinValue) Console.WriteLine($"\tСрок годности: {b.BBD.ToString("dd.MM.yyyy")}");
                Console.WriteLine();
            }
        }


        private static void ShowInfoOfPallet(Pallet _pallet)
        {
            Console.WriteLine($"Id: {_pallet.Id}");
            Console.WriteLine($"Название: {_pallet.Name}");
            Console.WriteLine($"ДхШхВ: {_pallet.Lenght}x{_pallet.Width}x{_pallet.Height}");
            Console.WriteLine($"Объем: {string.Format("{0:f3}", _pallet.Capacity)}");
            Console.WriteLine($"Вес в кг: {string.Format("{0:f3}", _pallet.Weight)}");
            if (_pallet.BBD != DateTime.MinValue) Console.WriteLine($"Срок годности: {_pallet.BBD.ToString("dd.MM.yyyy")}");
            if (_pallet.Boxes.Count > 0)
            {
                Console.WriteLine("Коробки на палете:");
                ShowAllBoxes(_pallet.Boxes);
            }

        }

        // Вывод сгруппированных и отсортированных паллет
        static void ShowGroupedPallets()
        {
            var GroгpedPallets = Pallets.OrderBy(p => p.BBD).ThenBy(p => p.Weight).GroupBy(p => p.BBD);

            foreach (var bbd in GroгpedPallets)
            {
                Console.WriteLine(bbd.Key.ToString("dd.MM.yyyy"));
                foreach (var pallet in bbd)
                {
                    ShowInfoOfPallet(pallet);
                    Console.WriteLine();
                }
            }

            Console.ReadKey();
            Console.Clear();
        } 

        static void ShowThreePalletsOfMaxBBDBoxes()
        {
            var pallets = Pallets.SelectMany(b => b.Boxes,
                                                (p, b) => new { pallet = p, bbd = b.BBD, boxes = b })
                                                .OrderByDescending(p => p.bbd)
                                                .Select(p => p.pallet)
                                                .Take(3);
                                                


            foreach (var p in pallets) 
            {
                p.Boxes = p.Boxes.OrderByDescending(b => b.Capacity).ToList(); ;
                ShowInfoOfPallet(p);
                Console.WriteLine();
            }

            Console.ReadKey();
            Console.Clear();
        }
    }
}
