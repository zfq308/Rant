﻿using System;
using static System.Console;
using static Rave.CmdLine;
using Rave.Build;
using Rave.DicSort;
using Rave.Packer;
using Rave.DicDoc;

namespace Rave
{
	class Program
	{
		static void Main(string[] args)
		{
			if (String.IsNullOrEmpty(Command))
			{
				Help.Print();
				return;
			}
#if !DEBUG
            try
			{
#endif
                switch (Command)
				{
					case "docs":
						{
							DocGenerator.Run();
							break;
						}
					case "sort":
						{
							TableSorter.Run();
							break;
						}
					case "pack":
						{
							PackGenerator.Run();
							break;
						}
					case "build":
						{
                            PatternBuilder.Run();
							break;
						}
					case "help":
						{
							foreach (var name in GetPaths())
							{
								WriteLine($"'{name}'");

								switch (name.ToLower())
								{
									case "docs":
										DocGenerator.GetHelp();
										break;
									case "sort":
										TableSorter.GetHelp();
										break;
									case "pack":
										PackGenerator.GetHelp();
										break;
                                    case "build":
                                        PatternBuilder.GetHelp();
                                        break;
									case "help":
										WriteLine("Are you serious?");
										break;
									default:
										WriteLine($"No help info found for '{name}'");
										break;
								}
								WriteLine();
							}
							break;
						}
					default:
						WriteLine($"Unknown command: '{Command}'");
						break;
				}
#if !DEBUG
            }
			catch (Exception ex)
			{
				ForegroundColor = ConsoleColor.Red;
				WriteLine(ex.Message);
				ResetColor();
				Environment.Exit(1);
			}
#endif
        }
	}
}
