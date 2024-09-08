namespace EmailService.MailTemplate
{
    public static class MailSubject
    {
        public static string GetMailSubject(string vendorName, string gatePassNo, DateTime scheduledDate)
        {
            var emailSubject = $@"Dear {vendorName},
I hope this message finds you well.
This is a gentle reminder regarding the return of the material of Gate pass No {gatePassNo}, which is scheduled for tomorrow, {scheduledDate:MMMM dd, yyyy}.
If there are any issues or if there are any changes in the delivery date, please inform us immediately so we can address them accordingly.";
            return emailSubject;
        }


    }
}
