using System;
using System.Collections.Generic;
using System.Linq;

//El usuario puede ver, buscar, agregar, eliminar y filtrar cursos de forma más organizada, 
//con validaciones y mensajes visuales claros que mejoran la interacción general.

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

            var cursosSeleccionados = new List<Curso>();

            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine("\n--- MENÚ ---");
                Console.WriteLine("1. Ver lista de cursos");
                Console.WriteLine("2. Buscar curso por ID");
                Console.WriteLine("3. Ver cursos seleccionados");
                Console.WriteLine("4. Salir");
                Console.Write("Elige una opción: ");

                string opcion = Console.ReadLine();

                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine("Lista de cursos:");
                        foreach (var c in cursos)
                            Console.WriteLine(c);  // Ya incluye el ID en ToString()

                        Console.Write("\n¿Deseas seleccionar alguno? (id / no): ");
                        string sel = Console.ReadLine();

                        if (int.TryParse(sel, out int idSel))
                        {
                            var cursoSel = cursos.FirstOrDefault(c => c.Id == idSel);
                            if (cursoSel != null)
                            {
                                cursosSeleccionados.Add(cursoSel);
                                Console.WriteLine("Curso agregado.");
                            }
                            else
                                Console.WriteLine("ID no válido.");
                        }
                        break;

                    case "2":
                        Console.Write("Ingresa el ID del curso: ");
                        string inputId = Console.ReadLine();

                        if (int.TryParse(inputId, out int idBuscado))
                        {
                            var cursoEncontrado = cursos.FirstOrDefault(c => c.Id == idBuscado);

                            if (cursoEncontrado == null)
                            {
                                Console.WriteLine("\nNo existe un curso con ese ID.");
                            }
                            else
                            {
                                Console.WriteLine("\nCurso encontrado:");
                                Console.WriteLine(cursoEncontrado);

                                Console.Write("\n¿Deseas seleccionarlo? (si/no): ");
                                string resp = Console.ReadLine().ToLower();

                                if (resp == "si")
                                {
                                    cursosSeleccionados.Add(cursoEncontrado);
                                    Console.WriteLine("Curso agregado.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID inválido.");
                        }

                        break;

                    case "3":
                        Console.WriteLine("Cursos seleccionados:");
                        if (cursosSeleccionados.Count == 0)
                            Console.WriteLine("No has seleccionado cursos todavía.");
                        else
                            foreach (var c in cursosSeleccionados)
                                Console.WriteLine(c);
                        break;

                    case "4":
                        continuar = false;
                        Console.WriteLine("Saliendo...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
        }
    }
}
