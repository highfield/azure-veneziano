using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AzureVeneziano.WebJob
{
    class CustomLogic
        : ICustomLogic
    {

        public void Run()
        {
            LogicVar analog0 = MachineStatus.Instance.Variables["Analog0"];
            LogicVar analog1 = MachineStatus.Instance.Variables["Analog1"];

            if ((analog0.IsChanged || analog1.IsChanged) &&
                (double)analog0.Value > (double)analog1.Value
                )
            {
                var mail = new MailMessage();
                mail.To.Add("(destination address)");
                mail.Body = "The value of Analog0 is greater than Analog1.";
                MachineStatus.Instance.SendMail(mail);
            }
        }

    }
}
