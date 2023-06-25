using System;
using System.Collections.Generic;
//using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopoliaTest.Classes
{

    [Serializable]
    public partial class Box
    {
        public Box() { }
        public Box(int id, string name, double w, double l, double h, double wh, DateTime dom, DateTime bbd)
        {
            this.Id = id;
            this.Name = name;
            this.Width = w;
            this.Lenght = l;
            this.Height = h;
            this.Weight = wh;
            if ((dom != DateTime.MinValue)||(bbd != DateTime.MinValue))
            {
                if (bbd == DateTime.MinValue)
                {
                    this.BBD = dom.AddDays(100);

                }
                else
                    this.BBD = bbd;

                this.DoM = dom;
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DoM { get; set; }           // Дата производства "Date Of Manufacture"
        public DateTime BBD { get; set; }           // срок годности Best Before Date

        public double Weight { get; set; }             // вес в кг

        public double Lenght { get; set; }             // длина

        public double Width { get; set; }              // ширина

        public double Height { get; set; }             // высота

        public double Capacity                         // объем
        { 
            get
            {
                return (Lenght * Width * Height);
            }
        } 
    }
}
