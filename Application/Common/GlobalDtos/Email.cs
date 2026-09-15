using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common;
public class Email
{
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Password { get; set; }


    public Email()
    {
        From = "IranFoodGuide@iranfoodguide.com";
        //From = "farghadani4747@gmail.com";
        Password = "Kratos1024";
    }
}
