using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace StackDev
{
    public class Cli
    {
        private const string RutaArchivoVariables = "variables.json";

        private static Dictionary<string, double> variables = new Dictionary<string, double>();

        // Palabras reservadas que no se pueden usar como nombre de variable.
        private static readonly string[] comandosReservados = { "help", "clear", "show", "rm" };

        static Cli()
        {
            CargarVariables();
        }

        public static void Help()
        {
            Console.WriteLine("//////// Commands ////////");
            Console.WriteLine();
            Console.WriteLine("-- help      Show this page");
            Console.WriteLine("-- clear     Clear the terminal");
            Console.WriteLine("-- show      Show every variable");
            Console.WriteLine("-- rm        List variables by id and remove one");
            Console.WriteLine();
            Console.WriteLine("//////// Actions ////////");
            Console.WriteLine();
            Console.WriteLine("To create/reassign a variable: '<name> = <value or expression>'");
            Console.WriteLine("To evaluate an operation, just type the expression");
            Console.WriteLine();
        }

        public static void Verifica(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            string normalizado = Program.Normalize(input);

            if (normalizado.Contains('='))
            {
                AsignarVariable(normalizado);
                return;
            }

            bool op = normalizado.Any(c => Program.operadores.ContainsKey(c));

            if (op)
            {
                EvaluarExpresion(normalizado);
                return;
            }

            // Buscar var
            if (variables.ContainsKey(normalizado))
            {
                Console.WriteLine($"{normalizado} = {variables[normalizado]}");
                return;
            }

            Command(normalizado);
        }

        public static void Command(string input)
        {
            switch (input)
            {
                case "help":
                    Help();
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "show":
                    MostrarVariables();
                    break;

                case "rm":
                    Rm();
                    break;

                default:
                    Console.WriteLine($"Papu: Unknow Command: {input}");
                    break;
            }
        }

        private static void MostrarVariables()
        {
            if (variables.Count == 0)
            {
                Console.WriteLine("No hay variables definidas.");
                return;
            }

            foreach (KeyValuePair<string, double> papu in variables)
            {
                Console.WriteLine($"{papu.Key} = {papu.Value}");
            }
        }

        private static void Rm()
        {
            if (variables.Count == 0)
            {
                Console.WriteLine("No hay variables definidas.");
                return;
            }

            List<string> nombres = variables.Keys.ToList();

            Console.WriteLine("Variables disponibles:");
            for (int i = 0; i < nombres.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {nombres[i]} = {variables[nombres[i]]}");
            }
            Console.WriteLine("[0] Cancelar");
            Console.Write("Id Var: ");

            string seleccion = Console.ReadLine();

            if (!int.TryParse(seleccion, out int id))
            {
                Console.WriteLine("Error: entrada invalida.");
                return;
            }

            if (id == 0)
            {
                Console.WriteLine("Operacion cancelada.");
                return;
            }
            else if (id < 1 || id > nombres.Count)
            {
                Console.WriteLine("Error: id fuera de rango.");
                return;
            }

            string nombreEliminar = nombres[id - 1];
            variables.Remove(nombreEliminar);
            GuardarVariables();
            Console.WriteLine($"Variable '{nombreEliminar}' eliminada.");
        }

        private static void AsignarVariable(string input)
        {
            string[] partes = input.Split('=');

            if (partes.Length != 2)
            {
                Console.WriteLine("Error: sintaxis invalida");
                return;
            }

            string nombre = partes[0];
            string valorExpr = partes[1];

            if (!Valid_Name(nombre))
            {
                Console.WriteLine($"Error: '{nombre}' no es un nombre de variable valido.");
                return;
            }

            if (string.IsNullOrEmpty(valorExpr))
            {
                Console.WriteLine("Error: falta el valor a asignar.");
                return;
            }

            try
            {
                double valor = EvaluarValor(valorExpr);
                bool reasignada = variables.ContainsKey(nombre);
                variables[nombre] = valor;
                GuardarVariables();
                Console.WriteLine(reasignada ? $"{nombre} reasignada = {valor}" : $"{nombre} = {valor}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private static void EvaluarExpresion(string input)
        {
            try
            {
                double resultado = EvaluarValor(input);
                Console.WriteLine($"Resultado: {resultado}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static double EvaluarValor(string input)
        {
            string papu = ReplaceVariables(input);

            if (!Program.CheckParentesis(papu))
            {
                throw new Exception("Los parentesis no estan balanceados.");
            }

            List<string> rpn = Program.ConvertToRPN(papu);
            List<double> resultado = Program.ExecutorOp(rpn);

            if (resultado.Count != 1)
            {
                throw new Exception("No se pudo calcular el resultado de la input.");
            }

            return resultado[0];
        }


        private static string ReplaceVariables(string input)
        {
            string resultado = "";
            int i = 0;

            while (i < input.Length)
            {
                char c = input[i];

                if (char.IsLetter(c) || c == '_')
                {
                    string identificador = "";
                    while (i < input.Length && (char.IsLetterOrDigit(input[i]) || input[i] == '_'))
                    {
                        identificador += input[i];
                        i++;
                    }

                    if (!variables.ContainsKey(identificador))
                    {
                        throw new Exception($"La variable '{identificador}' no existe.");
                    }

                    resultado += variables[identificador].ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    resultado += c;
                    i++;
                }
            }

            return resultado;
        }

        private static bool Valid_Name(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (!char.IsLetter(input[0]) && input[0] != '_')
            {
                return false;
            }

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                {
                    return false;
                }
            }

            // Buscamos el input dentro de comandos reservados
            if (Array.IndexOf(comandosReservados, input) >= 0)
            {
                // Si aparece alguna vez lo matamos.
                return false;
            }

            // Sobrevivio el input
            return true;
        }

        // ---------------- JSON ----------------

        private static void CargarVariables()
        {
            try
            {
                if (File.Exists(RutaArchivoVariables))
                {
                    string json = File.ReadAllText(RutaArchivoVariables);
                    Dictionary<string, double> datos = JsonSerializer.Deserialize<Dictionary<string, double>>(json);
                    variables = datos ?? new Dictionary<string, double>();
                }
                else
                {
                    variables = new Dictionary<string, double>();
                    GuardarVariables(); // crea el archivo vacio la primera vez
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aviso: no se pudo leer {RutaArchivoVariables} ({ex.Message}). Se inicia sin variables.");
                variables = new Dictionary<string, double>();
            }
        }

        private static void GuardarVariables()
        {
            try
            {
                JsonSerializerOptions opciones = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(variables, opciones);
                File.WriteAllText(RutaArchivoVariables, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aviso: no se pudo guardar {RutaArchivoVariables} ({ex.Message}).");
            }
        }
    }
}