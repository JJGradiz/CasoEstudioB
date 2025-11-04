using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasoEstudioB
{
    class Curso
    {
        public int Id { get; }
        public string Nombre { get; }
        public string Area { get; }

        public Curso(int id, string nombre, string area)
        {
            Id = id;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Area = area ?? throw new ArgumentNullException(nameof(area));
        }

        public override string ToString()
        {
            return $"{Id}: {Nombre} ({Area})";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var cursos = new List<Curso>
            {
                new Curso(1, "Programación Estructurada", "Informática"),
                new Curso(2, "Robótica", "Ingeniería"),
                new Curso(3, "Análisis y Diseño", "Software"),
                new Curso(4, "Cálculo", "Matemáticas")
            };

            foreach (var curso in cursos)
            {
                Console.WriteLine(curso);
            }
        }
    }
}