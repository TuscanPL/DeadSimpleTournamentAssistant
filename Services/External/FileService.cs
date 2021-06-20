using BLL.Interfaces;
using BLL.Internal;
using System;
using System.IO;

namespace Services.External
{
    public class FileService : IFileService
    {
        private readonly string _baseDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}\\Scores\\";
        public FileService()
        {
            Directory.CreateDirectory(_baseDirectory);
        }
        public void AppendData(FileDataDTO fileData)
        {
            File.WriteAllText($"{_baseDirectory}P1.txt", fileData.Player1Name);
            File.WriteAllText($"{_baseDirectory}P2.txt", fileData.Player2Name);
            File.WriteAllText($"{_baseDirectory}P1S.txt", fileData.Player1Score);
            File.WriteAllText($"{_baseDirectory}P2S.txt", fileData.Player2Score);
            File.WriteAllText($"{_baseDirectory}RT.txt", fileData.RoundText);
        }
    }
}
