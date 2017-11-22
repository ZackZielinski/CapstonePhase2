using System;

public class Program {

static void Main(){ 

VariableTests StudentTest = new VariableTests();

StudentTest.DoesStudentHaveStringVariable();

StudentTest.DoesStudentHaveIntVariable();

StudentTest.DoesStudentHaveCharVariable();

StudentTest.DoesStudentHaveDoubleVariable();

StudentTest.DoesStudentHaveFloatVariable();

StudentTest.DoesStudentHaveBooleanVariable();

 }

}

public class VariableTests
    {

public void DoesStudentHaveStringVariable()
        {
            bool IsStudentVariableString = false;
            Type StringVariable = typeof(string);
            Type StudentVariable;

            StudentVariable = typeof(Program.DisplayName());


            if (StudentVariable == StringVariable)
            {
                IsStudentVariableString = true;
            }

            Console.WriteLine("Is Student's Variable a string?" + IsStudentVariableString);
}


        public void DoesStudentHaveIntVariable()
        {
            bool IsStudentVariableInt = false;
            Type IntVariable = typeof(int);
            Type StudentVariable;

            StudentVariable = typeof(Program.DisplayAge());


            if (StudentVariable == IntVariable)
            {
                IsStudentVariableInt = true;
            }

            Console.WriteLine("Is Student's Variable an integer? " + IsStudentVariableInt);
        }

        public void DoesStudentHaveCharVariable()
        {
            Type CharVariable = typeof(char);
            Type StudentVariable;
            bool IsStudentVariableChar = false;

            StudentVariable = typeof(Program.DisplayLetter());

            if (StudentVariable == CharVariable)
            {
                IsStudentVariableChar = true;
            }

            Console.WriteLine("Is the Student's Variable a character? " + IsStudentVariableChar);

        }


        public void DoesStudentHaveDoubleVariable()
        {
            Type DoubleVariable = typeof(double);
            Type StudentVariable;
            bool IsStudentVariableDouble = false;

            StudentVariable = typeof(Program.DisplayDouble());

            if (StudentVariable == DoubleVariable)
            {
                IsStudentVariableDouble = true;
            }

            Console.WriteLine("Is the Student's Variable a double number? " + IsStudentVariableDouble);

        }


        public void DoesStudentHaveFloatVariable()
        {
            Type FloatVariable = typeof(float);
            Type StudentVariable;
            bool IsStudentVariableFloat = false;

            StudentVariable = typeof(Program.DisplayFloat());

            if (StudentVariable == FloatVariable)
            {
                IsStudentVariableFloat = true;
            }

            Console.WriteLine("Is the Student's Variable a floating number? " + IsStudentVariableFloat);

        }

        public void DoesStudentHaveBooleanVariable()
        {
            Type BooleanVariable = typeof(bool);
            Type StudentVariable;
            bool IsStudentVariableBoolean = false;

            StudentVariable = typeof(Program.DisplayFunFact());

            if (StudentVariable == BooleanVariable)
            {
                IsStudentVariableBoolean = true;
            }

            Console.WriteLine("Is the Student’s Variable a boolean ?" +IsStudentVariableBoolean);


        }

    }
