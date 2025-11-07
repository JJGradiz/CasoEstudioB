# Documentación del Proyecto CasoEstudioB

## Resumen General
CasoEstudioB es una aplicación de consola en C# que implementa un sistema de gestión de cursos educativos Program.cs . La aplicación permite a los usuarios ver, buscar, agregar, eliminar y filtrar cursos de manera interactiva a través de un menú de consola Program.cs


## Arquitectura del Sistema
El proyecto sigue una arquitectura monolítica donde toda la lógica reside en un único archivo Program.cs. La aplicación consta de dos clases principales:

### 1. Clase Curso (Entidad de Datos)
Define la estructura de un curso con tres propiedades inmutables

Id: Identificador único del curso
Nombre: Nombre del curso
Area: Área temática (Informática, Ingeniería, Software, Matemáticas)

#### C# Code

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

El constructor valida que los parámetros no sean nulos Program.cs: 16-21 , y la clase sobrescribe ToString() para mostrar el formato "{Id}: {Nombre} ({Area})" Program.cs: 23-26 .

### 2. Clase Program (Lógica de Aplicación)
Contiene el método Main que gestiona toda la aplicación Program.cs: 29-31 .

###Gestión de Estado
La aplicación mantiene dos listas en memoria Program.cs: 33-41 :

cursos: Catálogo maestro con 4 cursos iniciales (Programación Estructurada, Robótica, Análisis y Diseño, Cálculo)
cursosSeleccionados: Lista de cursos que el usuario ha seleccionado

#### C# Code
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

Importante: Los datos solo existen en memoria y se pierden al cerrar la aplicación.

## Funcionalidades del Menú
El sistema ofrece 7 opciones principales Program.cs: 52-58 :

### Opción 1: Ver Lista de Cursos
Muestra todos los cursos disponibles y permite seleccionar uno por ID Program.cs:67-88 . Valida que el curso no esté ya seleccionado antes de agregarlo Program.cs:80-84 .

#### C# Code 
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
                                if (cursosSeleccionados.Contains(cursoSel))
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

### Opción 2: Buscar por ID
Busca un curso específico usando LINQ FirstOrDefault Program.cs:108 y ofrece seleccionarlo si existe Program.cs:119-135 .

#### C# Code
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
                                    if (cursosSeleccionados.Contains(cursoEncontrado))
                                    {
                                        Console.WriteLine("Ya lo has agregado antes.");
                                    }
                                    else
                                    {
                                        cursosSeleccionados.Add(cursoEncontrado);
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


### Opción 3: Ver Cursos Seleccionados
Lista todos los cursos que el usuario ha agregado a su selección Program.cs:145-151 .

#### C# Code
case "3":
                        Console.WriteLine("Cursos seleccionados:");
                        if (cursosSeleccionados.Count == 0)
                            Console.WriteLine("No has seleccionado cursos todavía.");
                        else
                            foreach (var c in cursosSeleccionados)
                                Console.WriteLine(c);
                        break;


### Opción 4: Eliminar Curso Seleccionado
Remueve un curso de la lista de seleccionados (no del catálogo principal) Program.cs:155-165 .

#### C# Code 
  case "4":
                        Console.Write("Ingresa el ID del curso a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int idEliminar))
                        {
                            var cursoEliminar = cursosSeleccionados.FirstOrDefault(c => c.Id == idEliminar);
                            if (cursoEliminar != null)
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

### Opción 5: Filtrar por Área
Filtra cursos usando comparación case-insensitive del área Program.cs:178-180 .

#### C# Code
case "5":
                        Console.Write("Ingresa el área a filtrar: ");
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

### Opción 6: Agregar Nuevo Curso
Crea un curso con ID autogenerado (máximo ID actual + 1) Program.cs:200-202 y lo agrega al catálogo principal.

#### C# Code 

case "6":
                        Console.Write("Nombre del nuevo curso: ");
                        string nuevoNombre = Console.ReadLine();
                        Console.Write("Área del curso: ");
                        string nuevaArea = Console.ReadLine();
                        int nuevoId = cursos.Max(c => c.Id) + 1;

                        cursos.Add(new Curso(nuevoId, nuevoNombre, nuevaArea));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Curso agregado exitosamente.");
                        Console.ResetColor();
                        break;

### Opción 7: Salir
Termina la ejecución del programa Program.cs:209-211 .

#### C# Code

 case "7":
                        continuar = false;
                        Console.WriteLine("Saliendo...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;

## Flujo de Control
La aplicación ejecuta un bucle while continuo Program.cs:45 que:

1. Limpia la consola
2. Muestra el menú con colores (Cyan para el encabezado) Program.cs:47-50
3. Lee la opción del usuario
4. Ejecuta la operación correspondiente mediante switch Program.cs:64
5. Espera una tecla antes de continuar Program.cs:219-220

## Características Técnicas
1. Validación de entrada: Uso de int.TryParse para validar IDs numéricos
2. LINQ: Consultas con FirstOrDefault, Where, Max, Contains
3. Inmutabilidad: Propiedades de Curso son de solo lectura
4. Feedback visual: Mensajes con colores (Verde para éxito, Rojo para eliminación, Amarillo para advertencias)

### Fin