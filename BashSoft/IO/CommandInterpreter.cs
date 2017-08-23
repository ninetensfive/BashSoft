namespace BashSoft
{
    using System;
    using System.Linq;
    using System.Reflection;
    using BashSoft.Attributes;
    using Contracts;
    using Execptions;
    using IO.Commands;

    public class CommandInterpreter : IInterpreter
    {
        private IContentComparer judge;
        private IDatabase repository;
        private IDirectoryManager inputOutputManager;

        public CommandInterpreter(
            IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }

        public void InterpretCommand(string input)
        {
            string[] data = input.Split();
            string commandName = data[0];

            try
            {
                IExecutable command = this.ParseCommand(input, commandName, data);
                command.Execute();
            }
            catch (Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }
        }

        private IExecutable ParseCommand(string input, string command, string[] data)
        {
            object[] parametersForConstruction = new object[]
            {
                input, data
            };

            Type typeOfCommand = Assembly.GetExecutingAssembly()
                .GetTypes()
                .First(type => type.GetCustomAttributes(typeof(AliasAttribute))
                                   .Where(atr => atr.Equals(command))
                                   .ToArray().Length > 0);

            Type typeOfInterpreter = typeof(CommandInterpreter);

            Command exe = (Command) Activator.CreateInstance(
                typeOfCommand, parametersForConstruction);

            FieldInfo[] fieldsOnCommand = typeOfCommand
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            FieldInfo[] fieldsOnInterpreter = typeOfInterpreter
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fieldOnCommand in fieldsOnCommand)
            {
                Attribute atrAttribute = fieldOnCommand
                    .GetCustomAttribute(typeof(InjectAttribute));

                if (atrAttribute != null)
                {
                    if (fieldsOnInterpreter
                        .Any(x => x.FieldType == fieldOnCommand.FieldType))
                    {
                        fieldOnCommand.SetValue(exe, 
                            fieldsOnInterpreter
                            .First(x => x.FieldType == fieldOnCommand.FieldType)
                            .GetValue(this));
                    }
                }
            }

            return exe;
        }
    }
}