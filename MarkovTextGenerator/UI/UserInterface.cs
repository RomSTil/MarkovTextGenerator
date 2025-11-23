using Figgle;
using MarkovTextGenerator.Builder;
using MarkovTextGenerator.Core;
using MarkovTextGenerator.Infrastructure;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MarkovTextGenerator.UI
{
    internal class UserInterface
    {
        private string _filePath;
        private int _order = 2;      // порядок цепи по умолчанию — 2
        private int _length = 50;    // длина генерируемого текста по умолчанию
        private readonly FileWriter _fileWriter = new FileWriter();
        private readonly FileHistory _history = new FileHistory();

        public void Start()
        {
            Console.Title = "Markov Text Generator - DOS Edition";
            Console.Clear();

            while (true)
            {
                Console.Clear();
                ShowHeader();

                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[white]Выберите действие:[/]")
                        .HighlightStyle(new Style(foreground: Color.Yellow, decoration: Decoration.Bold))
                        .PageSize(10)
                        .AddChoices(new[]
                        {
                            "Загрузить текст",
                            "Настроить параметры генерации",
                            "Запустить генерацию текста",
                            "Выход"
                        }));

                switch (selection)
                {
                    case "Загрузить текст":
                        AnimateMenuItem("Загрузить текст");
                        LoadTextMenu();
                        break;

                    case "Настроить параметры генерации":
                        AnimateMenuItem("Настроить параметры генерации");
                        SettingsMenu();
                        break;

                    case "Запустить генерацию текста":
                        AnimateMenuItem("Запустить генерацию текста");
                        GenerateMenu();
                        break;

                    case "Выход":
                        AnimateMenuItem("Выход");
                        ExitProgram();
                        return;
                }
            }
        }

        private void ShowHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            string banner = FiggleFonts.Standard.Render("Markov GPT");
            Console.WriteLine(banner);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("   Текстовый генератор на основе цепей Маркова");
            Console.WriteLine("   -------------------------------------------\n");
        }

        // -----------------------------
        //    АНИМАЦИЯ ДОС-ТОЧЕК
        // -----------------------------
        private void AnimateMenuItem(string text, int cycles = 3, int delay = 120)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < cycles * 4; i++)
            {
                int dots = i % 4;
                string line = text + new string('.', dots);

                Console.Write("\r" + line + new string(' ', 4 - dots));
                Thread.Sleep(delay);
            }

            Console.Write("\r" + text + "    ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");
        }

        // -----------------------------
        //        МЕНЮ ФАЙЛОВ
        // -----------------------------
        private void LoadTextMenu()
        {
            Console.Clear();
            ShowHeader();

            var history = _history.LoadHistory();

            List<string> choices = new List<string>();

            foreach (var h in history)
                choices.Add(h.DisplayName);

            choices.Add("Ввести путь вручную");
            choices.Add("Назад");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Выберите файл или способ загрузки:[/]")
                    .AddChoices(choices));

            if (history.Any(h => h.DisplayName == choice))
            {
                var entry = history.First(h => h.DisplayName == choice);
                _filePath = entry.FilePath;

                AnsiConsole.MarkupLine($"[green]Выбран файл:[/] [white]{_filePath}[/]");
                WaitForKey();
                return;
            }

            if (choice == "Ввести путь вручную")
            {
                string path = AnsiConsole.Ask<string>("[grey]Введите путь к .txt файлу:[/]");

                if (File.Exists(path))
                {
                    _filePath = path;
                    _history.AddPath(path);

                    AnsiConsole.MarkupLine($"[green]Файл добавлен:[/] [white]{_filePath}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Ошибка:[/] файл не найден.");
                }

                WaitForKey();
                return;
            }

            return;
        }

        // -----------------------------
        //       МЕНЮ НАСТРОЕК
        // -----------------------------
        private void SettingsMenu()
        {
            Console.Clear();
            ShowHeader();

            AnsiConsole.MarkupLine("[yellow]НАСТРОЙКИ ГЕНЕРАТОРА[/]\n");

            var order = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[white]Выберите порядок цепи Маркова (рекомендуется 2):[/]")
                    .AddChoices(1, 2));

            _order = order;

            _length = AnsiConsole.Ask<int>("[white]Введите длину генерируемого текста (в словах):[/]", _length);

            AnsiConsole.MarkupLine($"\n[green]Параметры сохранены:[/]");
            AnsiConsole.MarkupLine($"  Порядок цепи: [yellow]{_order}[/]");
            AnsiConsole.MarkupLine($"  Длина текста: [yellow]{_length} слов[/]");

            WaitForKey();
        }

        // -----------------------------
        //      МЕНЮ ГЕНЕРАЦИИ
        // -----------------------------
        private void GenerateMenu()
        {
            Console.Clear();
            ShowHeader();

            AnsiConsole.MarkupLine("[yellow]ГЕНЕРАЦИЯ ТЕКСТА[/]\n");

            if (string.IsNullOrWhiteSpace(_filePath) || !File.Exists(_filePath))
            {
                AnsiConsole.MarkupLine("[red]Ошибка:[/] входной файл не выбран или не существует.");
                AnsiConsole.MarkupLine("[grey]Сначала выберите файл в пункте \"Загрузить текст\".[/]");
                WaitForKey();
                return;
            }

            var builder = new TextGeneratorBuilder()
                .SetFile(_filePath)
                .SetOrder(_order)
                .SetLength(_length);

            var generator = builder.Build();

            AnsiConsole.MarkupLine("[grey]Генерация текста...[/]\n");

            string result = generator.Generate();

            Console.ForegroundColor = ConsoleColor.Green;
            TypeWrite(result, 50);
            Console.ForegroundColor = ConsoleColor.Gray;

            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "generated_output.txt");
            bool saved = _fileWriter.Write(outputPath, result);

            AnsiConsole.WriteLine();
            if (saved)
            {
                AnsiConsole.MarkupLine($"\n[green]Текст сохранён в файл:[/] [white]{outputPath}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[red]Не удалось сохранить файл.[/]");
            }

            WaitForKey();
        }

        private void TypeWrite(string text, int delay = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        private void ExitProgram()
        {
            AnsiConsole.Markup("[green]Завершение работы…[/]");
            Thread.Sleep(600);
        }

        private void WaitForKey()
        {
            AnsiConsole.Markup("\n[grey]Нажмите любую клавишу для продолжения…[/]");
            Console.ReadKey(true);
        }
    }
}
