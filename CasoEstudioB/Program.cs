using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CasoEstudioB
{
    //Este es un ejemplo de nuestro Caso de Estudio B, agregando
    //funcionalidades de búsqueda, listar, añadir y eliminación de cursos.

    class Curso
    {
        public int Id { get; }
        public string Nombre { get; }
        public string Area { get; }

        public Curso(int id, string nombre, string area)
        {
            Id = id;
            Nombre = nombre ?? string.Empty;
            Area = area ?? string.Empty;
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

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Opciones: 1=Listar, 2=Buscar, 3=Añadir, 4=Eliminar, 0=Salir");
                Console.Write("Seleccione opción: ");
                var opt = Console.ReadLine();

                if (opt == "0") break;

                switch (opt)
                {
                    case "1":
                        Listar(cursos);
                        break;
                    case "2":
                        Buscar(cursos);
                        break;
                    case "3":
                        Añadir(cursos);
                        break;
                    case "4":
                        Eliminar(cursos);
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static void Listar(List<Curso> cursos)
        {
            Console.WriteLine();
            Console.WriteLine("Listado de cursos:");
            if (!cursos.Any())
            {
                Console.WriteLine("No hay cursos.");
                return;
            }

            foreach (var c in cursos)
            {
                Console.WriteLine(c);
            }
        }


        //De la linea 91 hasta la 180 se hizo con ayuda de Copilot
        static void Buscar(List<Curso> cursos)
        {
            Console.WriteLine();
            Console.Write("Buscar por (1) Id, (2) Área, (3) Nombre: ");
            var modo = Console.ReadLine();

            if (modo == "1")
            {
                Console.Write("Id: ");
                var txt = Console.ReadLine();
                int id;
                if (int.TryParse(txt, out id))
                {
                    var encontrado = cursos.FirstOrDefault(c => c.Id == id);
                    Console.WriteLine(encontrado != null ? encontrado.ToString() : "No encontrado.");
                }
                else Console.WriteLine("Id no válido.");
            }
            else if (modo == "2")
            {
                Console.Write("Área: ");
                var area = Console.ReadLine();
                var resultados = cursos.Where(c => string.Equals(c.Area, area ?? string.Empty, StringComparison.OrdinalIgnoreCase)).ToList();
                if (!resultados.Any()) Console.WriteLine("No hay cursos en esa área.");
                else resultados.ForEach(c => Console.WriteLine(c));
            }
            else if (modo == "3")
            {
                Console.Write("Texto en nombre: ");
                var txt = Console.ReadLine() ?? string.Empty;
                var resultados = cursos.Where(c => c.Nombre.IndexOf(txt, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                if (!resultados.Any()) Console.WriteLine("No se encontraron coincidencias.");
                else resultados.ForEach(c => Console.WriteLine(c));
            }
            else
            {
                Console.WriteLine("Modo no válido.");
            }
        }

        static void Añadir(List<Curso> cursos)
        {
            Console.WriteLine();
            Console.Write("Id (número): ");
            var idTxt = Console.ReadLine();
            int id;
            if (!int.TryParse(idTxt, out id))
            {
                Console.WriteLine("Id no válido.");
                return;
            }

            if (cursos.Any(c => c.Id == id))
            {
                Console.WriteLine("Ya existe un curso con ese Id.");
                return;
            }

            Console.Write("Nombre: ");
            var nombre = Console.ReadLine() ?? string.Empty;
            Console.Write("Área: ");
            var area = Console.ReadLine() ?? string.Empty;

            cursos.Add(new Curso(id, nombre, area));
            Console.WriteLine("Curso añadido.");
        }

        static void Eliminar(List<Curso> cursos)
        {
            Console.WriteLine();
            Console.Write("Id del curso a eliminar: ");
            var idTxt = Console.ReadLine();
            int id;
            if (!int.TryParse(idTxt, out id))
            {
                Console.WriteLine("Id no válido.");
                return;
            }

            var c = cursos.FirstOrDefault(x => x.Id == id);
            if (c == null)
            {
                Console.WriteLine("No se encontró curso con ese Id.");
                return;
            }

            cursos.Remove(c);
            Console.WriteLine("Curso eliminado.");
        }
    }
}
