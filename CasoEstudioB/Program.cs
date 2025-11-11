using System; //sirve para las operaciones básicas del sistema
using System.Collections.Generic;//sirve para usar listas y otras colecciones
using System.Linq;//sirve para realizar consultas y operaciones sobre colecciones

//El usuario puede ver, buscar, agregar, eliminar y filtrar cursos de forma más organizada, 
//con validaciones y mensajes visuales claras que mejoran la interacción general.

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
                Ui.Title("Gestión de Cursos");

                Ui.Subtitle("Menú principal");
                Console.WriteLine("1) Ver lista de cursos");
                Console.WriteLine("2) Buscar curso por ID");
                Console.WriteLine("3) Ver cursos seleccionados");
                Console.WriteLine("4) Eliminar curso seleccionado");
                Console.WriteLine("5) Filtrar cursos por área");
                Console.WriteLine("6) Agregar nuevo curso");
                Console.WriteLine("7) Salir");

                var opcion = Ui.ReadOption("\nElige una opción", new[] { "1", "2", "3", "4", "5", "6", "7" });
                Console.WriteLine();

                switch (opcion)
                {
                    // VER CURSOS
                    case "1":
                        Ui.Subtitle("Lista de cursos");
                        Ui.PrintTable(
                            new[] { "ID", "Nombre", "Área" },
                            cursos.Select(c => new[] { c.Id.ToString(), c.Nombre, c.Area })
                        );

                        if (cursos.Count > 0)
                        {
                            Console.WriteLine();
                            var sel = Ui.ReadLineOrEmpty("Ingresa un ID para seleccionar (o Enter para omitir)");
                            if (int.TryParse(sel, out int idSel))
                            {
                                var cursoSel = cursos.FirstOrDefault(c => c.Id == idSel);
                                if (cursoSel != null)
                                {
                                    if (cursosSeleccionados.Contains(cursoSel)) // Validación de duplicados
                                    {
                                        Ui.Warning("Ya has seleccionado este curso.");
                                    }
                                    else
                                    {
                                        cursosSeleccionados.Add(cursoSel);
                                        Ui.Success("Curso agregado a seleccionados.");
                                    }
                                }
                                else
                                {
                                    Ui.Error("ID no válido.");
                                }
                            }
                        }
                        Ui.Pause();
                        break;

                    // BUSCAR POR ID
                    case "2":
                        int idBuscado = Ui.ReadInt("Ingresa el ID del curso");
                        var cursoEncontrado = cursos.FirstOrDefault(c => c.Id == idBuscado);

                        if (cursoEncontrado == null) // Validación de existencia
                        {
                            Ui.Error("No existe un curso con ese ID.");
                        }
                        else
                        {
                            Ui.Subtitle("Curso encontrado");
                            Ui.PrintTable(
                                new[] { "ID", "Nombre", "Área" },
                                new[] { new[] { cursoEncontrado.Id.ToString(), cursoEncontrado.Nombre, cursoEncontrado.Area } }
                            );

                            if (Ui.Confirm("\n¿Deseas seleccionarlo?"))
                            {
                                if (cursosSeleccionados.Contains(cursoEncontrado)) // Validación de duplicados
                                {
                                    Ui.Warning("Ya lo has agregado antes.");
                                }
                                else
                                {
                                    cursosSeleccionados.Add(cursoEncontrado); // Agrega a seleccionados
                                    Ui.Success("Curso agregado a seleccionados.");
                                }
                            }
                        }
                        Ui.Pause();
                        break;

                    // VER SELECCIONADOS
                    case "3":
                        Ui.Subtitle("Cursos seleccionados");
                        if (cursosSeleccionados.Count == 0)
                        {
                            Ui.Info("No has seleccionado cursos todavía.");
                        }
                        else
                        {
                            Ui.PrintTable(
                                new[] { "ID", "Nombre", "Área" },
                                cursosSeleccionados.Select(c => new[] { c.Id.ToString(), c.Nombre, c.Area })
                            );
                        }
                        Ui.Pause();
                        break;

                    // ELIMINAR CURSO
                    case "4":
                        Ui.Subtitle("Eliminar curso seleccionado");
                        if (cursosSeleccionados.Count == 0)
                        {
                            Ui.Info("No tienes cursos seleccionados aún.");
                            Ui.Pause();
                            break;
                        }

                        Ui.PrintTable(
                            new[] { "ID", "Nombre", "Área" },
                            cursosSeleccionados.Select(c => new[] { c.Id.ToString(), c.Nombre, c.Area })
                        );

                        int idEliminar = Ui.ReadInt("\nIngresa el ID del curso a eliminar");
                        var cursoEliminar = cursosSeleccionados.FirstOrDefault(c => c.Id == idEliminar);
                        if (cursoEliminar != null) // Validación de existencia
                        {
                            cursosSeleccionados.Remove(cursoEliminar);
                            Ui.Success("Curso eliminado de la lista de seleccionados.");
                        }
                        else
                        {
                            Ui.Error("No se encontró ese curso en tus seleccionados.");
                        }
                        Ui.Pause();
                        break;

                    // FILTRAR POR ÁREA
                    case "5":
                        Ui.Subtitle("Filtrar cursos por área");
                        var areas = cursos
                            .Select(c => c.Area)
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .OrderBy(a => a)
                            .ToList();

                        if (areas.Count == 0)
                        {
                            Ui.Info("No hay áreas disponibles para filtrar.");
                            Ui.Pause();
                            break;
                        }

                        for (int i = 0; i < areas.Count; i++)
                            Console.WriteLine($"{i + 1}) {areas[i]}");

                        int idxArea = Ui.ReadInt("Elige un número de área", 1, areas.Count) - 1;
                        string areaElegida = areas[idxArea];

                        var filtrados = cursos
                            .Where(c => c.Area.Equals(areaElegida, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        if (filtrados.Count == 0)
                        {
                            Ui.Info("No hay cursos en esa área.");
                        }
                        else
                        {
                            Ui.Subtitle($"Cursos en área: {areaElegida}");
                            Ui.PrintTable(
                                new[] { "ID", "Nombre", "Área" },
                                filtrados.Select(c => new[] { c.Id.ToString(), c.Nombre, c.Area })
                            );

                            var selId = Ui.ReadLineOrEmpty("\nIngresa un ID para seleccionar (o Enter para omitir)");
                            if (int.TryParse(selId, out int idSelFiltro))
                            {
                                var cursoSel = filtrados.FirstOrDefault(c => c.Id == idSelFiltro);
                                if (cursoSel != null)
                                {
                                    if (cursosSeleccionados.Contains(cursoSel))
                                        Ui.Warning("Ese curso ya está en seleccionados.");
                                    else
                                    {
                                        cursosSeleccionados.Add(cursoSel);
                                        Ui.Success("Curso agregado a seleccionados.");
                                    }
                                }
                                else
                                {
                                    Ui.Error("ID no válido en el filtro actual.");
                                }
                            }
                        }
                        Ui.Pause();
                        break;

                    // AGREGAR NUEVO CURSO
                    case "6":
                        Ui.Subtitle("Agregar nuevo curso");

                        string nuevoNombre = Ui.ReadNonEmpty("Nombre del nuevo curso");
                        string nuevaArea = Ui.ReadNonEmpty("Área del curso");

                        // Validación simple de duplicados por nombre y área (ignorando mayúsculas/minúsculas)
                        bool duplicado = cursos.Any(c =>
                            c.Nombre.Equals(nuevoNombre, StringComparison.OrdinalIgnoreCase) &&
                            c.Area.Equals(nuevaArea, StringComparison.OrdinalIgnoreCase));

                        if (duplicado)
                        {
                            Ui.Warning("Ya existe un curso con el mismo nombre en esa área.");
                            Ui.Pause();
                            break;
                        }

                        int nuevoId = (cursos.Count == 0 ? 1 : cursos.Max(c => c.Id) + 1); // Genera nuevo ID
                        cursos.Add(new Curso(nuevoId, nuevoNombre, nuevaArea));
                        Ui.Success($"Curso '{nuevoNombre}' agregado con ID {nuevoId}.");

                        Ui.Pause();
                        break;

                    // SALIR
                    case "7":
                        continuar = false;
                        Ui.Info("Saliendo...");
                        break;
                }
            }
        }
    }

    // Utilidades de interfaz de consola para mejorar la presentación y la entrada de datos
    static class Ui
    {
        public static void Title(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Line('═', text.Length + 8); // Línea superior
            Console.WriteLine($"══  {text}  ══");
            Line('═', text.Length + 8); // Línea inferior
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void Subtitle(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"▸ {text}");
            Console.ResetColor();
        }

        public static void Info(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void Success(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void Warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void Pause(string message = "Presiona una tecla para continuar...")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Error("El valor no puede estar vacío.");
            }
        }

        public static string ReadLineOrEmpty(string prompt)
        {
            Console.Write($"{prompt}: ");
            var input = Console.ReadLine();
            return input == null ? string.Empty : input.Trim();
        }

        public static int ReadInt(string prompt, int? min = null, int? max = null)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine();
                int value;
                if (!int.TryParse(input, out value))
                {
                    Error("Ingresa un número válido.");
                    continue;
                }

                if (min.HasValue && value < min.Value)
                {
                    Error($"El valor debe ser mayor o igual a {min.Value}.");
                    continue;
                }

                if (max.HasValue && value > max.Value)
                {
                    Error($"El valor debe ser menor o igual a {max.Value}.");
                    continue;
                }

                return value;
            }
        }

        public static string ReadOption(string prompt, IEnumerable<string> allowed)
        {
            var set = new HashSet<string>(allowed);
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = (Console.ReadLine() ?? string.Empty).Trim();
                if (set.Contains(input))
                    return input;

                Error("Opción inválida.");
            }
        }

        public static bool Confirm(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (s/n): ");
                var input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
                if (input == "s" || input == "si" || input == "sí")
                    return true;
                if (input == "n" || input == "no")
                    return false;

                Error("Responde con 's' o 'n'.");
            }
        }

        public static void PrintTable(string[] headers, IEnumerable<string[]> rows)
        {
            // Calcular anchos
            var lista = rows?.ToList() ?? new List<string[]>();
            var cols = headers.Length;
            var widths = new int[cols];

            for (int i = 0; i < cols; i++)
                widths[i] = headers[i].Length;

            foreach (var r in lista)
            {
                for (int i = 0; i < cols && i < r.Length; i++)
                    widths[i] = Math.Max(widths[i], (r[i] ?? string.Empty).Length);
            }

            // Dibujar
            DrawSeparator(widths);
            DrawRow(headers, widths, isHeader: true);
            DrawSeparator(widths);
            foreach (var r in lista)
                DrawRow(r, widths, isHeader: false);
            DrawSeparator(widths);
        }

        static void DrawSeparator(int[] widths)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write('+');
            for (int i = 0; i < widths.Length; i++)
            {
                Console.Write(new string('─', widths[i] + 2));
                Console.Write('+');
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        static void DrawRow(string[] row, int[] widths, bool isHeader)
        {
            Console.Write('|');
            for (int i = 0; i < widths.Length; i++)
            {
                var cell = i < row.Length ? (row[i] ?? string.Empty) : string.Empty;
                if (isHeader) Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + cell.PadRight(widths[i]) + " ");
                Console.ResetColor();
                Console.Write('|');
            }
            Console.WriteLine();
        }

        static void Line(char ch, int count)
        {
            Console.WriteLine(new string(ch, Math.Max(1, count)));
        }
    }
}