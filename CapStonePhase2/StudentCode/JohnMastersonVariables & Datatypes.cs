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
public string DisplayName() { 
string Name = "Dave";
 return Name; 
 }

public int DisplayAge(){
int Age = 20;
return Age;
}

public char DisplayLetter(){
char FirstInital = 'D';
return FirstInital;
}

public double DisplayDouble {
double RandomNumber = 25.4;
return RandomNumber;
}

public float DisplayFloat(){
float AnotherWallet = 5.3;
return AnotherWallet;
}

public bool DisplayFunFact(){
bool IsAwesome = true;
return IsAwesome;
}
}
public class VariableTests    {	Program StartStudentMethod = new Program();public void DoesStudentHaveStringVariable()        {            bool IsStudentVariableString = false;            Type StringVariable = typeof(string);            Type StudentVariable;	    	    public string ReturnedString = StartStudentMethod.DisplayName();            StudentVariable = typeof(ReturnedString);            if (StudentVariable == StringVariable)            {                IsStudentVariableString = true;            }            Console.WriteLine("Is Student's Variable a string?" + IsStudentVariableString);}        public void DoesStudentHaveIntVariable()        {            bool IsStudentVariableInt = false;            Type IntVariable = typeof(int);            Type StudentVariable;	    	    int ReturnedVariable = StartStudentMethod.DisplayAge();            StudentVariable = typeof(ReturnedVariable);            if (StudentVariable == IntVariable)            {                IsStudentVariableInt = true;            }            Console.WriteLine("Is Student's Variable an integer? " + IsStudentVariableInt);        }        public void DoesStudentHaveCharVariable()        {            Type CharVariable = typeof(char);            Type StudentVariable;            bool IsStudentVariableChar = false;	   	    char ReturnedVariable = StartStudentMethod.DisplayLetter();            StudentVariable = typeof(ReturnedVariable);            if (StudentVariable == CharVariable)            {                IsStudentVariableChar = true;            }            Console.WriteLine("Is the Student's Variable a character? " + IsStudentVariableChar);        }        public void DoesStudentHaveDoubleVariable()        {            Type DoubleVariable = typeof(double);            Type StudentVariable;            bool IsStudentVariableDouble = false;	    	    double ReturnedVariable = StartStudentMethod.DisplayDouble();            StudentVariable = typeof(ReturnedVariable);            if (StudentVariable == DoubleVariable)            {                IsStudentVariableDouble = true;            }            Console.WriteLine("Is the Student's Variable a double number? " + IsStudentVariableDouble);        }        public void DoesStudentHaveFloatVariable()        {            Type FloatVariable = typeof(float);            Type StudentVariable;            bool IsStudentVariableFloat = false;	 	    float ReturnedVariable = StartStudentMethod.DisplayFloat();            StudentVariable = typeof(ReturnedVariable);            if (StudentVariable == FloatVariable)            {                IsStudentVariableFloat = true;            }            Console.WriteLine("Is the Student's Variable a floating number? " + IsStudentVariableFloat);        }        public void DoesStudentHaveBooleanVariable()        {            Type BooleanVariable = typeof(bool);            Type StudentVariable;            bool IsStudentVariableBoolean = false;	   	    bool ReturnedVariable = StartStudentMethod.DisplayFunFact();            StudentVariable = typeof(ReturnedVariable);            if (StudentVariable == BooleanVariable)            {                IsStudentVariableBoolean = true;            }            Console.WriteLine("Is the Student’s Variable a boolean?" +IsStudentVariableBoolean);        }    }