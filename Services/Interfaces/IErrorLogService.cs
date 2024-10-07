namespace BBMPCITZAPI.Services.Interfaces
{
    public interface IErrorLogService 
    {
        public void LogError(Exception oEx, string Id);
    }
}
