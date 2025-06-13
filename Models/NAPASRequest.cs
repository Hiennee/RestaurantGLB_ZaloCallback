using System.Text.Json.Serialization;

namespace RestaurantGLB_Webserver.Models
{
    public class NAPASRequest
    {
        public NAPASRequestPayload payload { get; set; }
        public NAPASRequestHeader header { get; set; }
    }
    public class NAPASRequestHeader
    {
        public string messageIdentifier { get; set; }
        public string senderReference { get; set; }
        public string creationDateTime { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public string signature { get; set; }
    }
    public class NAPASRequestPayload
    {
        public string caseId { get; set; }
        public string creationDateTime { get; set; }
        public long amount { get; set; }
        public string issueDate { get; set; }
        public string id { get; set; }
        public string transDateTime { get; set; }
    }
}
