using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopoliaTest.Classes
{
    [Serializable]
    public partial class Pallet
    {
        public Pallet() { }
        public Pallet(int _id, string _name, double _w, double _l, double _h) 
        {

            this.Id = _id;
            this.Name = _name;
            this.Width = _w;
            this.Lenght = _l;
            this.Height = _h;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public double Lenght { get; set; }         // Длина

        public double Width { get; set; }          // Ширина
        
        public double Height { get; set; }         // Высота

        public double Capacity                     // Объем
        {
            get
            {
                double _capacity = 0;
                foreach (var b in this.Boxes)
                {
                    _capacity += b.Capacity;
                }
                _capacity += (this.Height * this.Lenght * this.Width);
                return _capacity;
            }
        } 

        public double Weight                       // Вес в кг
        {
            get
            {
                double _weight = 0;
                foreach (var b in this.Boxes)
                {
                    _weight += b.Weight;
                }
                _weight += 30;
                return _weight;
            }
        }

        public DateTime BBD                     // Срок годности Best Before Date
        { 
            get
            {
                DateTime _bd = new DateTime();
                if (Boxes.Count > 0) _bd = Boxes.Min(b =>  b.BBD);
                return _bd;
            }
        }        

        public List<Box> Boxes { get; set; } = new List<Box>();

        public void AddBox(Box b)
        {
            try
            {
                if ((b.Width > this.Width) || (b.Lenght > this.Lenght))
                {
                    throw new Exception("Коробка не может быть больше палеты по длине или ширине");
                }
                Boxes.Add(b);
                Console.WriteLine("Коробка успешно добавлена.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }

    }
}
