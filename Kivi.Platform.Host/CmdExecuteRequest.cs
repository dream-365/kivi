namespace Kivi.Platform.Host
{
    public class CmdExecuteRequest
    {
        public string PackageId { get; set; }

        public string Command { get; set; }

        public string Arguments { get; set; }
    }
}
