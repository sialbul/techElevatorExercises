using System;

namespace EmployeeProjects
{
    public static class CLIHelper
    {
		public static object GetChoiceFromOptions(object[] options)
		{
			object choice = null;
			while (choice == null)
			{
				DisplayMenuOptions(options);
				choice = GetChoiceFromUserInput(options);
			}
			return choice;
		}

		private static object GetChoiceFromUserInput(object[] options)
		{
			object choice = null;
			string userInput = Console.ReadLine();

			if (int.TryParse(userInput, out int selectedOption))
			{
				if (selectedOption <= options.Length)
				{
					choice = options[selectedOption - 1];
				}
			}
			if (choice == null)
			{
				Console.WriteLine("\n*** " + userInput + " is not a valid option ***\n");
			}
			return choice;
		}

		private static void DisplayMenuOptions(object[] options)
		{
			Console.WriteLine();
			for (int i = 0; i < options.Length; i++)
			{
				int optionNum = i + 1;
				Console.WriteLine(optionNum + ") " + options[i]);
			}
			Console.Write("\nPlease choose an option >>> ");
		}
	}
}
