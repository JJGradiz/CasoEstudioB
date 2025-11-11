using System; //sirve para las operaciones básicas del sistema
using System.Collections.Generic;//sirve para usar listas y otras colecciones
using System.Linq;//sirve para realizar consultas y operaciones sobre colecciones

//El usuario puede ver, buscar, agregar, eliminar y filtrar cursos de forma más organizada, 
//con validaciones y mensajes visuales claros que mejoran la interacción general.

namespace CasoEstudioB
{
    class Curso // Representa un curso con ID, nombre y área
    {
        public int Id { get; }
        public string Nombre { get; }
        public string Area { get; }

        public Curso(int id, string nombre, string area) // Constructor del curso
        {
            Id = id;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Area = area ?? throw new ArgumentNullException(nameof(area));
        }

        public override string ToString() // Representación en cadena del curso
        {
            return $"{Id}: {Nombre} ({Area})"; // formato de salida
        }
    }

    internal class Program
    {
        static void Main(string[] args) // Punto de entrada del programa
        {
            var cursos = new List<Curso> // Lista inicial de cursos disponibles
            {
                new Curso(1, "Programación Estructurada", "Informática"),
                new Curso(2, "Robótica", "Ingeniería"),
                new Curso(3, "Análisis y Diseño", "Software"),
                new Curso(4, "Cálculo", "Matemáticas")
            };

            var cursosSeleccionados = new List<Curso>(); // Lista de cursos seleccionados por el usuario

            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--- MENÚ ---"); // Título del menú
                Console.ResetColor();

                Console.WriteLine("1. Ver lista de cursos");
                Console.WriteLine("2. Buscar curso por ID");
                Console.WriteLine("3. Ver cursos seleccionados");
                Console.WriteLine("4. Eliminar curso seleccionado");
                Console.WriteLine("5. Filtrar cursos por área");
                Console.WriteLine("6. Agregar nuevo curso");
                Console.WriteLine("7. Salir");
                Console.Write("\nElige una opción: ");

                string opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    // VER CURSOS
                    case "1":
                        Console.WriteLine("Lista de cursos:");
                        foreach (var c in cursos)
                            Console.WriteLine(c);

                        Console.Write("\n¿Deseas seleccionar alguno? (id / no): ");
                        string sel = Console.ReadLine();

                        if (int.TryParse(sel, out int idSel))
                        {
                            var cursoSel = cursos.FirstOrDefault(c => c.Id == idSel);
                            if (cursoSel != null)
                            {
                                if (cursosSeleccionados.Contains(cursoSel)) // Validación de duplicados
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Ya has seleccionado este curso.");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    cursosSeleccionados.Add(cursoSel);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Curso agregado exitosamente.");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                Console.WriteLine("ID no válido.");
                            }
                        }
                        break;

                    // BUSCAR POR ID
                    case "2":
                        Console.Write("Ingresa el ID del curso: "); // Solicita ID al usuario
                        string inputId = Console.ReadLine();

                        if (int.TryParse(inputId, out int idBuscado))
                        {
                            var cursoEncontrado = cursos.FirstOrDefault(c => c.Id == idBuscado);

                            if (cursoEncontrado == null) // Validación de existencia
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
                                    if (cursosSeleccionados.Contains(cursoEncontrado)) // Validación de duplicados
                                    {
                                        Console.WriteLine("Ya lo has agregado antes.");
                                    }
                                    else
                                    {
                                        cursosSeleccionados.Add(cursoEncontrado); // Agrega a seleccionados
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Curso agregado exitosamente.");
                                        Console.ResetColor();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID inválido.");
                        }
                        break;

                    // VER SELECCIONADOS
                    case "3":
                        Console.WriteLine("Cursos seleccionados:");
                        if (cursosSeleccionados.Count == 0)
                            Console.WriteLine("No has seleccionado cursos todavía.");
                        else
                            foreach (var c in cursosSeleccionados)
                                Console.WriteLine(c);
                        break;

                    // ELIMINAR CURSO
                    case "4":
                        Console.Write("Ingresa el ID del curso a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int idEliminar))
                        {
                            var cursoEliminar = cursosSeleccionados.FirstOrDefault(c => c.Id == idEliminar);
                            if (cursoEliminar != null) // Validación de existencia
                            {
                                cursosSeleccionados.Remove(cursoEliminar);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Curso eliminado de la lista.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine("No se encontró ese curso en tus seleccionados.");
                            }
                        }
                        break;

                    // FILTRAR POR ÁREA
                    case "5":
                        Console.Write("Ingresa el área a filtrar: "); // Solicita área al usuario
                        string area = Console.ReadLine();
                        var filtrados = cursos
                            .Where(c => c.Area.Equals(area, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        if (filtrados.Count == 0)
                        {
                            Console.WriteLine("No hay cursos en esa área.");
                        }
                        else
                        {
                            Console.WriteLine("Cursos encontrados:");
                            foreach (var c in filtrados)
                                Console.WriteLine(c);
                        }
                        break;

                    // AGREGAR NUEVO CURSO
                    case "6":
                        Console.Write("Nombre del nuevo curso: ");
                        string nuevoNombre = Console.ReadLine();
                        Console.Write("Área del curso: ");
                        string nuevaArea = Console.ReadLine();
                        int nuevoId = cursos.Max(c => c.Id) + 1; // Genera nuevo ID

                        cursos.Add(new Curso(nuevoId, nuevoNombre, nuevaArea));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Curso agregado exitosamente.");
                        Console.ResetColor();
                        break;

                    // SALIR
                    case "7":
                        continuar = false;
                        Console.WriteLine("Saliendo...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                Console.WriteLine("\nPresiona una tecla para continuar...");
                Console.ReadKey(); // Pausa antes de refrescar el menú
            }
        }
    }
}