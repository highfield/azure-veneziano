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
                var message = "The value of Analog0 is greater than Analog1.";

                var mail = new MailMessage();
                mail.To.Add("(destination address)");
                mail.Body = message;

                var rdp = new ReportDataParameters();
                rdp.OverviewText.Add(message);

                rdp.PlotIds.Add("Analog0");
                rdp.PlotIds.Add("Analog1");
                rdp.PlotIds.Add("Switch0");
                rdp.PlotIds.Add("Switch1");
                rdp.PlotIds.Add("Ramp20min");
                rdp.PlotIds.Add("Ramp30min");

                MachineStatus.Instance.SendMail(
                    mail,
                    rdp
                    );
            }
        }

    }
}
