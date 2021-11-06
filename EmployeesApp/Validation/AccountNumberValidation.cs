using System;

namespace EmployeesApp.Validation
{
    public class AccountNumberValidation
    {
        public const int firstSetOfNumbers = 3;
        public const int secondSetOfNumbers = 10;
        public const int lastSetOfNumbers = 2;

        //set the number of parts
        public bool validateAccountNumber(string acctNo)
        {
            var delimiterOne = acctNo.IndexOf('-');

            var delimiterTwo = acctNo.LastIndexOf('-');

            if (delimiterOne == -1 || delimiterTwo == delimiterOne) throw new ArgumentException();

            var accNoSectOne = acctNo.Substring(0, delimiterOne);

            if (accNoSectOne.Length != firstSetOfNumbers)
                return false;

            var tempPart = acctNo.Remove(0, firstSetOfNumbers + 1);

            var accNoSectionTwo = tempPart.Substring(0, tempPart.IndexOf('-'));

            if (accNoSectionTwo.Length != secondSetOfNumbers)
                return false;
            // var accNoSectTwo = acctNo.Substring(accNoSectOne.Length +1, delimiterTwo);

            var accNoSectionThree = acctNo.Substring(delimiterTwo + 1);

            if (accNoSectionThree.Length != lastSetOfNumbers)
                return false;

            return true;
        } 
        //check that the delimiter is -
        //check that the first part
        ///check the second part
        ///check the third part
        ///
    }
}
