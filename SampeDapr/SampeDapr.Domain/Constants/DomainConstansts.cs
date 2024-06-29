namespace SampeDapr.Domain
{
    public static class DomainConstansts
    {
        public static class EmailConsts
        {
            public const string Notification_AssignTo = "A Item has been assigned to <b>{0}</b>.";
            public const string Notification_You = "A Item has been assigned to you.";
            public const string Notification_ReceivedNew = "A Item has been received";
            public const string Notification_Amended = "Your Item was recently updated from customer.";
            public const string Notification_cancelled = "Your Item has been cancelled with reason <b>{0}</b>.";
            public const string Notification_confirmed = "Your Item has been successfully confirmed.";

            public const string Subject_Changed = "A Item Has Been Changed";
            public const string Subject_AssignToYou = "New Item Assigned To You";
            public const string Subject_Received = "New Item Received";
            public const string Subject_Amended = "A Item Has Been Updated";
            public const string Subject_Confirm = "A Item Has Been Confirmed";
            public const string Subject_Cancel = "A Item Has Been Cancelled";
        }

        public static class ImgConstants
        {
            public const string RootOfImageUrl = "##ipAddress##";
            public const string Directory = "Assets/Images/";
            public const string Base64ImgFormat = "data:image/jpg;base64,{0}";
        }
    }
}
