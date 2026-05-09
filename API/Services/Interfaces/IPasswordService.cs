namespace API.Services.Interfaces;

public interface IPasswordService
{
    string Hash(string password);

    bool verify(string password, string hash);
}
